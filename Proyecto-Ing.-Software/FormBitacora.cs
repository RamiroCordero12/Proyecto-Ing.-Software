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
    }
}
