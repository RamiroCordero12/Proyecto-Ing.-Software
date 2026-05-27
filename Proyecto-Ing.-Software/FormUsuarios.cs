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
    public partial class FormUsuarios : Form
    {
        private int dniUsuario = 0;
        public FormUsuarios()
        {
            InitializeComponent();
            ActualizarGrilla();
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = new Usuario();

                usuario.DNI = int.Parse(txtDNI.Text);
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Email = txtEmail.Text;
                usuario.Rol = cmbRoles.SelectedIndex + 1;
                usuario.Estado = true;

                UsuarioBLL usuarioBLL = new UsuarioBLL();

                int adminLogueado = SessionManager.GetInstance.usuario.DNI;

                bool exito = usuarioBLL.CrearUsuario(usuario, adminLogueado);

                if (exito)
                {
                    MessageBox.Show("Usuario creado!");
                    ActualizarGrilla();
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                string amigable;
                switch (sqlEx.Number)
                {
                    case 2627: // Violation of primary key or unique index
                    case 2601: // Cannot insert duplicate key row
                        amigable = "Ya existe un usuario con ese DNI).";
                        break;

                    default:
                        amigable = "Error de base de datos. Contacte al administrador.";
                        break;
                }
                MessageBox.Show(amigable, "Error al crear usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // fallback for other exceptions
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarGrilla()
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();

                dgvUsuario.DataSource = null;
                dgvUsuario.DataSource = usuarioBLL.ListarUsuarios();
                if (dgvUsuario.Columns.Contains("Contrasena"))
                    dgvUsuario.Columns["Contrasena"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la grilla: " + ex.Message);
            }
        }

        private void btnDeshabilitarUsuario_Click(object sender, EventArgs e)
        {
            int dniSeleccionado = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            bool exito = usuarioBLL.DeshabilitarUsuario(dniSeleccionado);

            if (exito)
            {
                MessageBox.Show("Usuario deshabilitado");
            }
            else
            {
                MessageBox.Show("Error para deshabilitar usuario");
            }
            ActualizarGrilla();

        }

        private void dgvUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvUsuario.Rows[e.RowIndex];

                dniUsuario = Convert.ToInt32(fila.Cells["DNI"].Value);
                txtDNI.Text = fila.Cells["DNI"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
                txtEmail.Text = fila.Cells["Email"].Value.ToString();

            }
        }

        private void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            if (dniUsuario == 0)
            {
                MessageBox.Show("Selecciona un usuario para modificar");
                return;
            }

            Usuario usuario = new Usuario();
            usuario.DNI = dniUsuario;
            usuario.DNI = int.Parse(txtDNI.Text);
            usuario.Nombre = txtNombre.Text;
            usuario.Apellido = txtApellido.Text;
            usuario.Email = txtEmail.Text;
            usuario.Rol = cmbRoles.SelectedIndex + 1;

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            bool exito = usuarioBLL.ModificarUsuario(usuario, dniUsuario);

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

        private void btnHabilitarUsuario_Click(object sender, EventArgs e)
        {
            int dniSeleccionado = Convert.ToInt32(dgvUsuario.SelectedRows[0].Cells["DNI"].Value);

            UsuarioBLL usuarioBLL = new UsuarioBLL();
            bool exito = usuarioBLL.HabilitarUsuario(dniSeleccionado);

            if (exito)
            {
                MessageBox.Show("Usuario Habilitado");
                ActualizarGrilla();

            }
            else
            {
                MessageBox.Show("Error para habilitar usuario");
            }

        }

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {

        }
    }
}
