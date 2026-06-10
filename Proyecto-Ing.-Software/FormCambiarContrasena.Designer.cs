namespace Proyecto_Ing._Software
{
    partial class FormCambiarContrasena
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
            this.txtContrasenaNueva = new System.Windows.Forms.TextBox();
            this.txtContrasenaActual = new System.Windows.Forms.TextBox();
            this.txtContrasenaConfirmar = new System.Windows.Forms.TextBox();
            this.btnCambiarContrasena = new System.Windows.Forms.Button();
            this.lblContraseñaActual = new System.Windows.Forms.Label();
            this.lblConfirmar = new System.Windows.Forms.Label();
            this.lblConfirmarContraseña = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtContrasenaNueva
            // 
            this.txtContrasenaNueva.Location = new System.Drawing.Point(40, 105);
            this.txtContrasenaNueva.Name = "txtContrasenaNueva";
            this.txtContrasenaNueva.Size = new System.Drawing.Size(180, 20);
            this.txtContrasenaNueva.TabIndex = 1;
            // 
            // txtContrasenaActual
            // 
            this.txtContrasenaActual.Location = new System.Drawing.Point(43, 35);
            this.txtContrasenaActual.Name = "txtContrasenaActual";
            this.txtContrasenaActual.Size = new System.Drawing.Size(180, 20);
            this.txtContrasenaActual.TabIndex = 2;
            // 
            // txtContrasenaConfirmar
            // 
            this.txtContrasenaConfirmar.Location = new System.Drawing.Point(270, 105);
            this.txtContrasenaConfirmar.Name = "txtContrasenaConfirmar";
            this.txtContrasenaConfirmar.Size = new System.Drawing.Size(180, 20);
            this.txtContrasenaConfirmar.TabIndex = 3;
            // 
            // btnCambiarContrasena
            // 
            this.btnCambiarContrasena.Location = new System.Drawing.Point(191, 152);
            this.btnCambiarContrasena.Name = "btnCambiarContrasena";
            this.btnCambiarContrasena.Size = new System.Drawing.Size(112, 37);
            this.btnCambiarContrasena.TabIndex = 4;
            this.btnCambiarContrasena.Text = "Confirmar";
            this.btnCambiarContrasena.UseVisualStyleBackColor = true;
            this.btnCambiarContrasena.Click += new System.EventHandler(this.btnCambiarContrasena_Click);
            // 
            // lblContraseñaActual
            // 
            this.lblContraseñaActual.AutoSize = true;
            this.lblContraseñaActual.Location = new System.Drawing.Point(40, 19);
            this.lblContraseñaActual.Name = "lblContraseñaActual";
            this.lblContraseñaActual.Size = new System.Drawing.Size(100, 13);
            this.lblContraseñaActual.TabIndex = 7;
            this.lblContraseñaActual.Text = "Contrasena (Actual)";
            // 
            // lblConfirmar
            // 
            this.lblConfirmar.AutoSize = true;
            this.lblConfirmar.Location = new System.Drawing.Point(40, 86);
            this.lblConfirmar.Name = "lblConfirmar";
            this.lblConfirmar.Size = new System.Drawing.Size(94, 13);
            this.lblConfirmar.TabIndex = 8;
            this.lblConfirmar.Text = "Contrasena nueva";
            // 
            // lblConfirmarContraseña
            // 
            this.lblConfirmarContraseña.AutoSize = true;
            this.lblConfirmarContraseña.Location = new System.Drawing.Point(270, 86);
            this.lblConfirmarContraseña.Name = "lblConfirmarContraseña";
            this.lblConfirmarContraseña.Size = new System.Drawing.Size(107, 13);
            this.lblConfirmarContraseña.TabIndex = 9;
            this.lblConfirmarContraseña.Text = "Confirmar contrasena";
            // 
            // FormCambiarContrasena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(500, 230);
            this.Controls.Add(this.lblConfirmarContraseña);
            this.Controls.Add(this.lblConfirmar);
            this.Controls.Add(this.lblContraseñaActual);
            this.Controls.Add(this.btnCambiarContrasena);
            this.Controls.Add(this.txtContrasenaConfirmar);
            this.Controls.Add(this.txtContrasenaActual);
            this.Controls.Add(this.txtContrasenaNueva);
            this.Name = "FormCambiarContrasena";
            this.Text = "FormCambiarContrasena";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtContrasenaNueva;
        private System.Windows.Forms.TextBox txtContrasenaActual;
        private System.Windows.Forms.TextBox txtContrasenaConfirmar;
        private System.Windows.Forms.Button btnCambiarContrasena;
        private System.Windows.Forms.Label lblContraseñaActual;
        private System.Windows.Forms.Label lblConfirmar;
        private System.Windows.Forms.Label lblConfirmarContraseña;
    }
}