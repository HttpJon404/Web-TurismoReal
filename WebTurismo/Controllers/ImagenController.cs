using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilidad;

namespace WebTurismo.Controllers
{
    public class ImagenController : Controller
    {
        public ActionResult Preview(String file, String contentType)
        {
            try
            {
                return File(file, contentType, "Imagen");
            }
            catch (Exception e)
            {
                Log.Web().Error(e.Message, e);
                return null;
            }
        }

        public ActionResult PreviewPerfil(String file, String contentType)
        {
            try
            {
                if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(contentType))
                {
                    file = Server.MapPath("~/Content/Images/perfil.svg");
                    contentType = "image/jpeg";
                }

                return File(file, contentType, "Imagen");
            }
            catch (Exception e)
            {
                Log.Web().Error(e.Message, e);
                return null;
            }
        }

        public ActionResult PreviewPdf(String file, String contentType)
        {
            try
            {
                return File(file, contentType);
            }
            catch (Exception e)
            {
                Log.Web().Error(e.Message, e);
                return null;
            }
        }

        public ActionResult Documento(String ruta, String contentType)
        {


            ViewData["ruta"] = ruta;
            ViewData["contentType"] = contentType;
            return View();
        }

    }
}
