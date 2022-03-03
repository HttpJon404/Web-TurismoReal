using EntidadServicio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTurismo.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult MiPerfilDetalle()
        {
            AvUsuario usuario = (AvUsuario)Session["AVUsuario"];
            List<DetalleReserva> detalle = null;
            if (usuario == null)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                ViewData["usuario"] = usuario;
                var id = Convert.ToInt32(usuario.ID);
                detalle = DetalleReservaBl.GetInstance().DetalleReserva(id);
                if (detalle.Count>0)
                {
                    ViewData["reserva"] = detalle;
                }
                else
                {
                    ViewData["reserva"] = null;
                }
                
            }
            

            return View();
        }

        public JsonResult LogoutUsuario()
        {
            var resp = UsuarioBl.GetInstance().LogoutUsuario();
            if (resp.EsPositiva)
            {
                Session["AVUsuario"] = null;
                resp.Elemento = null;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GenerarDetalle(int idDetalle)
        {
            List<DetalleReserva> detalle = null;
            AvUsuario usuario = (AvUsuario)Session["AVUsuario"];

            var id = Convert.ToInt32(usuario.ID);
            detalle = DetalleReservaBl.GetInstance().DetalleReserva(id);

            var det = detalle.Where(d => d.id == idDetalle).FirstOrDefault();

            string pathDoc = System.Configuration.ConfigurationManager.AppSettings["RutaArchivos"];


            // Creamos el documento con el tamaño de página tradicional
            Document doc = new Document(PageSize.LETTER);

            
            // Indicamos donde vamos a guardar el documento
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(System.IO.Path.Combine(pathDoc, "prueba.pdf"), FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            
            doc.AddCreator("Turismo Real");

            // Abrimos el archivo
            doc.Open();

            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // Escribimos el encabezamiento en el documento
            doc.AddTitle("Reserva Turismo Real");
            string pathImage = Server.MapPath("/Content/images/logo.png");
            Image logo = Image.GetInstance(pathImage);
            logo.Alignment = Image.ALIGN_LEFT;
            doc.Add(logo);

            doc.Add(new Paragraph("Detalle Reserva"));
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá la data
            PdfPTable tblDetalle = new PdfPTable(9);
            tblDetalle.WidthPercentage = 100;


            // Configuramos el título de las columnas de la tabla
            PdfPCell idDpto = new PdfPCell(new Phrase("Id Departamento", _standardFont));
            idDpto.BorderWidth = 1;
            idDpto.BorderWidthBottom = 0.75f;

            PdfPCell fechaLlegada = new PdfPCell(new Phrase("Fecha Llegada", _standardFont));
            fechaLlegada.BorderWidth = 1;
            fechaLlegada.BorderWidthBottom = 0.75f;

            PdfPCell fechaSalida = new PdfPCell(new Phrase("Fecha Salida", _standardFont));
            fechaSalida.BorderWidth = 1;
            fechaSalida.BorderWidthBottom = 0.75f;

            PdfPCell valorReserva = new PdfPCell(new Phrase("Valor Reserva", _standardFont));
            valorReserva.BorderWidth = 1;
            valorReserva.BorderWidthBottom = 0.75f;

            PdfPCell valorRestante = new PdfPCell(new Phrase("Valor Restante", _standardFont));
            valorRestante.BorderWidth = 1;
            valorRestante.BorderWidthBottom = 0.75f;

            PdfPCell valorExtra = new PdfPCell(new Phrase("Valor Extra", _standardFont));
            valorExtra.BorderWidth = 1;
            valorExtra.BorderWidthBottom = 0.75f;

            PdfPCell valorTotal = new PdfPCell(new Phrase("Valor Total", _standardFont));
            valorTotal.BorderWidth = 1;
            valorTotal.BorderWidthBottom = 0.75f;

            PdfPCell multa = new PdfPCell(new Phrase("Multa", _standardFont));
            multa.BorderWidth = 1;
            multa.BorderWidthBottom = 0.75f;

            PdfPCell pago = new PdfPCell(new Phrase("Forma de Pago", _standardFont));
            pago.BorderWidth = 1;
            pago.BorderWidthBottom = 0.75f;



            // Añadimos las celdas a la tabla
            tblDetalle.AddCell(idDpto);
            tblDetalle.AddCell(fechaLlegada);
            tblDetalle.AddCell(fechaSalida);
            tblDetalle.AddCell(valorReserva);
            tblDetalle.AddCell(valorRestante);
            tblDetalle.AddCell(valorExtra);
            tblDetalle.AddCell(valorTotal);
            tblDetalle.AddCell(multa);
            tblDetalle.AddCell(pago);

            // Llenamos la tabla con información
            idDpto = new PdfPCell(new Phrase(det.id_dpto.ToString(), _standardFont));
            idDpto.BorderWidth = 0;

            fechaLlegada = new PdfPCell(new Phrase(det.fecha_llegada.ToShortDateString(), _standardFont));
            fechaLlegada.BorderWidth = 0;

            fechaSalida = new PdfPCell(new Phrase(det.fecha_salida.ToShortDateString(), _standardFont));
            fechaSalida.BorderWidth = 0;

            valorReserva = new PdfPCell(new Phrase(det.valor_reserva.ToString(), _standardFont));
            valorReserva.BorderWidth = 0;

            valorRestante = new PdfPCell(new Phrase(det.valor_restante.ToString(), _standardFont));
            valorRestante.BorderWidth = 0;

            valorExtra = new PdfPCell(new Phrase(det.valor_extra.ToString(), _standardFont));
            valorExtra.BorderWidth = 0;

            valorTotal = new PdfPCell(new Phrase(det.valor_total.ToString(), _standardFont));
            valorTotal.BorderWidth = 0;

            multa = new PdfPCell(new Phrase(det.multa.ToString(), _standardFont));
            multa.BorderWidth = 0;

            pago = new PdfPCell(new Phrase(det.pago.ToString(), _standardFont));
            pago.BorderWidth = 0;



            // Añadimos las celdas a la tabla detalle
            tblDetalle.AddCell(idDpto);
            tblDetalle.AddCell(fechaLlegada);
            tblDetalle.AddCell(fechaSalida);
            tblDetalle.AddCell(valorReserva);
            tblDetalle.AddCell(valorRestante);
            tblDetalle.AddCell(valorExtra);
            tblDetalle.AddCell(valorTotal);
            tblDetalle.AddCell(multa);
            tblDetalle.AddCell(pago);

            doc.Add(tblDetalle);

            if (det.nombre_servicio.Length > 0 && (det.nombre_servicio[0] != "Sin Extra" && det.nombre_servicio[0] != null))
            {
                // Escribimos el encabezamiento de los tipos de servicios extra
                doc.Add(new Paragraph("Detalle Servicios"));
                doc.Add(Chunk.NEWLINE);

                float[] columnWidths = { 10, 10 };
                // Creamos una tabla que contendrá la data
                PdfPTable tblServicios = new PdfPTable(columnWidths);
                tblServicios.WidthPercentage = 100; 

                // Configuramos el título de las columnas de la tabla
                PdfPCell nombreServicio = new PdfPCell(new Phrase("Tipo Servicio", _standardFont));
                nombreServicio.BorderWidth = 1;
                nombreServicio.HorizontalAlignment = 1;
                nombreServicio.BorderWidthBottom = 0.75f;
                

                PdfPCell valorServicio = new PdfPCell(new Phrase("Valor", _standardFont));
                valorServicio.BorderWidth = 1;
                valorServicio.HorizontalAlignment = 1;
                valorServicio.BorderWidthBottom = 0.75f;
                

                tblServicios.AddCell(nombreServicio);
                tblServicios.AddCell(valorServicio);

                for (var i = 0; i<det.nombre_servicio.Length; i++)
                {
                    nombreServicio = new PdfPCell(new Phrase(det.nombre_servicio[i].ToString(), _standardFont));
                    nombreServicio.BorderWidth = 0;

                    valorServicio = new PdfPCell(new Phrase(det.valor_servicio[i].ToString(), _standardFont));
                    valorServicio.BorderWidth = 0;

                    tblServicios.AddCell(nombreServicio);
                    tblServicios.AddCell(valorServicio);
                }

                doc.Add(tblServicios);
            }

            if (det.rut_acompa.Length>0 && (det.rut_acompa[0] != "NOK" && det.rut_acompa[0] != null))
            {
                // Escribimos el encabezamiento de los acompañantes
                doc.Add(new Paragraph("Detalle Acompañantes"));
                doc.Add(Chunk.NEWLINE);

                // Creamos una tabla que contendrá la data
                float[] columnWidths = { 5, 5, 5};
                PdfPTable tblAcompa = new PdfPTable(columnWidths);
                tblAcompa.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell rut = new PdfPCell(new Phrase("Rut Acompañante", _standardFont));
                rut.BorderWidth = 1;
                rut.HorizontalAlignment = 1;
                rut.BorderWidthBottom = 0.75f;
                

                PdfPCell nombre = new PdfPCell(new Phrase("Nombre Acompañante", _standardFont));
                nombre.BorderWidth = 1;
                nombre.HorizontalAlignment = 1;
                nombre.BorderWidthBottom = 0.75f;

                PdfPCell celular = new PdfPCell(new Phrase("Celular Acompañante", _standardFont));
                celular.BorderWidth = 1;
                celular.HorizontalAlignment = 1;
                celular.BorderWidthBottom = 0.75f;

                tblAcompa.AddCell(rut);
                tblAcompa.AddCell(nombre);
                tblAcompa.AddCell(celular);

                for (var i = 0; i < det.rut_acompa.Length; i++)
                {
                    rut = new PdfPCell(new Phrase(det.rut_acompa[i].ToString(), _standardFont));
                    rut.BorderWidth = 0;

                    nombre = new PdfPCell(new Phrase(det.nombre_acompa[i].ToString()+" "+det.apellidos_acompa[i].ToString(), _standardFont));
                    nombre.BorderWidth = 0;

                    celular = new PdfPCell(new Phrase(det.celular_acompa[i].ToString(), _standardFont));
                    celular.BorderWidth = 0;

                    tblAcompa.AddCell(rut);
                    tblAcompa.AddCell(nombre);
                    tblAcompa.AddCell(celular);
                }

                doc.Add(tblAcompa);
            }
            

            // Finalmente, añadimos la tabla al documento PDF y cerramos el documento
            
            

            doc.Close();
            writer.Close();

            

            var resp = System.IO.Path.Combine(pathDoc, "prueba.pdf").ToString();

            
            
            

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelarReserva(int id)
        {
            AvUsuario usuario = (AvUsuario)Session["AVUsuario"];

            var resp = DetalleReservaBl.GetInstance().CancelarReserva(id);

            if (resp.EsPositiva)
            {
                DetalleReservaBl.GetInstance().GenerarCorreoCancelaReserva(usuario);
            }
           
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
    }
}
