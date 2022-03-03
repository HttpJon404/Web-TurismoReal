using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Contexto;
using EntidadServicio;
using Newtonsoft.Json;
using Utilidad;
namespace Negocio
{
    public class DetalleReservaBl
    {
        private static DetalleReservaBl instance;
        public static DetalleReservaBl GetInstance()
        {
            if (instance == null)
            {
                instance = new DetalleReservaBl();
            }
            return instance;
        }

        public List<DetalleReserva> DetalleReserva(int id_usuario)
        {
            try
            {
                List<DetalleReserva> detalleLista = new List<DetalleReserva>();
                var detalle = new DetalleReserva();

                DBApi dbApi = new DBApi();


                


                dynamic detalleReserva = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/Reserva/" + id_usuario);
                dynamic detalleServicio = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/DetalleServicio/" + id_usuario);
                dynamic detalleAcompa = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/DetalleAcompa/" + id_usuario);

                var respDet = detalleReserva.ToString();
                var respServ = detalleServicio.ToString();
                var respAcom = detalleAcompa.ToString();

                if (respDet == "")
                {
                    return null;
                }
                else
                {
                    //ajuste para manejar atributos nulos en json
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    List<DetalleReserva> jsonDesDet = JsonConvert.DeserializeObject<List<DetalleReserva>>(respDet, settings);
                    List<DetalleServicio> jsonDesServ = JsonConvert.DeserializeObject<List<DetalleServicio>>(respServ, settings);
                    List<DetalleAcompanante> jsonDesAcom = JsonConvert.DeserializeObject<List<DetalleAcompanante>>(respAcom, settings);

                    for (var i = 0; i < jsonDesDet.Count; i++)
                    {
                        detalle.id = jsonDesDet[i].id;
                        detalle.id_dpto = jsonDesDet[i].id_dpto;
                        detalle.portada = jsonDesDet[i].portada;
                        detalle.content_portada = jsonDesDet[i].content_portada;
                        detalle.fecha_llegada = jsonDesDet[i].fecha_llegada;
                        detalle.fecha_salida = jsonDesDet[i].fecha_salida;
                        detalle.valor_reserva = jsonDesDet[i].valor_reserva;
                        detalle.valor_restante = jsonDesDet[i].valor_restante;
                        detalle.valor_extra = jsonDesDet[i].valor_extra;
                        detalle.valor_total = jsonDesDet[i].valor_total;
                        detalle.id_estado = jsonDesDet[i].id_estado;
                        detalle.multa = jsonDesDet[i].multa;
                        detalle.pago = jsonDesDet[i].pago;
                        detalle.nombre_servicio = new string[jsonDesServ.Count];
                        detalle.valor_servicio = new decimal[jsonDesServ.Count];
                        detalle.rut_acompa = new string[jsonDesAcom.Count];
                        detalle.nombre_acompa = new string[jsonDesAcom.Count];
                        detalle.apellidos_acompa = new string [jsonDesAcom.Count];
                        detalle.celular_acompa = new string [jsonDesAcom.Count];

                        for (var l = 0; l < jsonDesServ.Count; l++)
                        {
                            if (jsonDesServ[l].nombre_servicio == "Sin Extra")
                            {
                                l = jsonDesServ.Count;
                            }
                            else
                            {
                                
                                detalle.nombre_servicio[l]= jsonDesServ[l].nombre_servicio;
                                detalle.valor_servicio[l] = jsonDesServ[l].valor_servicio;

                            }
                        }

                        for (var x = 0; x < jsonDesAcom.Count; x++)
                        {
                            if (jsonDesAcom[x].rut == "NOK")
                            {
                                x = jsonDesAcom.Count;
                            }
                            else
                            {
                                detalle.rut_acompa[x] = jsonDesAcom[x].rut;
                                detalle.nombre_acompa[x] = jsonDesAcom[x].nombres;
                                detalle.apellidos_acompa[x] = jsonDesAcom[x].apellidos;
                                detalle.celular_acompa[x] = jsonDesAcom[x].celular;

                            }
                        }


                        detalleLista.Add(detalle);
                        detalle = new DetalleReserva();
                        
                    }

                    
                    
                    

                    return detalleLista;
                }
                }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return null;
            }
        }

        public Respuesta<string> CancelarReserva(int id)
        {
            try
            {
                DBApi dbApi = new DBApi();

                CancelarReserva det = new CancelarReserva()
                {
                    id = id
                };

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(det, settings);

                dynamic res = dbApi.Put(ConfigurationManager.AppSettings["URLApi"] + "/api/Reserva",json);

                var resp = res.ToString();

                

                if (resp == "RESERVA CANCELADA")
                {
                    return new Respuesta<string>
                    {
                        Elemento = null,
                        EsPositiva = true,
                        Mensaje = "Reserva cancelada con exito"

                    };
                    
                }
                else
                {
                    return new Respuesta<string> 
                    {   Elemento = null, 
                        EsPositiva = false, 
                        Mensaje = "Ocurrio un error al cancelar la reserva, re-intente nuevamente" 
                    };
                }

            }
            catch (Exception e)
            {
                return new Respuesta<string>
                {
                    Elemento = null,
                    EsPositiva = false,
                    Mensaje = "Ocurrio un error inesperado"

                };
            }
        }

        public Respuesta<string> GenerarCorreoCancelaReserva(AvUsuario usuario)
        {
            try
            {

                if (usuario != null)
                {
                    String urlSistema = ConfigurationManager.AppSettings["URLSistema"];
                    //TODO: Enviar correo
                    string html;
                    string textBody = "";
                    var asunto = "";
                    string ruta_plantilla_correo = "";


                    asunto = "Turismo Real : Reserva cancelada con exito";

                    ruta_plantilla_correo =
                        HttpContext.Current.Server.MapPath(
                            "~/Content/Plantillas/planilla_correo_cancela_reserva.html");

                    html = File.ReadAllText(ruta_plantilla_correo);

                    textBody = "";
                        //"Bienvenid@ a TurismoReal:\n\n{0}\n\n" +
                        //"Se ha agendado tu reserva correctamente." +
                        //"\n\n----------------------------------\n\n" +
                        //"Este mensaje ha sido generado automáticamente, favor no responder.\nTurismo Real";


                    html =
                        html.Replace("{0}", usuario.NOMBRES)
                            .Replace("{1}", "Bienvenid@")
                            .Replace("{2}", urlSistema)
                            .Replace("{5}", ConfigurationManager.AppSettings["MailHeader"])
                            .Replace("{6}", ConfigurationManager.AppSettings["MailFooter"]);

                    textBody = textBody.Replace("{0}", usuario.NOMBRES);

                    Email.SendHtmlEmailJet(asunto, usuario.CORREO, "", "", html, null, null, null, textBody);

                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = null,
                        Mensaje = "Se envio un correo con la confirmación de su cancelación"
                    };

                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = "Ha ocurrido un error con la cancelación de su reserva"
                    };
                }


            }
            catch (Exception e)
            {
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error con la cancelación de su reserva"
                };
            }
        }
    }
}
