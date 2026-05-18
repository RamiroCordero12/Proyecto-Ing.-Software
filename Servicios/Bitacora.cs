using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public int DNI { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Modulo { get; set; }
        public string Criticidad { get; set; }


    }
}
