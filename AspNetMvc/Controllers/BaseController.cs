using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UsuarioId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Auths/Login");
                return;
            }

            ViewBag.UsuarioNombre = Session["UsuarioNombre"];
            ViewBag.TipoUsuario = Session["TipoUsuario"];

            base.OnActionExecuting(filterContext);
        }

        protected int ObtenerUsuarioId()
        {
            return (int)Session["UsuarioId"];
        }

        protected TipoUsuario ObtenerTipoUsuario()
        {
            return (TipoUsuario)Session["TipoUsuario"];
        }
    }
}