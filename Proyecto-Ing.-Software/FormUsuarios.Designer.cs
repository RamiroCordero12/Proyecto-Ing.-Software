namespace Proyecto_Ing._Software
{
    partial class FormUsuarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsuarios));
            this.dgvUsuario = new System.Windows.Forms.DataGridView();
            this.btnCrearUsuario = new System.Windows.Forms.Button();
            this.btnDeshabilitarUsuario = new System.Windows.Forms.Button();
            this.btnModificarUsuario = new System.Windows.Forms.Button();
            this.lblCambiarRol = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.cmbRoles = new System.Windows.Forms.ComboBox();
            this.btnHabilitarUsuario = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.cmbLenguaje = new System.Windows.Forms.ComboBox();
            this.lblCambiarLenguaje = new System.Windows.Forms.Label();
            this.lblFiltroEstado = new System.Windows.Forms.Label();
            this.cmbFiltroEstado = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuario)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUsuario
            // 
            this.dgvUsuario.AllowUserToAddRows = false;
            this.dgvUsuario.AllowUserToDeleteRows = false;
            this.dgvUsuario.AllowUserToResizeColumns = false;
            this.dgvUsuario.AllowUserToResizeRows = false;
            this.dgvUsuario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvUsuario, "dgvUsuario");
            this.dgvUsuario.Name = "dgvUsuario";
            this.dgvUsuario.ReadOnly = true;
            this.dgvUsuario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuario.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsuario_CellClick);
            // 
            // btnCrearUsuario
            // 
            resources.ApplyResources(this.btnCrearUsuario, "btnCrearUsuario");
            this.btnCrearUsuario.Name = "btnCrearUsuario";
            this.btnCrearUsuario.UseVisualStyleBackColor = true;
            this.btnCrearUsuario.Click += new System.EventHandler(this.btnCrearUsuario_Click);
            // 
            // btnDeshabilitarUsuario
            // 
            resources.ApplyResources(this.btnDeshabilitarUsuario, "btnDeshabilitarUsuario");
            this.btnDeshabilitarUsuario.Name = "btnDeshabilitarUsuario";
            this.btnDeshabilitarUsuario.UseVisualStyleBackColor = true;
            this.btnDeshabilitarUsuario.Click += new System.EventHandler(this.btnDeshabilitarUsuario_Click);
            // 
            // btnModificarUsuario
            // 
            resources.ApplyResources(this.btnModificarUsuario, "btnModificarUsuario");
            this.btnModificarUsuario.Name = "btnModificarUsuario";
            this.btnModificarUsuario.UseVisualStyleBackColor = true;
            this.btnModificarUsuario.Click += new System.EventHandler(this.btnModificarUsuario_Click);
            // 
            // lblCambiarRol
            // 
            resources.ApplyResources(this.lblCambiarRol, "lblCambiarRol");
            this.lblCambiarRol.Name = "lblCambiarRol";
            // 
            // lblEmail
            // 
            resources.ApplyResources(this.lblEmail, "lblEmail");
            this.lblEmail.Name = "lblEmail";
            // 
            // txtEmail
            // 
            resources.ApplyResources(this.txtEmail, "txtEmail");
            this.txtEmail.Name = "txtEmail";
            // 
            // cmbRoles
            // 
            this.cmbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoles.FormattingEnabled = true;
            this.cmbRoles.Items.AddRange(new object[] {
            resources.GetString("cmbRoles.Items"),
            resources.GetString("cmbRoles.Items1")});
            resources.ApplyResources(this.cmbRoles, "cmbRoles");
            this.cmbRoles.Name = "cmbRoles";
            this.cmbRoles.SelectedIndexChanged += new System.EventHandler(this.cmbRoles_SelectedIndexChanged);
            // 
            // btnHabilitarUsuario
            // 
            resources.ApplyResources(this.btnHabilitarUsuario, "btnHabilitarUsuario");
            this.btnHabilitarUsuario.Name = "btnHabilitarUsuario";
            this.btnHabilitarUsuario.UseVisualStyleBackColor = true;
            this.btnHabilitarUsuario.Click += new System.EventHandler(this.btnHabilitarUsuario_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lblApellido
            // 
            resources.ApplyResources(this.lblApellido, "lblApellido");
            this.lblApellido.Name = "lblApellido";
            // 
            // lblNombre
            // 
            resources.ApplyResources(this.lblNombre, "lblNombre");
            this.lblNombre.Name = "lblNombre";
            // 
            // txtApellido
            // 
            resources.ApplyResources(this.txtApellido, "txtApellido");
            this.txtApellido.Name = "txtApellido";
            // 
            // txtNombre
            // 
            resources.ApplyResources(this.txtNombre, "txtNombre");
            this.txtNombre.Name = "txtNombre";
            // 
            // lblDNI
            // 
            resources.ApplyResources(this.lblDNI, "lblDNI");
            this.lblDNI.Name = "lblDNI";
            // 
            // txtDNI
            // 
            resources.ApplyResources(this.txtDNI, "txtDNI");
            this.txtDNI.Name = "txtDNI";
            // 
            // cmbLenguaje
            // 
            this.cmbLenguaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLenguaje.FormattingEnabled = true;
            this.cmbLenguaje.Items.AddRange(new object[] {
            resources.GetString("cmbLenguaje.Items"),
            resources.GetString("cmbLenguaje.Items1"),
            resources.GetString("cmbLenguaje.Items2")});
            resources.ApplyResources(this.cmbLenguaje, "cmbLenguaje");
            this.cmbLenguaje.Name = "cmbLenguaje";
            // 
            // lblCambiarLenguaje
            //
            resources.ApplyResources(this.lblCambiarLenguaje, "lblCambiarLenguaje");
            this.lblCambiarLenguaje.Name = "lblCambiarLenguaje";
            //
            // lblFiltroEstado
            //
            this.lblFiltroEstado.AutoSize = true;
            this.lblFiltroEstado.Location = new System.Drawing.Point(23, 9);
            this.lblFiltroEstado.Name = "lblFiltroEstado";
            this.lblFiltroEstado.Size = new System.Drawing.Size(48, 13);
            this.lblFiltroEstado.TabIndex = 16;
            this.lblFiltroEstado.Text = "Mostrar:";
            //
            // cmbFiltroEstado
            //
            this.cmbFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroEstado.FormattingEnabled = true;
            this.cmbFiltroEstado.Location = new System.Drawing.Point(90, 5);
            this.cmbFiltroEstado.Name = "cmbFiltroEstado";
            this.cmbFiltroEstado.Size = new System.Drawing.Size(160, 21);
            this.cmbFiltroEstado.TabIndex = 17;
            this.cmbFiltroEstado.SelectedIndexChanged += new System.EventHandler(this.cmbFiltroEstado_SelectedIndexChanged);
            //
            // FormUsuarios
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.Controls.Add(this.cmbFiltroEstado);
            this.Controls.Add(this.lblFiltroEstado);
            this.Controls.Add(this.cmbLenguaje);
            this.Controls.Add(this.lblCambiarLenguaje);
            this.Controls.Add(this.lblDNI);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnHabilitarUsuario);
            this.Controls.Add(this.cmbRoles);
            this.Controls.Add(this.lblCambiarRol);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnModificarUsuario);
            this.Controls.Add(this.btnDeshabilitarUsuario);
            this.Controls.Add(this.btnCrearUsuario);
            this.Controls.Add(this.dgvUsuario);
            this.Name = "FormUsuarios";
            this.Load += new System.EventHandler(this.FormUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvUsuario;
        private System.Windows.Forms.Button btnCrearUsuario;
        private System.Windows.Forms.Button btnDeshabilitarUsuario;
        private System.Windows.Forms.Button btnModificarUsuario;
        private System.Windows.Forms.Label lblCambiarRol;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.ComboBox cmbRoles;
        private System.Windows.Forms.Button btnHabilitarUsuario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.ComboBox cmbLenguaje;
        private System.Windows.Forms.Label lblCambiarLenguaje;
        private System.Windows.Forms.Label lblFiltroEstado;
        private System.Windows.Forms.ComboBox cmbFiltroEstado;
    }
}