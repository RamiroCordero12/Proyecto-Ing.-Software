using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Servicios;

namespace Proyecto_Ing._Software
{
    public partial class FormMenuPrincipal : Form
    {
        public FormMenuPrincipal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUsuarios formUsuarios = new FormUsuarios();
            formUsuarios.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Usuario usuarioLogueado = SessionManager.GetInstance.usuario;

            if (usuarioLogueado.Rol == 1)
            {
                usuarioToolStripMenuItem.Visible = true;
                bitacoraToolStripMenuItem.Visible = true;
            }
            else if(usuarioLogueado.Rol == 2)
            {
                usuarioToolStripMenuItem.Visible = false;
                bitacoraToolStripMenuItem.Visible = false;
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult confirmar = MessageBox.Show("Estas seguro de cerrar sesion?", "Cerrar sesion", MessageBoxButtons.YesNo);

            if(confirmar == DialogResult.Yes)
            {
                SessionManager.GetInstance.Logout();

                FormLogin formlogin = new FormLogin();
                formlogin.Show();

                this.Close();
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void bitacoraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormBitacora formBitacora = new FormBitacora();
            formBitacora.Show();
        }

        private void cambiarContrasenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCambiarContrasena formCambiar = new FormCambiarContrasena();
            formCambiar.Show();
        }
    }
}
