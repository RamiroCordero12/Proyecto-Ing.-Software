using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Ing._Software
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUsuarios formUsuarios = new FormUsuarios();
            formUsuarios.Show();
        }
    }
}
