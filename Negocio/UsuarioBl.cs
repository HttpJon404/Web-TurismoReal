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
    public class UsuarioBl
    {
        private static UsuarioBl instance;
        public static UsuarioBl GetInstance()
        {
            if (instance == null)
            {
                instance = new UsuarioBl();
            }
            return instance;
        }

        public AvUsuario CompletaSessionUsuario(int id)
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic user = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/usuarios" + "/"+id);

                var resp = user.ToString();

                if(resp == "")
                {
                    return null;
                }
                else
                {
                    List<UsuarioGet> jsonDes = JsonConvert.DeserializeObject<List<UsuarioGet>>(resp);


                    var userSession = new AvUsuario();
                    userSession.ID = decimal.ToInt32(jsonDes[0].id);
                    userSession.ID_COMUNA = decimal.ToInt32(jsonDes[0].id_comuna);
                    userSession.ID_REGION = decimal.ToInt32(jsonDes[0].id_region);
                    userSession.RUT = jsonDes[0].rut;
                    userSession.NOMBRES = jsonDes[0].nombres;
                    userSession.APELLIDOS = jsonDes[0].apellidos;
                    userSession.DIRECCION = jsonDes[0].direccion;
                    userSession.CORREO = jsonDes[0].correo;
                    userSession.CELULAR = jsonDes[0].celular;
                    userSession.EDAD = decimal.ToInt32(jsonDes[0].edad);
                    userSession.GENERO = jsonDes[0].genero;

                    //userSession.Roles = new List<AvRol>();

                    List<AvRol> avrol = new List<AvRol>();
                    var rol = new AvRol();
                    for (int i = 0; i < jsonDes.Count; i++)
                    {
                        rol.Id = jsonDes[i].id_rol;
                        rol.Descripcion = jsonDes[i].descripcion_rol;
                        avrol.Add(rol);
                        rol = new AvRol();
                    }
                    userSession.Roles = avrol;

                    //userSession.Roles.Add(avrol);

                    return userSession;
                }

                
                
            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return null;
            }
        }

        public Respuesta<string> RegistrarUsuario(string nombre, string apellidos, decimal edad,string rut, string idGenero, decimal idComuna, decimal idRegion,
                     string direccion, string correo, string celular, string contrasena, decimal idRol)
        {
            try
            {
                DBApi dbApi = new DBApi();

                if (idGenero == "1")
                {
                    idGenero = "M";
                }
                else
                {
                    idGenero = "F";
                }
                contrasena = CripSha1.Encriptar(contrasena);
                Usuario.Item usuario = new Usuario.Item
                {
                    id_comuna = idComuna,
                    id_region = idRegion,
                    rut = rut,
                    nombres = nombre,
                    apellidos = apellidos,
                    edad = edad,
                    direccion = direccion,
                    correo = correo,
                    celular = celular,
                    genero = idGenero,
                    contrasena = contrasena,
                    estado = "1",
                    id_rol = idRol
                };

                string json = JsonConvert.SerializeObject(usuario);

                dynamic respuesta = dbApi.Post(ConfigurationManager.AppSettings["URLApi"] +"/api/usuarios",json);

                var resp = respuesta.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes[0] == "DATOS INGRESADOS CORRECTAMENTE")
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = jsonDes[1],
                        Mensaje = "Usuario registrado correctamente."
                    };
                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = jsonDes[1],
                        Mensaje = jsonDes[0]+"En nuestra base de datos, intente recuperando su contraseña"
                    };
                }

                
            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error al registrarse, por favor reintentelo nuevamente."
                };
            }
        }

        public UsuarioGet GetUsuarioEmail(string email)
        {
            try
            {
                var usuario = new UsuarioGet();

                DBApi dbApi = new DBApi();

               

                dynamic user = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/Login?correo=" + email);

                var resp = user.ToString();

                if (resp == "")
                {
                    return null;
                }
                else
                {
                    List<UsuarioGet> jsonDes = JsonConvert.DeserializeObject<List<UsuarioGet>>(resp);


                    usuario.id = jsonDes[0].id;
                    usuario.id_comuna = jsonDes[0].id_comuna;
                    usuario.id_region = jsonDes[0].id_region;
                    usuario.rut = jsonDes[0].rut;
                    usuario.nombres = jsonDes[0].nombres;
                    usuario.apellidos = jsonDes[0].apellidos;
                    usuario.direccion = jsonDes[0].direccion;
                    usuario.correo = jsonDes[0].correo;
                    usuario.celular = jsonDes[0].celular;
                    usuario.edad = decimal.ToInt32(jsonDes[0].edad);
                    usuario.genero = jsonDes[0].genero;
                    usuario.id_rol = jsonDes[0].id_rol;


                    return usuario;
                }



            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return null;
            }


        }

        public Respuesta<string> LoginUsuario(string correo, string contrasena)
        {
            try
            {
                DBApi dbApi = new DBApi();

                contrasena = CripSha1.Encriptar(contrasena);

                LoginUsuario login = new LoginUsuario()
                {
                    correo = correo,
                    contrasena = contrasena
                };

                string json = JsonConvert.SerializeObject(login);

                dynamic respuesta = dbApi.Post(ConfigurationManager.AppSettings["URLApi"] + "/api/login", json);

                var resp = respuesta.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes[0] == "OK")
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = jsonDes[1],
                        Mensaje = "Inicio de sesión correctamente."
                    };
                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = jsonDes[0]
                    };
                }

                



            }
            catch (Exception e )
            {
                Log.Business().Error(e.Message, e);
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error al ingresar, por favor reintentelo nuevamente."
                };
            }
        }

        public Respuesta<Usuario.Item> LogoutUsuario()
        {
            try
            {
                return new Respuesta<Usuario.Item>
                {
                    EsPositiva = true,
                    Elemento = null,
                    Mensaje = ""
                };
            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new Respuesta<Usuario.Item>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error al cerrar sesión, por favor inténtelo más tarde."
                };
            }
        }

        public bool GenerarLinkContrasena(int id, int typeId)
        {
            try
            {
                var clink = UsuarioBl.GetInstance().GenerarLinkContrasena1(id, typeId);
                if (clink != null)
                {
                    //cambiar url de sistema a la de maquina local en webconfig
                    String urlSistema = ConfigurationManager.AppSettings["URLSistema"];
                    string link = urlSistema + "Home/Restablecer/" + clink[0];
                    //TODO: Enviar correo
                    string html;
                    string textBody = "";
                    var asunto = "";
                    string ruta_plantilla_correo = "";

                        asunto = "Turismo Real : Restablecer Contraseña";

                        ruta_plantilla_correo =
                            HttpContext.Current.Server.MapPath(
                                "~/Content/Plantillas/planilla_recuperacion_contrasena.html");

                        html = File.ReadAllText(ruta_plantilla_correo);

                        textBody =
                            "Recuperación de Contraseña:" +
                            "\n\nEstimad@ {0}\n\nSe ha solicitado la recuperación de su contraseña para el sistema Turismo Real." +
                            "\nPara establecer su contraseña, copie la siguiente URL y péguela en el navegador web de su elección." +
                            "\n\n{1}\n\nRecuerde que, por su seguridad, el vínculo anterior tiene una validez de 30 minutos." +
                            "\n\n----------------------------------\n\n" +
                            "Este mensaje ha sido generado automáticamente, favor no responder.\nTurismo Real";
                    

                    html =
                        html.Replace("{0}", clink[1])
                            .Replace("{1}", link)
                            .Replace("{2}", urlSistema)
                            .Replace("{5}", ConfigurationManager.AppSettings["MailHeader"])
                            .Replace("{6}", ConfigurationManager.AppSettings["MailFooter"]);

                    textBody = textBody.Replace("{0}", clink[1]).Replace("{1}", link);

                    Email.SendHtmlEmailJet(asunto, clink[2], "", "", html, null, null, null, textBody);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GenerarLinkContrasena1(int id_usuario, int typeId)
        {
            try
            {

                DBApi dbApi = new DBApi();

                ContrasenaLink con = new ContrasenaLink()
                {
                    ClnkId = 0,
                    ClnkLink = "",
                    UsuId = id_usuario,
                    TclnId = typeId,
                    ClnkEstado = false,
                    ClnkFechaCreacion = DateTime.Now,
                    ClnkFechaSeteoPass = null,
                    UsuPassword = null


                };

                string json = JsonConvert.SerializeObject(con);


                dynamic user = dbApi.Post(ConfigurationManager.AppSettings["URLApi"] + "/api/Contrasena", json);

              
                var resp = user.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes.Count >0 )
                {
                    return jsonDes;
                }
                else
                {
                    return new List<string>();
                }


            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new List<string>();
            }
        }

        public string GetLinkContrasena(string id)
        {
            try
            {
                var link = string.Empty;

                DBApi dbApi = new DBApi();



                dynamic resp = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/Contrasena?link=" + id);

                link = resp.ToString();

                if (link != "Vacio")
                {
                    return link;
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception e)
            {

                Log.Business().Error(e.Message, e);
                return null;
            }
        }

        public bool CambiarContrasena(string idLink, string newPass)
        {
            try
            {

                DBApi dbApi = new DBApi();


                ContrasenaLink clink = new ContrasenaLink() { 
                   ClnkLink = idLink,
                   UsuPassword = newPass
                
                };

                string json = JsonConvert.SerializeObject(clink);


                dynamic user = dbApi.Put(ConfigurationManager.AppSettings["URLApi"] + "/api/Contrasena", json);

                var resp = user.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes.Count > 0)
                {

                    RecuperacionExito(jsonDes[1]);
                    return true;
                }
                else
                {
                    return false;
                }
                
                
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RecuperacionExito(string correo)
        {
            try
            {
                var usuario = UsuarioBl.GetInstance().GetUsuarioEmail(correo);
                if (usuario != null)
                {
                    String urlSistema = ConfigurationManager.AppSettings["URLSistema"];
                    //TODO: Enviar correo
                    string html;
                    string textBody = "";
                    var asunto = "";
                    string ruta_plantilla_correo = "";


                    asunto = "Turismo Real : Recuperación con éxito";

                    ruta_plantilla_correo =
                        HttpContext.Current.Server.MapPath(
                            "~/Content/Plantillas/planilla_recuperacion_exito.html");

                    html = File.ReadAllText(ruta_plantilla_correo);

                    textBody =
                        "Bienvenid@ a TurismoReal:\n\n{0}\n\n" +
                        "Se ha reestablecido su contraseña con éxito." +
                        "\n\n----------------------------------\n\n" +
                        "Este mensaje ha sido generado automáticamente, favor no responder.\nTurismo Real";


                    html =
                        html.Replace("{0}", usuario.nombres)
                            .Replace("{1}", "Bienvenid@")
                            .Replace("{2}", urlSistema)
                            .Replace("{5}", ConfigurationManager.AppSettings["MailHeader"])
                            .Replace("{6}", ConfigurationManager.AppSettings["MailFooter"]);

                    textBody = textBody.Replace("{0}", usuario.nombres);

                    Email.SendHtmlEmailJet(asunto, usuario.correo, "", "", html, null, null, null, textBody);

                }


            }
            catch (Exception e)
            {

            }
        }

        public Respuesta<string> GenerarCorreoBienvenida(AvUsuario usuario)
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


                    asunto = "Turismo Real : Creación de cuenta exitosa";

                    ruta_plantilla_correo =
                        HttpContext.Current.Server.MapPath(
                            "~/Content/Plantillas/planilla_correo_bienvenida.html");

                    html = File.ReadAllText(ruta_plantilla_correo);

                    textBody =
                        "Bienvenid@ a TurismoReal:\n\n{0}\n\n" +
                        "Se ha creado su cuenta con éxito." +
                        "\n\n----------------------------------\n\n" +
                        "Este mensaje ha sido generado automáticamente, favor no responder.\nTurismo Real";


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
                        Mensaje = "Se envio un correo con la confirmación de su registro"
                    };

                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = "Ha ocurrido un error con la creación de su cuenta"
                    };
                }


            }
            catch (Exception e)
            {
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error con la creación de su cuenta"
                };
            }
        }

        public Respuesta<string> PagoConfirmado(int id, int idUsuario,DateTime fecha_llegada, DateTime fecha_salida, int valorReserva,
            int valorRestante, int valorTotalDias, int valorDiario, string listaAcompanantes, string listaServicios,string pago)
        {
            try
            {
                DBApi dbApi = new DBApi();
                


                Reserva res = new Reserva()
                {
                    id_dpto = id,
                    id_usuario = idUsuario,
                    listaAcompanante = listaAcompanantes,
                    listaServicios = listaServicios,
                    fecha_llegada = fecha_llegada,
                    fecha_salida = fecha_salida,
                    valorReserva = valorReserva,
                    valorRestante = valorRestante,
                    valorTotalDias = valorTotalDias,
                    valorDiario = valorDiario,
                    pago = pago
                };

                string json = JsonConvert.SerializeObject(res);

                dynamic respuesta = dbApi.Post(ConfigurationManager.AppSettings["URLApi"] + "/api/Reserva", json);

                var resp = respuesta.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes[0] == "SU RESERVA HA SIDO CREADA EXITOSAMENTE")
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = jsonDes[1],
                        Mensaje = "Reserva Creada Exitosamente"
                    };
                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = jsonDes[0]
                    };
                }





            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error al reservar, por favor re-intentelo nuevamente."
                };
            }
        }

        public Respuesta<string> GenerarCorreoReserva(AvUsuario usuario)
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


                    asunto = "Turismo Real : Reserva agendada Exitosamente";

                    ruta_plantilla_correo =
                        HttpContext.Current.Server.MapPath(
                            "~/Content/Plantillas/planilla_correo_reserva.html");

                    html = File.ReadAllText(ruta_plantilla_correo);

                    textBody =
                        "Bienvenid@ a TurismoReal:\n\n{0}\n\n" +
                        "Se ha agendado tu reserva correctamente." +
                        "\n\n----------------------------------\n\n" +
                        "Este mensaje ha sido generado automáticamente, favor no responder.\nTurismo Real";


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
                        Mensaje = "Se envio un correo con la confirmación de su registro"
                    };

                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = "Ha ocurrido un error con la creación de su reserva"
                    };
                }


            }
            catch (Exception e)
            {
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error con la creación de su reserva"
                };
            }
        }


        public Respuesta<string> PagoRestante(int id)
        {
            try
            {
                DBApi dbApi = new DBApi();

                
                //usare la clase cancelar solo para serializar el ID y no crear otra clase más
                CancelarReserva res = new CancelarReserva()
                {
                    id = id
                };

                string json = JsonConvert.SerializeObject(res);

                dynamic respuesta = dbApi.Post(ConfigurationManager.AppSettings["URLApi"] + "/api/pago", json);

                var resp = respuesta.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes[0] == "PAGO REALIZADO CON EXITO")
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = null,
                        Mensaje = "PAGO REALIZADO CON EXITO"
                    };
                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = "HUBO UN ERROR AL REALIZAR EL PAGO"
                    };
                }





            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error al pagar, por favor re-intentelo nuevamente."
                };
            }
        }


        public Respuesta<string> GenerarCorreoPagoRestante(AvUsuario usuario)
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


                    asunto = "Turismo Real : Pago Restante Realizado con éxito";

                    ruta_plantilla_correo =
                        HttpContext.Current.Server.MapPath(
                            "~/Content/Plantillas/planilla_correo_pago.html");

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
                        Mensaje = "Se envio un correo con la confirmación de su pago"
                    };

                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = null,
                        Mensaje = "Ha ocurrido un error con el pago"
                    };
                }


            }
            catch (Exception e)
            {
                return new Respuesta<string>
                {
                    EsPositiva = false,
                    Elemento = null,
                    Mensaje = "Ha ocurrido un error con el pago"
                };
            }
        }

    }
}
