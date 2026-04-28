using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;

namespace Proyecto_Ing._Software
{
    public partial class FormUsuarios : Form
    {
        public FormUsuarios()
        {
            InitializeComponent();
            ActualizarGrilla();
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBE usuario = new UsuarioBE();
                usuario.NombreUsuario = txtNombreUsuario.Text;
                usuario.Contrasena = txtContrasena.Text;
                usuario.Estado = true;

                UsuarioBLL usuarioBLL = new UsuarioBLL();
                bool confirmacion = usuarioBLL.CrearUsuario(usuario);

                if (confirmacion)
                {
                    MessageBox.Show("Usuario creado!");

                    txtNombreUsuario.Clear();
                    txtContrasena.Clear();

                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void ActualizarGrilla()
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();

                dgvUsuario.DataSource = null;
                dgvUsuario.DataSource = usuarioBLL.ListarUsuarios();

            }
            catch
            {
                MessageBox.Show("Error al cargar el la grilla");
            }
        }

        private void btnDeshabilitarUsuario_Click(object sender, EventArgs e)
        {
            int idSeleccionado = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["IdUsuario"].Value);

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            bool exito = usuarioBLL.DeshabilitarUsuario(idSeleccionado);

            ActualizarGrilla();

            if (exito)
            {
                MessageBox.Show("Usuario deshabilitado");
            }
            else
            {
                MessageBox.Show("Error para deshabilitar usuario");
            }
        }
    }
}
