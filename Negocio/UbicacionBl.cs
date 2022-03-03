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
    public class UbicacionBl
    {
        private static UbicacionBl instance;
        public static UbicacionBl GetInstance()
        {
            if (instance == null)
            {
                instance = new UbicacionBl();
            }
            return instance;
        }

        public List<GetComunas> GetComunas()
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic result = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/comuna");

                var res = result.ToString();


                if (res != "")
                {
                    List<GetComunas> json = JsonConvert.DeserializeObject<List<GetComunas>>(res);

                    return json;
                }
                else
                {
                    return new List<GetComunas>();
                }
                
            }
            catch (Exception)
            {

                return new List<GetComunas>();
            }
        }

        public List<GetComunas> GetRegion()
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic result = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/region");

                var res = result.ToString();


                if (res != "")
                {
                    List<GetComunas> json = JsonConvert.DeserializeObject<List<GetComunas>>(res);

                    return json;
                }
                else
                {
                    return new List<GetComunas>();
                }

            }
            catch (Exception)
            {

                return new List<GetComunas>();
            }
        }

        public List<GetComunas> GetRegionPorComuna(int id)
        {
            try
            {
                DBApi dbApi = new DBApi();

                dynamic result = dbApi.Get(ConfigurationManager.AppSettings["URLApi"] + "/api/region/"+id);

                var res = result.ToString();


                if (res != "")
                {
                    List<GetComunas> json = JsonConvert.DeserializeObject<List<GetComunas>>(res);

                    return json;
                }
                else
                {
                    return new List<GetComunas>();
                }

            }
            catch (Exception)
            {

                return new List<GetComunas>();
            }
        }
    }
}
