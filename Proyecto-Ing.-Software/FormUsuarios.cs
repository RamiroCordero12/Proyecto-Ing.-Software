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
        private int IdUsuarioSeleccionado = 0;
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

        private void dgvUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvUsuario.Rows[e.RowIndex];

                IdUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
                txtNombreUsuario.Text = fila.Cells["NombreUsuario"].Value.ToString();
                txtContrasena.Text = fila.Cells["Contrasena"].Value.ToString();
            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            if(IdUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un usuario para modificar");
                return;
            }

            UsuarioBE usuario = new UsuarioBE();
            usuario.IdUsuario = IdUsuarioSeleccionado;
            usuario.NombreUsuario = txtNombreUsuario.Text;
            usuario.Contrasena = txtContrasena.Text;

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            bool exito = usuarioBLL.ModificarUsuario(usuario);

            if (exito)
            {
                MessageBox.Show("Usuario modificado");
                ActualizarGrilla();
            }
            else
            {
                MessageBox.Show("Error al modificar un usuario");
                return;
            }

        }
    }
}
