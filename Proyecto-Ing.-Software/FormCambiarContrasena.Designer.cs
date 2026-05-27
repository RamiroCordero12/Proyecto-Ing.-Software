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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Contrasena (Actual)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Contrasena nueva";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(270, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Confirmar contrasena";
            // 
            // FormCambiarContrasena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(500, 230);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}