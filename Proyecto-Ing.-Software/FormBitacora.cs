using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormBitacora : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;
        private List<Bitacora> _todasLasEntradas = new List<Bitacora>();

        public FormBitacora()
        {
            InitializeComponent();

            if (SessionManager.GetInstance.Permisos != null &&
                !SessionManager.GetInstance.Permisos.Tiene(Patente.Bitacora))
            {
                MessageBox.Show("No tiene permisos para acceder a este modulo.",
                    "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Load += (s, e) => this.Close();
                return;
            }

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

            lblCmbUsuario.Text = _loc["FormBitacora", "LabelPorUsuario"];
            label2.Text = _loc["FormBitacora", "LabelPorEvento"];
            label3.Text = _loc["FormBitacora", "LabelPorCriticidad"];
            lblModulo.Text = _loc["FormBitacora", "LabelPorModulo"];

            // Rebuild combos only after data has been loaded
            if (_todasLasEntradas.Count > 0)
            {
                PopularCombo(cmbUsuario, _todasLasEntradas.Select(b => b.Usuario).Distinct().OrderBy(x => x));
                PopularCombo(cmbEvento, _todasLasEntradas.Select(b => b.Accion).Distinct().OrderBy(x => x));
                PopularCombo(cmbModulo, _todasLasEntradas.Select(b => b.Modulo).Distinct().OrderBy(x => x));
                PopularCombo(cmbCriticidad, _todasLasEntradas.Select(b => b.Criticidad).Distinct().OrderBy(x => x));
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Data loading
        // ─────────────────────────────────────────

        private void ActualizarDataGridView()
        {
            try
            {
                _todasLasEntradas = new BitacoraBLL().ListarBitacora();

                PopularCombo(cmbUsuario, _todasLasEntradas.Select(b => b.Usuario).Distinct().OrderBy(x => x));
                PopularCombo(cmbEvento, _todasLasEntradas.Select(b => b.Accion).Distinct().OrderBy(x => x));
                PopularCombo(cmbModulo, _todasLasEntradas.Select(b => b.Modulo).Distinct().OrderBy(x => x));
                PopularCombo(cmbCriticidad, _todasLasEntradas.Select(b => b.Criticidad).Distinct().OrderBy(x => x));

                DgvBitacora.DataSource = null;
                DgvBitacora.DataSource = _todasLasEntradas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_loc["FormBitacora", "MsgErrorCargar"] + ex.Message);
            }
        }

        private void PopularCombo(ComboBox cmb, IEnumerable<string> valores)
        {
            string seleccion = cmb.SelectedItem as string;

            cmb.Items.Clear();

            // Fallback label if the JSON key is not yet deployed
            string todos = _loc["FormBitacora", "OpcionTodos"];
            if (todos.StartsWith("[")) todos = "(Todos)";

            cmb.Items.Add(todos);
            foreach (string v in valores)
                if (!string.IsNullOrWhiteSpace(v))
                    cmb.Items.Add(v);

            int idx = seleccion != null ? cmb.Items.IndexOf(seleccion) : -1;
            cmb.SelectedIndex = idx >= 0 ? idx : 0;
        }

        // ─────────────────────────────────────────
        //  Filtering
        // ─────────────────────────────────────────

        private void AplicarFiltros()
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

            string filtroUsuario = SelectedFilter(cmbUsuario);
            string filtroEvento = SelectedFilter(cmbEvento);
            string filtroModulo = SelectedFilter(cmbModulo);
            string filtroCriticidad = SelectedFilter(cmbCriticidad);

            var resultado = _todasLasEntradas
                .Where(b => b.FechaHora >= desde && b.FechaHora <= hasta)
                .Where(b => filtroUsuario == null || b.Usuario == filtroUsuario)
                .Where(b => filtroEvento == null || b.Accion == filtroEvento)
                .Where(b => filtroModulo == null || b.Modulo == filtroModulo)
                .Where(b => filtroCriticidad == null || b.Criticidad == filtroCriticidad)
                .ToList();

            DgvBitacora.DataSource = null;
            DgvBitacora.DataSource = resultado;

            if (resultado.Count == 0)
            {
                MessageBox.Show(
                    _loc["FormBitacora", "ErrorSinResultados"],
                    _loc["FormBitacora", "ErrorSinResultadosTitle"],
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string SelectedFilter(ComboBox cmb)
        {
            if (cmb.SelectedIndex <= 0) return null;
            return cmb.SelectedItem as string;
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
            AplicarFiltros();
        }
    }
}