using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilidad
{
    public class CripSha1
    {
        public static string Encriptar(string cadena)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(cadena));
            StringBuilder resultado = new StringBuilder();
            foreach (byte b in hash)
            {
                resultado.Append(b.ToString("X2"));
            }
            return resultado.ToString();
        }

        public static bool Validar(string cadena, string cadenaCifrada)
        {
            String cadenaCifradaAComparar = Encriptar(cadena);
            return cadenaCifrada.Equals(cadenaCifradaAComparar);
        }
    }
}
