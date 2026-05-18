using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;
using DAL;

namespace BLL
{
    public class BitacoraBLL
    {
        public List<Bitacora> ListarBitacora()
        {
            BitacoraDAL bitacora = new BitacoraDAL();
            return bitacora.ListarBitacora();
        }
    }
}
