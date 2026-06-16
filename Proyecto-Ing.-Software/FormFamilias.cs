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
    public partial class FormFamilias : Form
    {
        private int _idFamiliaSeleccionada = 0;
        private List<PatenteComponent> _todasLasPatentes = new List<PatenteComponent>();
        private List<FamiliaComponent> _todasLasFamilias = new List<FamiliaComponent>();

        public FormFamilias()
        {
            InitializeComponent();
            CargarPatentes();
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

        private void ActualizarGrilla()
        {
            try
            {
                _todasLasFamilias = new FamiliasBLL().ListarFamilias();

                // Build a flat DataTable-like list for display
                var display = new System.Data.DataTable();
                display.Columns.Add("ID", typeof(int));
                display.Columns.Add("Nombre", typeof(string));
                display.Columns.Add("Descripcion", typeof(string));
                display.Columns.Add("Patentes", typeof(string));

                foreach (var f in _todasLasFamilias)
                {
                    var nombres = new List<string>();
                    foreach (var h in f.Hijos)
                        nombres.Add(h.Nombre);

                    display.Rows.Add(
                        f.Id,
                        f.Nombre,
                        f.Descripcion,
                        string.Join(", ", nombres));
                }

                dgvFamilias.DataSource = display;

                if (dgvFamilias.Columns.Contains("ID"))
                    dgvFamilias.Columns["ID"].Width = 40;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar familias: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _idFamiliaSeleccionada = 0;
            txtNombre.Clear();
            txtDescripcion.Clear();
            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, false);
        }

        private List<int> ObtenerPatentesMarcadas()
        {
            var ids = new List<int>();
            foreach (int idx in clbPatentes.CheckedIndices)
                ids.Add(_todasLasPatentes[idx].Id);
            return ids;
        }

        // ── Grid click: load row into form ────────────────────────────────
        private void dgvFamilias_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvFamilias.Rows[e.RowIndex];
            _idFamiliaSeleccionada = Convert.ToInt32(row.Cells["ID"].Value);

            // Find the FamiliaComponent
            FamiliaComponent fam = _todasLasFamilias.Find(f => f.Id == _idFamiliaSeleccionada);
            if (fam == null) return;

            txtNombre.Text = fam.Nombre;
            txtDescripcion.Text = fam.Descripcion;

            // Tick only the patents this family contains
            var idsEnFamilia = new HashSet<int>();
            foreach (var h in fam.Hijos)
                idsEnFamilia.Add(h.Id);

            for (int i = 0; i < clbPatentes.Items.Count; i++)
            {
                var p = (PatenteComponent)clbPatentes.Items[i];
                clbPatentes.SetItemChecked(i, idsEnFamilia.Contains(p.Id));
            }
        }
       

        

       
       

        private void btnCrearFamilia_Click(object sender, EventArgs e)
        {

            try
            {
                var familia = new FamiliaComponent
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();

                bool ok = new FamiliasBLL().CrearFamilia(familia, idsPatentes);

                if (ok)
                {
                    MessageBox.Show("Familia creada correctamente.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al crear familia",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();

        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (_idFamiliaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una familia de la grilla para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "¿Esta seguro de que desea eliminar esta familia?",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool ok = new FamiliasBLL().EliminarFamilia(_idFamiliaSeleccionada);

                if (ok)
                {
                    MessageBox.Show("Familia eliminada.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al eliminar familia",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (_idFamiliaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione una familia de la grilla para modificar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var familia = new FamiliaComponent
                {
                    Id = _idFamiliaSeleccionada,
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                List<int> idsPatentes = ObtenerPatentesMarcadas();

                bool ok = new FamiliasBLL().ModificarFamilia(familia, idsPatentes);

                if (ok)
                {
                    MessageBox.Show("Familia modificada correctamente.",
                        "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    ActualizarGrilla();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al modificar familia",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
