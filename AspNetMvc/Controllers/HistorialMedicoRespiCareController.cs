using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    /// <summary>
    /// Controlador para gestionar historiales médicos del sistema RespiCare
    /// </summary>
    public class HistorialMedicoRespiCareController : Controller
    {
        private readonly HistorialMedicoService _historialService;

        public HistorialMedicoRespiCareController()
        {
            _historialService = new HistorialMedicoService();
        }

        // GET: HistorialMedicoRespiCare
        public async Task<ActionResult> Index()
        {
            try
            {
                var historiales = await _historialService.ObtenerTodos();
                return View(historiales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historiales: {ex.Message}";
                return View();
            }
        }

        // GET: HistorialMedicoRespiCare/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historial = await _historialService.ObtenerPorId(id);
                if (historial == null)
                {
                    TempData["Error"] = "Historial no encontrado";
                    return RedirectToAction("Index");
                }

                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historial: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HistorialMedicoRespiCare/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HistorialMedicoRespiCare historial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _historialService.Crear(historial);
                    TempData["Success"] = "Historial médico creado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear historial: {ex.Message}";
                return View(historial);
            }
        }

        // GET: HistorialMedicoRespiCare/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historial = await _historialService.ObtenerPorId(id);
                if (historial == null)
                {
                    TempData["Error"] = "Historial no encontrado";
                    return RedirectToAction("Index");
                }

                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historial: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: HistorialMedicoRespiCare/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HistorialMedicoRespiCare historial)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _historialService.Actualizar(historial);
                    if (resultado)
                    {
                        TempData["Success"] = "Historial actualizado exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo actualizar el historial";
                    }
                }

                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar historial: {ex.Message}";
                return View(historial);
            }
        }

        // GET: HistorialMedicoRespiCare/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historial = await _historialService.ObtenerPorId(id);
                if (historial == null)
                {
                    TempData["Error"] = "Historial no encontrado";
                    return RedirectToAction("Index");
                }

                return View(historial);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historial: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: HistorialMedicoRespiCare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var resultado = await _historialService.Eliminar(id);
                if (resultado)
                {
                    TempData["Success"] = "Historial eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el historial";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar historial: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/PorPaciente/id
        public async Task<ActionResult> PorPaciente(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historiales = await _historialService.ObtenerPorPaciente(id);
                ViewBag.PatientId = id;
                return View(historiales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historiales del paciente: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/PorDoctor/id
        public async Task<ActionResult> PorDoctor(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historiales = await _historialService.ObtenerPorDoctor(id);
                ViewBag.DoctorId = id;
                return View(historiales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historiales del doctor: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/Urgentes
        public async Task<ActionResult> Urgentes()
        {
            try
            {
                var historiales = await _historialService.ObtenerUrgentes();
                return View(historiales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar historiales urgentes: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/Estadisticas
        public async Task<ActionResult> Estadisticas()
        {
            try
            {
                var estadisticas = await _historialService.ObtenerEstadisticas();
                return View(estadisticas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar estadísticas: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: HistorialMedicoRespiCare/Buscar
        public async Task<ActionResult> Buscar(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var historiales = await _historialService.Buscar(q);
                ViewBag.Query = q;
                return View(historiales);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al buscar historiales: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

