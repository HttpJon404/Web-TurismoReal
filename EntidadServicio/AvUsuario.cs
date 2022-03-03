using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadServicio
{
    public class AvUsuario
    {
        public decimal ID { get; set; }
        public decimal ID_COMUNA { get; set; }
        public decimal ID_REGION { get; set; }
        public String RUT { get; set; }
        public String NOMBRES { get; set; }
        public String APELLIDOS { get; set; }
        public String DIRECCION { get; set; }
        public String CORREO { get; set; }
        public String CELULAR { get; set; }
        public decimal EDAD { get; set; }
        public String GENERO { get; set; }
        public List<String> Paginas { get; set; }
        public List<Menu> Menu { get; set; }
        public List<AvRol> Roles { get; set; }

        public bool TieneRol(int id)
        {
            if (this.Roles != null && this.Roles.Count() > 0)
            {
                foreach (AvRol rol in this.Roles)
                {
                    if (rol.Id == id) return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

    }
}
