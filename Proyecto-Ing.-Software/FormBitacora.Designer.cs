namespace Proyecto_Ing._Software
{
    partial class FormBitacora
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DgvBitacora = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeDesde = new System.Windows.Forms.DateTimePicker();
            this.dateTimeHasta = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.lblHasta = new System.Windows.Forms.Label();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.cmbUsuario = new System.Windows.Forms.ComboBox();
            this.cmbEvento = new System.Windows.Forms.ComboBox();
            this.cmbCriticidad = new System.Windows.Forms.ComboBox();
            this.lblCmbUsuario = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblModulo = new System.Windows.Forms.Label();
            this.cmbModulo = new System.Windows.Forms.ComboBox();
            this.btnLimpiarBitacora = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvBitacora)).BeginInit();
            this.SuspendLayout();
            // 
            // DgvBitacora
            // 
            this.DgvBitacora.AllowUserToAddRows = false;
            this.DgvBitacora.AllowUserToDeleteRows = false;
            this.DgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvBitacora.Location = new System.Drawing.Point(12, 52);
            this.DgvBitacora.Name = "DgvBitacora";
            this.DgvBitacora.ReadOnly = true;
            this.DgvBitacora.Size = new System.Drawing.Size(703, 339);
            this.DgvBitacora.TabIndex = 0;
            this.DgvBitacora.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.bitacora_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(349, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "Bitacora de eventos";
            // 
            // dateTimeDesde
            // 
            this.dateTimeDesde.Location = new System.Drawing.Point(721, 76);
            this.dateTimeDesde.Name = "dateTimeDesde";
            this.dateTimeDesde.Size = new System.Drawing.Size(200, 20);
            this.dateTimeDesde.TabIndex = 2;
            // 
            // dateTimeHasta
            // 
            this.dateTimeHasta.Location = new System.Drawing.Point(721, 125);
            this.dateTimeHasta.Name = "dateTimeHasta";
            this.dateTimeHasta.Size = new System.Drawing.Size(200, 20);
            this.dateTimeHasta.TabIndex = 3;
            // 
            // lblDesde
            // 
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(721, 55);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(70, 13);
            this.lblDesde.TabIndex = 4;
            this.lblDesde.Text = "Filtrar desde: ";
            // 
            // lblHasta
            // 
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(721, 104);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(35, 13);
            this.lblHasta.TabIndex = 5;
            this.lblHasta.Text = "Hasta";
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(724, 368);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btnFiltrar.TabIndex = 6;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            //
            // btnLimpiarBitacora
            //
            this.btnLimpiarBitacora.Location = new System.Drawing.Point(809, 368);
            this.btnLimpiarBitacora.Name = "btnLimpiarBitacora";
            this.btnLimpiarBitacora.Size = new System.Drawing.Size(112, 23);
            this.btnLimpiarBitacora.TabIndex = 15;
            this.btnLimpiarBitacora.Text = "Limpiar bitácora";
            this.btnLimpiarBitacora.UseVisualStyleBackColor = true;
            this.btnLimpiarBitacora.Click += new System.EventHandler(this.btnLimpiarBitacora_Click);
            //
            // cmbUsuario
            // 
            this.cmbUsuario.FormattingEnabled = true;
            this.cmbUsuario.Location = new System.Drawing.Point(721, 174);
            this.cmbUsuario.Name = "cmbUsuario";
            this.cmbUsuario.Size = new System.Drawing.Size(121, 21);
            this.cmbUsuario.TabIndex = 7;
            // 
            // cmbEvento
            // 
            this.cmbEvento.FormattingEnabled = true;
            this.cmbEvento.Location = new System.Drawing.Point(721, 224);
            this.cmbEvento.Name = "cmbEvento";
            this.cmbEvento.Size = new System.Drawing.Size(121, 21);
            this.cmbEvento.TabIndex = 8;
            // 
            // cmbCriticidad
            // 
            this.cmbCriticidad.FormattingEnabled = true;
            this.cmbCriticidad.Location = new System.Drawing.Point(721, 274);
            this.cmbCriticidad.Name = "cmbCriticidad";
            this.cmbCriticidad.Size = new System.Drawing.Size(121, 21);
            this.cmbCriticidad.TabIndex = 9;
            // 
            // lblCmbUsuario
            // 
            this.lblCmbUsuario.AutoSize = true;
            this.lblCmbUsuario.Location = new System.Drawing.Point(721, 153);
            this.lblCmbUsuario.Name = "lblCmbUsuario";
            this.lblCmbUsuario.Size = new System.Drawing.Size(63, 13);
            this.lblCmbUsuario.TabIndex = 10;
            this.lblCmbUsuario.Text = "Por usuario:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(721, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Por evento:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(721, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Por criticidad:";
            // 
            // lblModulo
            // 
            this.lblModulo.AutoSize = true;
            this.lblModulo.Location = new System.Drawing.Point(721, 303);
            this.lblModulo.Name = "lblModulo";
            this.lblModulo.Size = new System.Drawing.Size(63, 13);
            this.lblModulo.TabIndex = 14;
            this.lblModulo.Text = "Por módulo:";
            // 
            // cmbModulo
            // 
            this.cmbModulo.FormattingEnabled = true;
            this.cmbModulo.Location = new System.Drawing.Point(721, 324);
            this.cmbModulo.Name = "cmbModulo";
            this.cmbModulo.Size = new System.Drawing.Size(121, 21);
            this.cmbModulo.TabIndex = 13;
            // 
            // FormBitacora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ShowIcon = false;
            this.ClientSize = new System.Drawing.Size(933, 439);
            this.Controls.Add(this.lblModulo);
            this.Controls.Add(this.cmbModulo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCmbUsuario);
            this.Controls.Add(this.cmbCriticidad);
            this.Controls.Add(this.cmbEvento);
            this.Controls.Add(this.cmbUsuario);
            this.Controls.Add(this.btnLimpiarBitacora);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.dateTimeHasta);
            this.Controls.Add(this.dateTimeDesde);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DgvBitacora);
            this.Name = "FormBitacora";
            this.Text = "FormBitacora";
            this.Load += new System.EventHandler(this.FormBitacora_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvBitacora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DgvBitacora;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimeDesde;
        private System.Windows.Forms.DateTimePicker dateTimeHasta;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.ComboBox cmbUsuario;
        private System.Windows.Forms.ComboBox cmbEvento;
        private System.Windows.Forms.ComboBox cmbCriticidad;
        private System.Windows.Forms.Label lblCmbUsuario;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblModulo;
        private System.Windows.Forms.ComboBox cmbModulo;
        private System.Windows.Forms.Button btnLimpiarBitacora;
    }
}