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
    public partial class FormRoles : Form
    {
        private int _idRolSeleccionado = 0;
        private List<PatenteComponent> _todasLasPatentes = new List<PatenteComponent>();
        private List<FamiliaComponent> _todasLasFamilias = new List<FamiliaComponent>();
        private List<RolBE> _todosLosRoles = new List<RolBE>();

        public FormRoles()
        {
            InitializeComponent();
            CargarPatentes();
            CargarFamilias();
            ActualizarGrilla();
        }

        // ── Helpers ───────────────────────────────────────────────────────

        private void CargarPatentes()
        {
            try
            {
                _todasLasPatentes = new PatentesBLL().ListarPatentes();
                clbPatentes.Items.Clear();
                foreach (var p in _todasLasPatentes)
                    clbPatentes.Items.Add(p, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar patentes: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarFamilias()
        {
            try
            {
                _todasLasFamilias = new FamiliasBLL().ListarFamilias();
                clbFamilias.Items.Clear();
                foreach (var f in _todasLasFamilias)
                    clbFamilias.Items.Add(f, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar familias: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarGrilla()
        {
            try
            {
                _todosLosRoles = new RolesBLL().ListarRoles();

                var display = new System.Data.DataTable();
                display.Columns.Add("ID", typeof(int));
                display.Columns.Add("Nombre", typeof(string));
                display.Columns.Add("Descripcion", typeof(string));
                display.Columns.Add("Permisos", typeof(string));

                var rolesBLL = new RolesBLL();

                foreach (var r in _todosLosRoles)
                {
                    // Load the full tree so we can show a readable summary
                    RolBE completo = rolesBLL.ObtenerRolConPermisos(r.IdRol);
                    var nombres = new List<string>();
                    foreach (var c in completo.Componentes)
                        nombres.Add(c.Nombre);

                    display.Rows.Add(
                        r.IdRol,
                        r.NombreRol,
                        r.Descripcion,
                        string.Join(", ", nombres));
                }

                dgvRoles.DataSource = display;

                if (dgvRoles.Columns.Contains("ID"))
                    dgvRoles.Columns["ID"].Width = 40;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _idRolSeleccionado = 0;
            txtNombreRol.Clear();
            txtDescripcion.Clear();

            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, false);

            for (int i = 0; i < clbFamilias.Items.Count; i++)
                clbFamilias.SetItemChecked(i, false);
        }

        private List<int> ObtenerPatentesMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbPatentes.CheckedIndices)
                ids.Add(_todasLasPatentes[idx].Id);
            return ids;
        }

        private List<int> ObtenerFamiliasMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbFamilias.CheckedIndices)
                ids.Add(_todasLasFamilias[idx].Id);
            return ids;
        }

        // ── Grid click: load role into form ───────────────────────────────
        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvRoles.Rows[e.RowIndex];
            _idRolSeleccionado = Convert.ToInt32(row.Cells["ID"].Value);

            RolBE rol = _todosLosRoles.Find(r => r.IdRol == _idRolSeleccionado);
            if (rol == null) return;

            txtNombreRol.Text = rol.NombreRol;
            txtDescripcion.Text = rol.Descripcion;

            try
            {
                var rolesBLL = new RolesBLL();
                List<int> idsPatentesDirectas = rolesBLL.ObtenerIdPatentesPorRol(_idRolSeleccionado);
                List<int> idsFamilias = rolesBLL.ObtenerIdFamiliasPorRol(_idRolSeleccionado);

                var setPatentes = new HashSet<int>(idsPatentesDirectas);
                var setFamilias = new HashSet<int>(idsFamilias);

                for (int i = 0; i < clbPatentes.Items.Count; i++)
                {
                    var p = (PatenteComponent)clbPatentes.Items[i];
                    clbPatentes.SetItemChecked(i, setPatentes.Contains(p.Id));
                }

                for (int i = 0; i < clbFamilias.Items.Count; i++)
                {
                    var f = (FamiliaComponent)clbFamilias.Items[i];
                    clbFamilias.SetItemChecked(i, setFamilias.Contains(f.Id));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los permisos del rol: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (_idRolSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un rol de la grilla para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Block deleting a role that's still in use by some user would require
            // a check against Usuarios; the FK constraint will raise a SqlException
            // if any user still references this role, which we surface below.
            DialogResult confirm = MessageBox.Show(
                "¿Esta seguro de que desea eliminar este rol?",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = new RolesBLL().EliminarRol(_idRolSeleccionado);

                if (ok)
                {
                    MessageBox.Show("Rol eliminado.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                MessageBox.Show(
                    "No se puede eliminar este rol porque hay usuarios asignados a el.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar rol",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCrearRol_Click(object sender, EventArgs e)
        {
            try
            {
                var rol = new RolBE
                {
                    NombreRol = txtNombreRol.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();
                List<int> idsFamilias = ObtenerFamiliasMarcadas();

                bool ok = new RolesBLL().CrearRol(rol, idsPatentes, idsFamilias);

                if (ok)
                {
                    MessageBox.Show("Rol creado correctamente.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al crear rol",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (_idRolSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un rol de la grilla para modificar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var rol = new RolBE
                {
                    IdRol = _idRolSeleccionado,
                    NombreRol = txtNombreRol.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();
                List<int> idsFamilias = ObtenerFamiliasMarcadas();

                bool ok = new RolesBLL().ModificarRol(rol, idsPatentes, idsFamilias);

                if (ok)
                {
                    MessageBox.Show("Rol modificado correctamente.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al modificar rol",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}
