using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    /// <summary>
    /// Controlador para gestionar usuarios del sistema RespiCare
    /// </summary>
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController()
        {
            _usuarioService = new UsuarioService();
        }

        // GET: Usuario
        public async Task<ActionResult> Index()
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerTodos();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar usuarios: {ex.Message}";
                return View();
            }
        }

        // GET: Usuario/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var usuario = await _usuarioService.ObtenerPorId(id);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar usuario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UsuarioRespiCare usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _usuarioService.Crear(usuario);
                    TempData["Success"] = "Usuario creado exitosamente";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear usuario: {ex.Message}";
                return View(usuario);
            }
        }

        // GET: Usuario/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var usuario = await _usuarioService.ObtenerPorId(id);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar usuario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UsuarioRespiCare usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultado = await _usuarioService.Actualizar(usuario);
                    if (resultado)
                    {
                        TempData["Success"] = "Usuario actualizado exitosamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = "No se pudo actualizar el usuario";
                    }
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar usuario: {ex.Message}";
                return View(usuario);
            }
        }

        // GET: Usuario/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            try
            {
                var usuario = await _usuarioService.ObtenerPorId(id);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar usuario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var resultado = await _usuarioService.Eliminar(id);
                if (resultado)
                {
                    TempData["Success"] = "Usuario eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el usuario";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar usuario: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuario/Estadisticas
        public async Task<ActionResult> Estadisticas()
        {
            try
            {
                var estadisticas = await _usuarioService.ObtenerEstadisticas();
                return View(estadisticas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar estadísticas: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: Usuario/PorRol
        public async Task<ActionResult> PorRol(UserRole rol)
        {
            try
            {
                var usuarios = await _usuarioService.ObtenerPorRol(rol);
                ViewBag.Rol = rol;
                return View(usuarios);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar usuarios por rol: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}

