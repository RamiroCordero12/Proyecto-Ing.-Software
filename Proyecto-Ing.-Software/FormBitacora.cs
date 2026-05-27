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

namespace Proyecto_Ing._Software
{
    public partial class FormBitacora : Form
    {
        public FormBitacora()
        {
            InitializeComponent();
        }

        private void ActualizarDataGridView()
        {
            try
            {
                BitacoraBLL listBitacora = new BitacoraBLL();

                DgvBitacora.DataSource = null;
                DgvBitacora.DataSource = listBitacora.ListarBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message);
            }

        }
        
        
        
        private void FormBitacora_Load(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void bitacora_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            const string ErrToBeforeFrom = "La fecha 'Hasta' no puede ser anterior a la fecha 'Desde'.";
            const string ErrNoResults = "No se encontraron registros en el rango de fechas seleccionado.";

            DateTime desde = dateTimeDesde.Value.Date;
            DateTime hasta = dateTimeHasta.Value.Date.AddDays(1).AddTicks(-1);

            if (hasta < desde)
            {
                MessageBox.Show(ErrToBeforeFrom, "Filtro inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool anyVisible = false;

            foreach (DataGridViewRow row in DgvBitacora.Rows)
            {
                if (row.IsNewRow) continue;

                object cellVal = row.Cells["FechaHora"].Value;
                DateTime rowDate;

                if (cellVal is DateTime dt)
                {
                    rowDate = dt;
                }
                else if (cellVal != null && DateTime.TryParse(cellVal.ToString(), out rowDate))
                {
                    // rowDate ya asignada por TryParse
                }
                else
                {
                    if (DgvBitacora.CurrentCell != null && DgvBitacora.CurrentCell.OwningRow == row)
                        DgvBitacora.CurrentCell = null;

                    row.Visible = false;
                    continue;
                }

                bool inRange = rowDate >= desde && rowDate <= hasta;

                if (!inRange && DgvBitacora.CurrentCell != null && DgvBitacora.CurrentCell.OwningRow == row)
                    DgvBitacora.CurrentCell = null;

                row.Visible = inRange;
                if (inRange) anyVisible = true;
            }

            if (!anyVisible)
            {
                MessageBox.Show(ErrNoResults, "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
