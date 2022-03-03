using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadServicio
{
    public class Respuesta<T>
    {
        public bool EsPositiva { get; set; }
        public T Elemento { get; set; }
        public String Mensaje { get; set; }
    }
}
