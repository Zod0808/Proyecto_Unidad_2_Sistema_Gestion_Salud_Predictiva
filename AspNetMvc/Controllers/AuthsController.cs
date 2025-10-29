using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class AuthsController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var usuario = AutenticarUsuario(model.Email, model.Password);
                if (usuario != null)
                {
                    // Establecer sesión
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioNombre"] = usuario.NombreCompleto;
                    Session["TipoUsuario"] = usuario.TipoUsuario;

                    // Redirección según tipo de usuario
                    if (usuario.TipoUsuario == TipoUsuario.Paciente)
                    {
                        return RedirectToAction("Dashboard", "Paciente");
                    }
                    else if (usuario.TipoUsuario == TipoUsuario.Medico)
                    {
                        return RedirectToAction("Dashboard", "Medico");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email o contraseña incorrectos");
                }
            }

            return View(model);
        }

        // GET: Registro
        public ActionResult Registro()
        {
            ViewBag.Especialidades = ObtenerEspecialidades();
            return View();
        }

        // POST: Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya existe
                if (EmailExiste(model.Email))
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado");
                    ViewBag.Especialidades = ObtenerEspecialidades();
                    return View(model);
                }

                // Crear nuevo usuario
                var usuario = CrearUsuario(model);
                if (usuario != null)
                {
                    // Establecer sesión
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioNombre"] = usuario.NombreCompleto;
                    Session["TipoUsuario"] = usuario.TipoUsuario;

                    // Redirección según tipo de usuario
                    if (usuario.TipoUsuario == TipoUsuario.Paciente)
                    {
                        return RedirectToAction("Dashboard", "Paciente");
                    }
                    else if (usuario.TipoUsuario == TipoUsuario.Medico)
                    {
                        return RedirectToAction("Dashboard", "Medico");
                    }
                }
            }

            ViewBag.Especialidades = ObtenerEspecialidades();
            return View(model);
        }

        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        #region Métodos privados

        private Usuario AutenticarUsuario(string email, string password)
        {
            // TODO: Implementar autenticación con base de datos
            // Por ahora, usuarios de ejemplo
            var usuarios = ObtenerUsuariosEjemplo();
            return usuarios.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }

        private bool EmailExiste(string email)
        {
            // TODO: Implementar verificación con base de datos
            var usuarios = ObtenerUsuariosEjemplo();
            return usuarios.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private Usuario CrearUsuario(RegistroViewModel model)
        {
            // TODO: Implementar creación en base de datos
            var usuario = new Usuario
            {
                Id = new Random().Next(1000, 9999), // ID temporal
                Email = model.Email,
                Password = model.Password, // TODO: Hashear password
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Telefono = model.Telefono,
                TipoUsuario = model.TipoUsuario,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            return usuario;
        }

        private List<Usuario> ObtenerUsuariosEjemplo()
        {
            return new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Email = "paciente@test.com",
                    Password = "123456",
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Telefono = "555-0001",
                    TipoUsuario = TipoUsuario.Paciente
                },
                new Usuario
                {
                    Id = 2,
                    Email = "medico@test.com",
                    Password = "123456",
                    Nombre = "Dr. María",
                    Apellido = "González",
                    Telefono = "555-0002",
                    TipoUsuario = TipoUsuario.Medico
                }
            };
        }

        private List<Especialidad> ObtenerEspecialidades()
        {
            return new List<Especialidad>
            {
                new Especialidad { Id = 1, Nombre = "Medicina General" },
                new Especialidad { Id = 2, Nombre = "Cardiología" },
                new Especialidad { Id = 3, Nombre = "Dermatología" },
                new Especialidad { Id = 4, Nombre = "Pediatría" },
                new Especialidad { Id = 5, Nombre = "Ginecología" },
                new Especialidad { Id = 6, Nombre = "Traumatología" }
            };
        }

        #endregion
    }
}