using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilidad
{
    public class Log
    {
        private static readonly ILog logDAO = LogManager.GetLogger("DAOLogger");
        private static readonly ILog logBL = LogManager.GetLogger("BusinessLogger");
        private static readonly ILog logWeb = LogManager.GetLogger("WebLogger");

        public static ILog DAO()
        {
            return logDAO;
        }

        public static ILog Business()
        {
            return logBL;
        }

        public static ILog Web()
        {
            return logWeb;
        }
    }
}
