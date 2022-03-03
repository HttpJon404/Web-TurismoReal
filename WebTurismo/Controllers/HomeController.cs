using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Contexto;
using EntidadServicio;
using Negocio;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utilidad;
using Newtonsoft.Json;

namespace WebTurismo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dep = DepartamentoBl.GetInstance().GetDepartamentos();


            ViewData["Propiedades"] = dep;
            ViewData["Comunas"] = UbicacionBl.GetInstance().GetComunas();
            ViewData["Region"] = UbicacionBl.GetInstance().GetRegion();
            return View();
        }

        public ActionResult About()
        {


            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }

        private readonly int _RegistrosPorPagina = 9;
        private List<Departamento> _Customers;
        private PaginadorGenerico<Departamento> _PaginadorCustomers;

        public ActionResult Listado(int? idRegion, string idComuna, string direccion,int pagina = 1)
        {
            var dep = DepartamentoBl.GetInstance().GetDepartamentos();

            
            
            int _TotalRegistros = 0;

            // Número total de registros de dep
            if (direccion != "" && direccion != null)
            {
                direccion = direccion.ToLower();
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.direccion.ToLower().Contains(direccion)).Count();

            }
            else if (idRegion != 0 && idComuna == "sin-comuna")
            {
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && Convert.ToInt32(d.region) == idRegion).Count();

            }

            else if (idComuna != "sin-comuna" && idRegion.HasValue)
            {
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.nombre_comuna == idComuna && Convert.ToInt32(d.region) == idRegion).Count();

            }

            else if ((direccion != "" && direccion != null) && idComuna != "sin-comuna" && idRegion.HasValue)
            {
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.direccion.ToLower().Contains(direccion)  && d.nombre_comuna == idComuna && Convert.ToInt32(d.region) == idRegion).Count();
            }
            
            else if (idComuna != "sin-comuna" && idRegion == 0)
            {
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.nombre_comuna == idComuna).Count();

            }
            else
            {
                _TotalRegistros = dep.Where(d => Convert.ToInt32(d.id_estado) == 1).Count();

            }
            // Obtenemos la 'página de registros' de dep
            if (direccion != "" && direccion != null)
            {
                direccion = direccion.ToLower();
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.direccion.ToLower().Contains(direccion)).ToList();

            }
            else if (idRegion != 0 && idComuna == "sin-comuna")
            {
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && Convert.ToInt32(d.region) == idRegion).ToList();

            }
            else if (idComuna != "sin-comuna" && idRegion.HasValue)
            {
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.nombre_comuna == idComuna && Convert.ToInt32(d.region) == idRegion).ToList();
            }

            else if ((direccion != "" && direccion != null) && idComuna != "sin-comuna" && idRegion.HasValue)
            {
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.direccion.ToLower().Contains(direccion) && d.nombre_comuna == idComuna && Convert.ToInt32(d.region) == idRegion).ToList();

            }

            
            else if (idComuna != "sin-comuna" && idRegion == 0)
            {
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1 && d.nombre_comuna == idComuna).ToList();

            }
            else
            {
                _Customers = dep.Where(d => Convert.ToInt32(d.id_estado) == 1).ToList();

            }
            
                // Número total de páginas de dep
                var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
                // Instanciamos la 'Clase de paginación' y asignamos los nuevos valores
                _PaginadorCustomers = new PaginadorGenerico<Departamento>()
                {
                    RegistrosPorPagina = _RegistrosPorPagina,
                    TotalRegistros = _TotalRegistros,
                    TotalPaginas = _TotalPaginas,
                    PaginaActual = pagina,
                    Resultado = _Customers
                };
                // Enviamos a la Vista la 'Clase de paginación'
                
            


            ViewData["Propiedades"] = dep;
            ViewData["Comunas"] = UbicacionBl.GetInstance().GetComunas();
            ViewData["Region"] = UbicacionBl.GetInstance().GetRegion();

            return View(_PaginadorCustomers);
        }

        public ActionResult Propiedad(int? id)
        {
            AvUsuario usuario = (AvUsuario)Session["AVUsuario"];
            List<Departamento> departamento = null;
            List<DetalleReserva> detalle = null;
            var dep = DepartamentoBl.GetInstance().GetDepartamentos();

            if (id.HasValue)
            {
                departamento = DepartamentoBl.GetInstance().GetDepartamentoPorId(id);
            }

            if (usuario != null)
            {
                var idUsu = Convert.ToInt32(usuario.ID);
                detalle = DetalleReservaBl.GetInstance().DetalleReserva(idUsu);
                if (detalle.Count > 0)
                {
                    foreach (var i in detalle)
                    {
                        if (Convert.ToInt32(i.id_estado) == 5)
                        {
                            ViewData["reserva"] = "Tiene";
                        }
                        else
                        {
                            ViewData["reserva"] = "OK";
                        }

                    }

                }
                else
                {
                    ViewData["reserva"] = "OK";
                }
            }

            ViewData["id"] = id;
            ViewData["departamentos"] = dep;
            ViewData["Propiedad"] = departamento;

            return View();
        }

        public JsonResult RegistrarUsuario(string nombre, string apellidos, decimal edad, string rut, string idGenero, decimal idComuna, decimal idRegion,
                     string direccion, string correo, string celular, string contrasena, decimal idRol)
        {
            var resp = UsuarioBl.GetInstance().RegistrarUsuario(nombre, apellidos, edad, rut, idGenero, idComuna, idRegion, direccion, correo, celular, contrasena, idRol);
            if (resp.EsPositiva == true)
            {
                int id = Int32.Parse(resp.Elemento);
                var usuario = UsuarioBl.GetInstance().CompletaSessionUsuario(id);
                Session["AVUsuario"] = usuario;
                resp.Elemento = null;

                var resp2 = UsuarioBl.GetInstance().GenerarCorreoBienvenida(usuario);
                if (resp2.EsPositiva == true)
                {
                    resp.Mensaje = resp.Mensaje + " y " + resp2.Mensaje;
                }
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoginUsuario(string correo, string contrasena)
        {
            var resp = UsuarioBl.GetInstance().LoginUsuario(correo, contrasena);
            if (resp.EsPositiva)
            {
                int id = Int32.Parse(resp.Elemento);
                Session["AVUsuario"] = UsuarioBl.GetInstance().CompletaSessionUsuario(id);
                
                resp.Elemento = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Recuperar()
        {
            ViewData["tipoMensaje"] = TempData["tipoMensaje"];
            ViewData["tituloMensaje"] = TempData["tituloMensaje"];
            ViewData["contenidoMensaje"] = TempData["contenidoMensaje"];
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Recuperar(string email)
        {

            var usuario = UsuarioBl.GetInstance().GetUsuarioEmail(email);
            if (usuario != null)
            {
                var id = decimal.ToInt32(usuario.id);
                var resultado = UsuarioBl.GetInstance().GenerarLinkContrasena(id, 2);
                if (resultado)
                {
                    ViewData["tipoMensaje"] = "success";
                    ViewData["tituloMensaje"] = "CORRECTO";
                    ViewData["contenidoMensaje"] = "Se le ha enviado un correo con las indicaciones para recuperar su contrasenaa.";
                    //return RedirectToAction("Index");
                }
                else
                {
                    ViewData["tipoMensaje"] = "danger";
                    ViewData["tituloMensaje"] = "ERROR";
                    ViewData["contenidoMensaje"] = "No se pudo generar la recuperación de contrasena. Reintente mas tarde.";
                }
            }
            else
            {
                ViewData["tipoMensaje"] = "danger";
                ViewData["tituloMensaje"] = "ERROR";
                ViewData["contenidoMensaje"] = "Error Reintente nuevamente.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Restablecer(string id)
        {
            var clink = UsuarioBl.GetInstance().GetLinkContrasena(id);
            if (clink == null)
            {
                TempData["tipoMensaje"] = "danger";
                TempData["tituloMensaje"] = "ERROR";
                TempData["contenidoMensaje"] =
                    "Ha ocurrido un error inesperado, reintente nuevamente.";
                return RedirectToAction("Recuperar");
            }
            else
            {

                ViewData["Id"] = id;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Restablecer(string id, string pass, string pass2)
        {
            var clink = UsuarioBl.GetInstance().GetLinkContrasena(id);
            if (clink == null)
            {
                TempData["tipoMensaje"] = "danger";
                TempData["tituloMensaje"] = "ERROR";
                TempData["contenidoMensaje"] =
                    "El tiempo disponible para generar su contraseña ha caducado.";
                return RedirectToAction("Recuperar");
            }
            else
            {
                if (pass.Length < 6)
                {
                    ViewData["tipoMensaje"] = "danger";
                    ViewData["tituloMensaje"] = "ERROR";
                    ViewData["contenidoMensaje"] =
                        "La nueva contraseña debe tener como mínimo 6 caracteres. Favor inténtelo nuevamente.";
                    ViewData["Id"] = id;
                }
                else if (!pass.Equals(pass2))
                {
                    ViewData["tipoMensaje"] = "danger";
                    ViewData["tituloMensaje"] = "ERROR";
                    ViewData["contenidoMensaje"] =
                        "Las contraseñas no coinciden. Favor inténtelo nuevamente.";
                    ViewData["Id"] = id;
                }
                else
                {
                    string newPass = CripSha1.Encriptar(pass);
                    bool resultado = UsuarioBl.GetInstance().CambiarContrasena(id, newPass);
                    if (resultado)
                    {
                        TempData["tipoMensaje"] = "success";
                        TempData["tituloMensaje"] = "CORRECTO";
                        TempData["contenidoMensaje"] =
                            "Contraseña actualizada correctamente.";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewData["tipoMensaje"] = "danger";
                        ViewData["tituloMensaje"] = "ERROR";
                        ViewData["contenidoMensaje"] =
                            "Ocurrió un problema al guardar la contraseña. Favor inténtelo nuevamente.";
                        ViewData["Id"] = id;
                    }
                }
                return View();
            }
        }


        public JsonResult GetRegionPorComuna(int id)
        {
            var resp = UbicacionBl.GetInstance().GetRegionPorComuna(id);


            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleReserva(int? id)
        {
            List<Departamento> departamento = null;


            if (id.HasValue)
            {
                departamento = DepartamentoBl.GetInstance().GetDepartamentoPorId(id);
            }

            ViewData["Propiedad"] = departamento;


            return View();
        }

        public ActionResult Pago(int? id, DateTime? fecha_llegada, DateTime? fecha_salida, int? valorReserva,
            int? valorRestante, int? valorTotalDias, int? valorDiario, string listaAcompanantes, string listaServicios, int? valorExtras, int? idDetalle, int? multa)
        {
            List<Departamento> departamento = null;

            if (id.HasValue)
            {
                departamento = DepartamentoBl.GetInstance().GetDepartamentoPorId(id);
            }

            if (!multa.HasValue)
            {
                multa = 0;
            }
            if (!idDetalle.HasValue)
            {
                idDetalle = 0;
            }

            ViewData["Propiedad"] = departamento;
            ViewData["FechaLlegada"] = fecha_llegada;
            ViewData["FechaSalida"] = fecha_salida;
            ViewData["valorReserva"] = valorReserva;
            ViewData["valorRestante"] = valorRestante + multa;
            ViewData["valorTotalDias"] = valorTotalDias;
            if (id.HasValue && id != 0)
            {
                ViewData["valorDiario"] = departamento[0].valor_arriendo;

            }
            else
            {
                ViewData["valorDiario"] = 0;

            }
            ViewData["valorExtras"] = valorExtras;
            ViewData["listaAcompanantes"] = listaAcompanantes;
            ViewData["listaServicios"] = listaServicios;
            ViewData["detalle"] = idDetalle;
            
            return View();
        }

        public ActionResult Acompanante(int id)
        {
            
            ViewData["id"] = id;

            return PartialView();
        }

        public JsonResult PagoConfirmado(int idDpto, int idUsuario,DateTime fecha_llegada, DateTime fecha_salida, int valorReserva,
            int valorRestante, int valorTotalDias, int valorDiario,string listaAcompanantes, string listaServicios, string pago)
        {
            //tipo 1 es pago de reserva, si es tipo 2 es el pago del check-out
           
                var resp = UsuarioBl.GetInstance().PagoConfirmado(idDpto, idUsuario, fecha_llegada, fecha_salida, valorReserva, valorRestante, valorTotalDias, valorDiario, listaAcompanantes, listaServicios, pago);
                var usuario = UsuarioBl.GetInstance().CompletaSessionUsuario(idUsuario);

                if (resp.EsPositiva == true)
                {
                    var resp2 = UsuarioBl.GetInstance().GenerarCorreoReserva(usuario);

                    resp.Mensaje = resp.Mensaje + " y " + resp2.Mensaje;
                }
            
          

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiciosExtra(int id)
        {
            ViewData["id"] = id;
            List<Servicios> servicios = null;
            servicios = DepartamentoBl.GetInstance().GetServicios();
            ViewData["Servicios"] = servicios;


            return PartialView();
        }
        
        public JsonResult PagoRestante(int id)
        {
            var resp = UsuarioBl.GetInstance().PagoRestante(id);
            AvUsuario usuario = (AvUsuario)Session["AVUsuario"];

            if (resp.EsPositiva)
            {
                UsuarioBl.GetInstance().GenerarCorreoPagoRestante(usuario);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
    }
}