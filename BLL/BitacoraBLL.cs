using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
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

        // Wipes the audit log, then immediately writes a single fresh entry
        // recording who did it — otherwise the act of clearing the trail
        // would itself be untraceable.
        public bool LimpiarBitacora(int dniAdmin)
        {
            BitacoraDAL dal = new BitacoraDAL();
            bool exito = dal.LimpiarBitacora();

            if (exito)
            {
                dal.RegistroBitacora(0, dniAdmin, "Bitacora limpiada por el administrador",
                    DateTime.Now, "Bitacora", "Alta");
            }

            return exito;
        }
    }
}
