using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    /// <summary>
    /// Controlador para gestionar reportes de síntomas
    /// </summary>
    public class ReporteSintomasController : Controller
    {
        private readonly ReporteSintomasService _reporteService;

        public ReporteSintomasController()
        {
            _reporteService = new ReporteSintomasService();
        }

        // GET: ReporteSintomas
        public async Task<ActionResult> Index()
        {
            try
            {
                var reportes = await _reporteService.ObtenerTodos();
                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reportes: {ex.Message}";
                return View();
            }
        }

        // GET: ReporteSintomas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var reporte = await _reporteService.ObtenerPorId(id);
                if (reporte == null)
                {
                    TempData["Error"] = "Reporte no encontrado";
                    return RedirectToAction("Index");
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reporte: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: ReporteSintomas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReporteSintomas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReporteSintomas reporte)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // El servicio ya se encarga de analizar con IA
                    await _reporteService.Crear(reporte);
                    TempData["Success"] = "Reporte creado exitosamente y analizado por IA";
                    return RedirectToAction("Details", new { id = reporte.Id });
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear reporte: {ex.Message}";
                return View(reporte);
            }
        }

        // GET: ReporteSintomas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var reporte = await _reporteService.ObtenerPorId(id);
                if (reporte == null)
                {
                    TempData["Error"] = "Reporte no encontrado";
                    return RedirectToAction("Index");
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reporte: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: ReporteSintomas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReporteSintomas reporte)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _reporteService.Actualizar(reporte);
                    if (resultado)
                    {
                        TempData["Success"] = "Reporte actualizado exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo actualizar el reporte";
                    }
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar reporte: {ex.Message}";
                return View(reporte);
            }
        }

        // GET: ReporteSintomas/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var reporte = await _reporteService.ObtenerPorId(id);
                if (reporte == null)
                {
                    TempData["Error"] = "Reporte no encontrado";
                    return RedirectToAction("Index");
                }

                return View(reporte);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reporte: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: ReporteSintomas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var resultado = await _reporteService.Eliminar(id);
                if (resultado)
                {
                    TempData["Success"] = "Reporte eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el reporte";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar reporte: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: ReporteSintomas/Urgentes
        public async Task<ActionResult> Urgentes()
        {
            try
            {
                var reportes = await _reporteService.ObtenerUrgentes();
                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reportes urgentes: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: ReporteSintomas/PorEstado
        public async Task<ActionResult> PorEstado(ReportStatus estado)
        {
            try
            {
                var reportes = await _reporteService.ObtenerPorEstado(estado);
                ViewBag.Estado = estado;
                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar reportes por estado: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: ReporteSintomas/Estadisticas
        public async Task<ActionResult> Estadisticas()
        {
            try
            {
                var estadisticas = await _reporteService.ObtenerEstadisticas();
                return View(estadisticas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar estadísticas: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: ReporteSintomas/CambiarEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarEstado(string id, ReportStatus nuevoEstado)
        {
            try
            {
                var resultado = await _reporteService.CambiarEstado(id, nuevoEstado);
                if (resultado)
                {
                    TempData["Success"] = "Estado cambiado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo cambiar el estado";
                }

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cambiar estado: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: ReporteSintomas/Mapa
        public async Task<ActionResult> Mapa()
        {
            try
            {
                var reportes = await _reporteService.ObtenerTodos();
                // Filtrar solo los que tienen ubicación
                var reportesConUbicacion = reportes.FindAll(r => r.HasLocation);
                return View(reportesConUbicacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar mapa: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

