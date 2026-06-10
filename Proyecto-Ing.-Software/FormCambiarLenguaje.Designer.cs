namespace Proyecto_Ing._Software
{
    partial class FormCambiarLenguaje
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
            this.cmbLenguaje = new System.Windows.Forms.ComboBox();
            this.lblCambiarLenguaje = new System.Windows.Forms.Label();
            this.btnCambiarLenguaje = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbLenguaje
            // 
            this.cmbLenguaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLenguaje.FormattingEnabled = true;
            this.cmbLenguaje.Items.AddRange(new object[] {
            "Español",
            "English",
            "Portuges"});
            this.cmbLenguaje.Location = new System.Drawing.Point(25, 44);
            this.cmbLenguaje.Name = "cmbLenguaje";
            this.cmbLenguaje.Size = new System.Drawing.Size(121, 21);
            this.cmbLenguaje.TabIndex = 25;
            // 
            // lblCambiarLenguaje
            // 
            this.lblCambiarLenguaje.AutoSize = true;
            this.lblCambiarLenguaje.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCambiarLenguaje.Location = new System.Drawing.Point(22, 28);
            this.lblCambiarLenguaje.Name = "lblCambiarLenguaje";
            this.lblCambiarLenguaje.Size = new System.Drawing.Size(91, 13);
            this.lblCambiarLenguaje.TabIndex = 24;
            this.lblCambiarLenguaje.Text = "Cambiar lenguaje:";
            // 
            // btnCambiarLenguaje
            // 
            this.btnCambiarLenguaje.Location = new System.Drawing.Point(25, 85);
            this.btnCambiarLenguaje.Name = "btnCambiarLenguaje";
            this.btnCambiarLenguaje.Size = new System.Drawing.Size(75, 23);
            this.btnCambiarLenguaje.TabIndex = 26;
            this.btnCambiarLenguaje.Text = "Aceptar";
            this.btnCambiarLenguaje.UseVisualStyleBackColor = true;
            this.btnCambiarLenguaje.Click += new System.EventHandler(this.btnCambiarLenguaje_Click);
            // 
            // FormCambiarLenguaje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.ClientSize = new System.Drawing.Size(222, 132);
            this.Controls.Add(this.btnCambiarLenguaje);
            this.Controls.Add(this.cmbLenguaje);
            this.Controls.Add(this.lblCambiarLenguaje);
            this.Name = "FormCambiarLenguaje";
            this.Text = "FormCambiarLenguaje";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLenguaje;
        private System.Windows.Forms.Label lblCambiarLenguaje;
        private System.Windows.Forms.Button btnCambiarLenguaje;
    }
}