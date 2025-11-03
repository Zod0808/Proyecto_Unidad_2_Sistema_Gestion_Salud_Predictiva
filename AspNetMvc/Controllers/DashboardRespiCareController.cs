using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Diagnostics;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    /// <summary>
    /// Controlador para el Dashboard principal de RespiCare con Analytics
    /// </summary>
    public class DashboardRespiCareController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly HistorialMedicoService _historialService;
        private readonly ReporteSintomasService _reporteService;
        private readonly AIService _aiService;

        public DashboardRespiCareController()
        {
            _usuarioService = new UsuarioService();
            _historialService = new HistorialMedicoService();
            _reporteService = new ReporteSintomasService();
            _aiService = new AIService();
        }

        // GET: DashboardRespiCare
        public ActionResult Index()
        {
            try
            {
                // Obtener estadísticas generales (con timeout)
                Dictionary<string, int> estadisticasUsuarios = new Dictionary<string, int>();
                Dictionary<string, object> estadisticasHistoriales = new Dictionary<string, object>();
                Dictionary<string, object> estadisticasReportes = new Dictionary<string, object>();
                
                try
                {
                    var usuariosTask = _usuarioService.ObtenerEstadisticas();
                    if (usuariosTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasUsuarios = usuariosTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas usuarios: {ex.Message}");
                }
                
                try
                {
                    var historialesTask = _historialService.ObtenerEstadisticas();
                    if (historialesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasHistoriales = historialesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas historiales: {ex.Message}");
                }
                
                try
                {
                    var reportesTask = _reporteService.ObtenerEstadisticas();
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasReportes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas reportes: {ex.Message}");
                }

                // Crear modelo de vista
                var modelo = new Dictionary<string, object>
                {
                    { "Usuarios", estadisticasUsuarios },
                    { "Historiales", estadisticasHistoriales },
                    { "Reportes", estadisticasReportes },
                    { "Fecha", DateTime.Now }
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar dashboard: {ex.Message}";
                return View(new Dictionary<string, object>());
            }
        }

        // GET: DashboardRespiCare/Analytics
        public ActionResult Analytics()
        {
            try
            {
                Dictionary<string, object> estadisticasReportes = new Dictionary<string, object>();
                Dictionary<string, object> estadisticasHistoriales = new Dictionary<string, object>();
                
                try
                {
                    var reportesTask = _reporteService.ObtenerEstadisticas();
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasReportes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas reportes: {ex.Message}");
                }
                
                try
                {
                    var historialesTask = _historialService.ObtenerEstadisticas();
                    if (historialesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasHistoriales = historialesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas historiales: {ex.Message}");
                }

                var modelo = new Dictionary<string, object>
                {
                    { "Reportes", estadisticasReportes },
                    { "Historiales", estadisticasHistoriales }
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar analytics: {ex.Message}";
                return View(new Dictionary<string, object>());
            }
        }

        // GET: DashboardRespiCare/MapaInteractivo
        public ActionResult MapaInteractivo()
        {
            try
            {
                List<Models.ReporteSintomas> reportes = new List<Models.ReporteSintomas>();
                
                try
                {
                    var reportesTask = _reporteService.ObtenerTodos();
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        reportes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB mapa: {ex.Message}");
                }
                
                var reportesConUbicacion = reportes.FindAll(r => !string.IsNullOrEmpty(r.Ubicacion) && r.Ubicacion != "No especificada");
                
                return View(reportesConUbicacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar mapa: {ex.Message}";
                return View();
            }
        }

        // GET: DashboardRespiCare/Estadisticas
        public ActionResult Estadisticas()
        {
            try
            {
                Dictionary<string, int> estadisticasUsuarios = new Dictionary<string, int>();
                Dictionary<string, object> estadisticasHistoriales = new Dictionary<string, object>();
                Dictionary<string, object> estadisticasReportes = new Dictionary<string, object>();
                
                try
                {
                    var usuariosTask = _usuarioService.ObtenerEstadisticas();
                    if (usuariosTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasUsuarios = usuariosTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas usuarios: {ex.Message}");
                }
                
                try
                {
                    var historialesTask = _historialService.ObtenerEstadisticas();
                    if (historialesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasHistoriales = historialesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas historiales: {ex.Message}");
                }
                
                try
                {
                    var reportesTask = _reporteService.ObtenerEstadisticas();
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        estadisticasReportes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB estadísticas reportes: {ex.Message}");
                }

                var modelo = new Dictionary<string, Dictionary<string, object>>
                {
                    { "Usuarios", new Dictionary<string, object>(estadisticasUsuarios.ToDictionary(k => k.Key, v => (object)v.Value)) },
                    { "Historiales", new Dictionary<string, object>(estadisticasHistoriales) },
                    { "Reportes", new Dictionary<string, object>(estadisticasReportes) }
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar estadísticas: {ex.Message}";
                return View(new Dictionary<string, Dictionary<string, object>>());
            }
        }

        // GET: DashboardRespiCare/EstadoServicioIA
        public ActionResult EstadoServicioIA()
        {
            try
            {
                bool disponible = false;
                AIHealthResponse estado = null;
                
                try
                {
                    var disponibleTask = _aiService.EstaDisponible();
                    if (disponibleTask.Wait(TimeSpan.FromSeconds(3)))
                    {
                        disponible = disponibleTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error verificar AI disponible: {ex.Message}");
                }
                
                ViewBag.Disponible = disponible;

                if (disponible)
                {
                    try
                    {
                        var estadoTask = _aiService.ObtenerEstadoServicio();
                        if (estadoTask.Wait(TimeSpan.FromSeconds(3)))
                        {
                            estado = estadoTask.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error obtener estado AI: {ex.Message}");
                    }
                    return View(estado);
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al verificar servicio de IA: {ex.Message}";
                ViewBag.Disponible = false;
                return View();
            }
        }

        // GET: DashboardRespiCare/Urgencias
        public ActionResult Urgencias()
        {
            try
            {
                List<Models.HistorialMedicoRespiCare> historialesUrgentes = new List<Models.HistorialMedicoRespiCare>();
                List<Models.ReporteSintomas> reportesUrgentes = new List<Models.ReporteSintomas>();
                
                try
                {
                    var historialesTask = _historialService.ObtenerUrgentes();
                    if (historialesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        historialesUrgentes = historialesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB historiales urgentes: {ex.Message}");
                }
                
                try
                {
                    var reportesTask = _reporteService.ObtenerUrgentes();
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        reportesUrgentes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB reportes urgentes: {ex.Message}");
                }

                var modelo = new Dictionary<string, object>
                {
                    { "HistorialesUrgentes", historialesUrgentes },
                    { "ReportesUrgentes", reportesUrgentes }
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar urgencias: {ex.Message}";
                return View(new Dictionary<string, object>());
            }
        }

        // GET: DashboardRespiCare/TendenciasTemporal
        public ActionResult TendenciasTemporal()
        {
            try
            {
                var fechaInicio = DateTime.Now.AddMonths(-3);
                var fechaFin = DateTime.Now;

                List<Models.HistorialMedicoRespiCare> historiales = new List<Models.HistorialMedicoRespiCare>();
                List<Models.ReporteSintomas> reportes = new List<Models.ReporteSintomas>();
                
                try
                {
                    var historialesTask = _historialService.ObtenerPorFechas(fechaInicio, fechaFin);
                    if (historialesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        historiales = historialesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB historiales por fechas: {ex.Message}");
                }
                
                try
                {
                    var reportesTask = _reporteService.ObtenerPorFechas(fechaInicio, fechaFin);
                    if (reportesTask.Wait(TimeSpan.FromSeconds(5)))
                    {
                        reportes = reportesTask.Result;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error MongoDB reportes por fechas: {ex.Message}");
                }

                var modelo = new Dictionary<string, object>
                {
                    { "Historiales", historiales },
                    { "Reportes", reportes },
                    { "FechaInicio", fechaInicio },
                    { "FechaFin", fechaFin }
                };

                return View(modelo);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar tendencias: {ex.Message}";
                return View(new Dictionary<string, object>());
            }
        }
    }
}



