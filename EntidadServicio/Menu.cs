using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadServicio
{
    public class Menu
    {
        public int Id { get; set; }
        public String Nombre { get; set; }
        public String Action { get; set; }
        public String Controller { get; set; }
        public int Orden { get; set; }
        public List<Menu> Submenu { get; set; }
    }
}
