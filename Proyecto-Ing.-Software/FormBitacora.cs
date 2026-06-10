using System;
using System.Windows.Forms;
using BLL;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormBitacora : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;

        public FormBitacora()
        {
            InitializeComponent();
            _loc.Subscribe(this);
            ApplyLocalization();
        }

        // ─────────────────────────────────────────
        //  ILocalizationObserver
        // ─────────────────────────────────────────

        public void OnLanguageChanged() => ApplyLocalization();

        private void ApplyLocalization()
        {
            this.Text = _loc["FormBitacora", "Title"];
            label1.Text = _loc["FormBitacora", "Title"];
            lblDesde.Text = _loc["FormBitacora", "LabelDesde"];
            lblHasta.Text = _loc["FormBitacora", "LabelHasta"];
            btnFiltrar.Text = _loc["FormBitacora", "ButtonFiltrar"];
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Helpers
        // ─────────────────────────────────────────

        private void ActualizarDataGridView()
        {
            try
            {
                DgvBitacora.DataSource = null;
                DgvBitacora.DataSource = new BitacoraBLL().ListarBitacora();
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormBitacora", "MsgErrorCargar"] + ex.Message);
            }
        }

        // ─────────────────────────────────────────
        //  Event handlers
        // ─────────────────────────────────────────

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            ActualizarDataGridView();
        }

        private void bitacora_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            DateTime desde = dateTimeDesde.Value.Date;
            DateTime hasta = dateTimeHasta.Value.Date.AddDays(1).AddTicks(-1);

            if (hasta < desde)
            {
                MessageBox.Show(
                    _loc["FormBitacora", "ErrorFechaInvalida"],
                    _loc["FormBitacora", "ErrorFechaInvalidaTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                else if (cellVal != null &&
                         DateTime.TryParse(cellVal.ToString(), out rowDate))
                {
                    // parsed successfully
                }
                else
                {
                    if (DgvBitacora.CurrentCell?.OwningRow == row)
                        DgvBitacora.CurrentCell = null;
                    row.Visible = false;
                    continue;
                }

                bool inRange = rowDate >= desde && rowDate <= hasta;

                if (!inRange && DgvBitacora.CurrentCell?.OwningRow == row)
                    DgvBitacora.CurrentCell = null;

                row.Visible = inRange;
                if (inRange) anyVisible = true;
            }

            if (!anyVisible)
            {
                MessageBox.Show(
                    _loc["FormBitacora", "ErrorSinResultados"],
                    _loc["FormBitacora", "ErrorSinResultadosTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}