using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Diagnostics;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class ReporteSintomasController : BaseController
    {
        private readonly ReporteSintomasService _reporteService;

        public ReporteSintomasController()
        {
            _reporteService = new ReporteSintomasService();
        }

        /// <summary>
        /// Lista todos los reportes del paciente
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                var pacienteId = ObtenerUsuarioId().ToString();
                List<ReporteSintomas> reportes = new List<ReporteSintomas>();
                
                try
                {
                    var reportesTask = _reporteService.ObtenerPorPaciente(pacienteId);
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        reportes = reportesTask.Result;
                    }
                    else
                    {
                        Debug.WriteLine("Timeout al obtener reportes desde MongoDB");
                    }
                }
                catch (Exception mongoEx)
                {
                    Debug.WriteLine($"Error MongoDB en ReporteSintomas Index: {mongoEx.Message}");
                    reportes = new List<ReporteSintomas>();
                }

                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar los reportes: " + ex.Message;
                return View(new List<ReporteSintomas>());
            }
        }

        /// <summary>
        /// Muestra el formulario para crear un nuevo reporte
        /// </summary>
        public ActionResult Crear()
        {
            var modelo = new CrearReporteSintomasViewModel();
            ViewBag.SintomasDisponibles = SintomasDisponibles.ObtenerTodos();
            return View(modelo);
        }

        /// <summary>
        /// Procesa el formulario de creación de reporte
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(CrearReporteSintomasViewModel modelo, string[] sintomas)
        {
            try
            {
                if (sintomas == null || sintomas.Length == 0)
                {
                    ModelState.AddModelError("", "Debe seleccionar al menos un síntoma");
                    ViewBag.SintomasDisponibles = SintomasDisponibles.ObtenerTodos();
                    return View(modelo);
                }

                var pacienteId = ObtenerUsuarioId().ToString();
                var nombrePaciente = Session["UsuarioNombre"]?.ToString() ?? "Usuario";

                // Crear el reporte
                var reporte = new ReporteSintomas
                {
                    PacienteId = pacienteId,
                    NombrePaciente = nombrePaciente,
                    FechaReporte = DateTime.Now,
                    Ubicacion = modelo.Ubicacion ?? "No especificada",
                    NotasAdicionales = modelo.Descripcion,
                    Sintomas = new List<SintomaDetalle>()
                };

                // Agregar síntomas seleccionados
                foreach (var sintomaSeleccionado in sintomas)
                {
                    reporte.Sintomas.Add(new SintomaDetalle
                    {
                        Nombre = sintomaSeleccionado,
                        Gravedad = modelo.Gravedad,
                        Duracion = modelo.Duracion,
                        Descripcion = modelo.Descripcion
                    });
                }

                // Si hay temperatura, agregar como síntoma adicional
                if (modelo.Temperatura.HasValue && modelo.Temperatura >= 37.5)
                {
                    var gravedadFiebre = modelo.Temperatura >= 39 ? "Grave" : 
                                        modelo.Temperatura >= 38 ? "Moderado" : "Leve";
                    
                    reporte.Sintomas.Add(new SintomaDetalle
                    {
                        Nombre = $"Fiebre ({modelo.Temperatura}°C)",
                        Gravedad = gravedadFiebre,
                        Duracion = modelo.Duracion,
                        Descripcion = $"Temperatura corporal: {modelo.Temperatura}°C"
                    });
                }

                // Guardar en MongoDB
                try
                {
                    var crearTask = _reporteService.CrearReporte(reporte);
                    if (crearTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        var reporteCreado = crearTask.Result;
                        TempData["Success"] = "Reporte de síntomas creado exitosamente";
                        return RedirectToAction("Detalle", new { id = reporteCreado.Id });
                    }
                    else
                    {
                        throw new TimeoutException("Timeout al crear reporte");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al crear reporte: {ex.Message}");
                    TempData["Error"] = "Error al guardar el reporte. Por favor, intenta de nuevo.";
                    ViewBag.SintomasDisponibles = SintomasDisponibles.ObtenerTodos();
                    return View(modelo);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear el reporte: " + ex.Message;
                ViewBag.SintomasDisponibles = SintomasDisponibles.ObtenerTodos();
                return View(modelo);
            }
        }

        /// <summary>
        /// Muestra el detalle de un reporte específico
        /// </summary>
        public ActionResult Detalle(string id)
        {
            try
            {
                ReporteSintomas reporte = null;
                
                try
                {
                    var reporteTask = _reporteService.ObtenerPorId(id);
                    if (reporteTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        reporte = reporteTask.Result;
                    }
                    else
                    {
                        Debug.WriteLine("Timeout al obtener reporte desde MongoDB");
                    }
                }
                catch (Exception mongoEx)
                {
                    Debug.WriteLine($"Error MongoDB en ReporteSintomas Detalle: {mongoEx.Message}");
                }

                if (reporte == null)
                {
                    TempData["Error"] = "Reporte no encontrado";
                    return RedirectToAction("Index");
                }

                // Verificar que el reporte pertenece al paciente actual
                var pacienteId = ObtenerUsuarioId().ToString();
                if (reporte.PacienteId != pacienteId)
                {
                    TempData["Error"] = "No tiene permiso para ver este reporte";
                    return RedirectToAction("Index");
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el reporte: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Obtiene estadísticas de síntomas (AJAX)
        /// </summary>
        public JsonResult Estadisticas()
        {
            try
            {
                Dictionary<string, int> sintomasMasReportados = new Dictionary<string, int>();
                Dictionary<string, int> estadisticasUbicacion = new Dictionary<string, int>();
                
                try
                {
                    var sintomasTask = _reporteService.ObtenerSintomasMasReportados();
                    var ubicacionTask = _reporteService.ObtenerEstadisticasPorUbicacion();
                    
                    if (sintomasTask.Wait(TimeSpan.FromSeconds(3)) && ubicacionTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        sintomasMasReportados = sintomasTask.Result;
                        estadisticasUbicacion = ubicacionTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al obtener estadísticas: {ex.Message}");
                }

                return Json(new 
                { 
                    sintomasMasReportados = sintomasMasReportados,
                    estadisticasUbicacion = estadisticasUbicacion
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
