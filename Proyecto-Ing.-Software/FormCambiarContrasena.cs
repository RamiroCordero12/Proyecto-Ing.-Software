using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Servicios;

namespace Proyecto_Ing._Software
{
    public partial class FormCambiarContrasena : Form
    {
        public FormCambiarContrasena()
        {
            InitializeComponent();
        }

        private void btnCambiarContrasena_Click(object sender, EventArgs e)
        {
            try
            {
                int dni = SessionManager.GetInstance.usuario.DNI; // or obtain selected user's DNI
                string actual = txtContrasenaActual.Text;
                string nueva = txtContrasenaNueva.Text;
                string confirmar = txtContrasenaConfirmar.Text;

                UsuarioBLL usuarioBLL = new UsuarioBLL();
                bool ok = usuarioBLL.CambiarContrasena(dni, actual, nueva, confirmar);

                if (ok)
                {
                    MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Clear fields
                    txtContrasenaActual.Clear();
                    txtContrasenaNueva.Clear();
                    txtContrasenaConfirmar.Clear();
                }
            }
            catch (ArgumentException aex)
            {
                MessageBox.Show(aex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException uex)
            {
                MessageBox.Show(uex.Message, "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar la contraseña: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
