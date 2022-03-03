using EntidadServicio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTurismo.Controllers
{
    public class PaginadorController : Controller
    {
        // GET: Paginador

        private readonly int _RegistrosPorPagina = 10;

        

        public ActionResult SelectorPaginas(int pagina = 1)
        {
           

            
            return PartialView();
        }
    }
}