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
    public class DepartamentoBl
    {
        private static DepartamentoBl instance;
        public static DepartamentoBl GetInstance()
        {
            if (instance == null)
            {
                instance = new DepartamentoBl();
            }
            return instance;
        }

        public Respuesta<string> RegistrarDepartamento(int dormitorios, int baños, decimal metrosm2, int estacionamiento, string direccion, int id_comuna, int id_estado,
            decimal valor_arriendo, string condiciones, decimal[] id_tipo_inventario, string[]  rutaArchivo )
        {
            try
            {
                DBApi dbApi = new DBApi();


                Departamento depto = new Departamento()
                {
                    dormitorios = dormitorios,
                    baños = baños,
                    metrosm2 = metrosm2,
                    estacionamiento = estacionamiento,
                    direccion = direccion,
                    comuna = id_comuna,
                    id_estado = id_estado,
                    valor_arriendo = valor_arriendo,
                    condiciones = condiciones,
                    tipo_inventario = id_tipo_inventario,
                    ruta_archivo = rutaArchivo
                };

                string json = JsonConvert.SerializeObject(depto);

                dynamic respuesta = dbApi.Post(ConfigurationManager.AppSettings["URLApi"]+"/api/departamento", json);

                var resp = respuesta.ToString();

                List<string> jsonDes = JsonConvert.DeserializeObject<List<string>>(resp);

                if (jsonDes[0] == "DEPARTAMENTO CREADO CON EXITO")
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = true,
                        Elemento = jsonDes[1],
                        Mensaje = jsonDes[0]
                    };
                }
                else
                {
                    return new Respuesta<string>
                    {
                        EsPositiva = false,
                        Elemento = jsonDes[1],
                        Mensaje = jsonDes[0] + "En nuestra base de datos, intente recuperando su contraseña"
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

        public List<Departamento> GetDepartamentos()
        {
            try
            {
                
                DBApi dbApi = new DBApi();

                dynamic dptos = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/departamento");
                

                var resp = dptos.ToString();
                

                if (resp == "")
                {
                    return null;
                }
                else
                {
                    List<Departamento> jsonDes = JsonConvert.DeserializeObject<List<Departamento>>(resp);
                    
                    

                    return jsonDes;
                }

            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new List<Departamento>();
            }
        }

        public List<Departamento> GetFotoDepartamentos(int id)
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic dptos = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/imagenes/"+id);

                var resp = dptos.ToString();

                if (resp == "")
                {
                    return null;
                }
                else
                {
                    List<Departamento> jsonDes = JsonConvert.DeserializeObject<List<Departamento>>(resp);

                    

                    return jsonDes;
                }

            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new List<Departamento>();
            }
        }

        public List<Departamento> GetDepartamentoPorId(int? id)
        {
            try
            {
                List<Departamento> detalleLista = new List<Departamento>();
                var detalle = new Departamento();
                DBApi dbApi = new DBApi();

                dynamic dptos = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/departamento/" + id);
                dynamic dptosImg = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/imagenes/"+id);
                var resp = dptos.ToString();
                var respImg = dptosImg.ToString();

                if (resp == "")
                {
                    return null;
                }
                else
                {
                    List<Departamento> jsonDes = JsonConvert.DeserializeObject<List<Departamento>>(resp);

                    List<ImagenesDpto> jsonDesImg = JsonConvert.DeserializeObject<List<ImagenesDpto>>(respImg);

                    for (var i = 0; i < jsonDes.Count; i++)
                    {
                        detalle.id = jsonDes[i].id;
                        detalle.dormitorios = jsonDes[i].dormitorios;
                        detalle.baños = jsonDes[i].baños;
                        detalle.metrosm2 = jsonDes[i].metrosm2;
                        detalle.estacionamiento = jsonDes[i].estacionamiento;
                        detalle.direccion = jsonDes[i].direccion;
                        detalle.comuna = jsonDes[i].comuna;
                        detalle.nombre_comuna = jsonDes[i].nombre_comuna;
                        detalle.region = jsonDes[i].region;
                        detalle.nombre_region = jsonDes[i].nombre_region;
                        detalle.id_estado = jsonDes[i].id_estado;
                        detalle.nombre_estado = jsonDes[i].nombre_estado;
                        detalle.valor_arriendo = jsonDes[i].valor_arriendo;
                        detalle.condiciones = jsonDes[i].condiciones;
                        detalle.resultado = jsonDes[i].resultado;
                        detalle.tipo_inventario = new decimal[1];
                        detalle.nombre_inventario = new string[1];
                        detalle.precio_inventario = new decimal[1];
                        detalle.ruta_archivo = new string[jsonDesImg.Count];
                        detalle.contenttype = new string[jsonDesImg.Count];
                        detalle.portada = jsonDes[i].portada;
                        detalle.content_portada = jsonDes[i].content_portada;
                        detalle.fecha_creacion = jsonDes[i].fecha_creacion;
                        detalle.orden = new decimal[jsonDesImg.Count];
                        detalle.id_gastos = new decimal[1];
                        detalle.nombre_gastos = new string[1];
                        detalle.valor_gasto = new decimal[1];

                        for (var l = 0; l < jsonDesImg.Count; l++)
                        {
                            if (jsonDesImg[i].ruta_archivo == "" || jsonDesImg[i].ruta_archivo == null)
                            {
                                l = jsonDesImg.Count;
                            }
                            else
                            {

                                detalle.ruta_archivo[l] = jsonDesImg[l].ruta_archivo;
                                detalle.contenttype[l] = jsonDesImg[l].contenttype;
                                detalle.orden[l] = jsonDesImg[l].orden;

                            }
                        }
                        detalleLista.Add(detalle);

                    }

                        return detalleLista;
                }

            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new List<Departamento>();
            }
        }

        public List<Servicios> GetServicios()
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic serv = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/servicio");

                var resp = serv.ToString();

                //ajuste para manejar atributos nulos en json
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                if (resp == "")
                {
                    return null;
                }
                else
                {
                    List<Servicios> jsonDes = JsonConvert.DeserializeObject<List<Servicios>>(resp, settings);

                    
                    return jsonDes;
                }

            }
            catch (Exception e)
            {
                Log.Business().Error(e.Message, e);
                return new List<Servicios>();
            }
        }

    }
}
