using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public async Task<ActionResult> Index()
        {
            try
            {
                // Obtener estadísticas generales
                var estadisticasUsuarios = await _usuarioService.ObtenerEstadisticas();
                var estadisticasHistoriales = await _historialService.ObtenerEstadisticas();
                var estadisticasReportes = await _reporteService.ObtenerEstadisticas();

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
        public async Task<ActionResult> Analytics()
        {
            try
            {
                var estadisticasReportes = await _reporteService.ObtenerEstadisticas();
                var estadisticasHistoriales = await _historialService.ObtenerEstadisticas();

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
        public async Task<ActionResult> MapaInteractivo()
        {
            try
            {
                var reportes = await _reporteService.ObtenerTodos();
                var reportesConUbicacion = reportes.FindAll(r => r.HasLocation);
                
                return View(reportesConUbicacion);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar mapa: {ex.Message}";
                return View();
            }
        }

        // GET: DashboardRespiCare/Estadisticas
        public async Task<ActionResult> Estadisticas()
        {
            try
            {
                var estadisticasUsuarios = await _usuarioService.ObtenerEstadisticas();
                var estadisticasHistoriales = await _historialService.ObtenerEstadisticas();
                var estadisticasReportes = await _reporteService.ObtenerEstadisticas();

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
        public async Task<ActionResult> EstadoServicioIA()
        {
            try
            {
                var disponible = await _aiService.EstaDisponible();
                ViewBag.Disponible = disponible;

                if (disponible)
                {
                    var estado = await _aiService.ObtenerEstadoServicio();
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
        public async Task<ActionResult> Urgencias()
        {
            try
            {
                var historialesUrgentes = await _historialService.ObtenerUrgentes();
                var reportesUrgentes = await _reporteService.ObtenerUrgentes();

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
        public async Task<ActionResult> TendenciasTemporal()
        {
            try
            {
                var fechaInicio = DateTime.Now.AddMonths(-3);
                var fechaFin = DateTime.Now;

                var historiales = await _historialService.ObtenerPorFechas(fechaInicio, fechaFin);
                var reportes = await _reporteService.ObtenerPorFechas(fechaInicio, fechaFin);

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

