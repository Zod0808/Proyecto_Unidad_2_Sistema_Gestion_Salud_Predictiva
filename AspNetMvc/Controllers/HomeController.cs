using System;
using System.Web.Mvc;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Si el usuario ya está logueado, redirigir según su tipo
            if (Session["UsuarioId"] != null)
            {
                var tipoUsuario = (Models.TipoUsuario?)Session["TipoUsuario"];
                if (tipoUsuario == Models.TipoUsuario.Paciente)
                {
                    return RedirectToAction("Dashboard", "Paciente");
                }
                else if (tipoUsuario == Models.TipoUsuario.Medico)
                {
                    return RedirectToAction("Dashboard", "Medico");
                }
            }

            // Si no está logueado, mostrar página de inicio con opción de login
            return RedirectToAction("Login", "Auths");
        }
    }
}