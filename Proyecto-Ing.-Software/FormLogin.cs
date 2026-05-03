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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private UsuarioBLL usuarioBLL = new UsuarioBLL();

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuarioLogueado = usuarioBLL.Login(txtNombreUsuario.Text, txtContrasena.Text);

                MessageBox.Show($"Bienvenido {txtNombreUsuario.Text} al sistema!!");

                Form1 form1 = new Form1();
                form1.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al iniciar sesion");
            }
        }
    }
}
