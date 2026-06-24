namespace Proyecto_Ing._Software
{
    partial class FormMenuPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarContrasenaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarLenguajeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.familiasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnProbarDigito = new System.Windows.Forms.Button();
            this.btnPruebaCrearUsuario = new System.Windows.Forms.Button();
            this.btnPruebaLogin = new System.Windows.Forms.Button();
            this.btnPruebaDesbloquearUsuario = new System.Windows.Forms.Button();
            this.btnPruebaModificarUsuario = new System.Windows.Forms.Button();
            this.btnPruebaCambiarClave = new System.Windows.Forms.Button();
            this.btnPruebaLogout = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuarioToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.bitacoraToolStripMenuItem,
            this.cambiarContrasenaToolStripMenuItem,
            this.reloginToolStripMenuItem,
            this.cambiarLenguajeToolStripMenuItem,
            this.familiasToolStripMenuItem,
            this.rolesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(762, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usuarioToolStripMenuItem
            // 
            this.usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
            this.usuarioToolStripMenuItem.Size = new System.Drawing.Size(116, 20);
            this.usuarioToolStripMenuItem.Text = "Gestor de usuarios";
            this.usuarioToolStripMenuItem.Click += new System.EventHandler(this.usuarioToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // bitacoraToolStripMenuItem
            // 
            this.bitacoraToolStripMenuItem.Name = "bitacoraToolStripMenuItem";
            this.bitacoraToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.bitacoraToolStripMenuItem.Text = "Bitacora";
            this.bitacoraToolStripMenuItem.Click += new System.EventHandler(this.bitacoraToolStripMenuItem_Click_1);
            // 
            // cambiarContrasenaToolStripMenuItem
            // 
            this.cambiarContrasenaToolStripMenuItem.Name = "cambiarContrasenaToolStripMenuItem";
            this.cambiarContrasenaToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
            this.cambiarContrasenaToolStripMenuItem.Text = "Cambiar contrasena";
            this.cambiarContrasenaToolStripMenuItem.Click += new System.EventHandler(this.cambiarContrasenaToolStripMenuItem_Click);
            // 
            // reloginToolStripMenuItem
            // 
            this.reloginToolStripMenuItem.Name = "reloginToolStripMenuItem";
            this.reloginToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reloginToolStripMenuItem.Text = "Relogin";
            this.reloginToolStripMenuItem.Click += new System.EventHandler(this.reloginToolStripMenuItem_Click);
            // 
            // cambiarLenguajeToolStripMenuItem
            // 
            this.cambiarLenguajeToolStripMenuItem.Name = "cambiarLenguajeToolStripMenuItem";
            this.cambiarLenguajeToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.cambiarLenguajeToolStripMenuItem.Text = "Cambiar lenguaje";
            this.cambiarLenguajeToolStripMenuItem.Click += new System.EventHandler(this.cambiarLenguajeToolStripMenuItem_Click);
            // 
            // familiasToolStripMenuItem
            // 
            this.familiasToolStripMenuItem.Name = "familiasToolStripMenuItem";
            this.familiasToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.familiasToolStripMenuItem.Text = "Familias";
            this.familiasToolStripMenuItem.Click += new System.EventHandler(this.familiasToolStripMenuItem_Click);
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.rolesToolStripMenuItem.Text = "Roles";
            this.rolesToolStripMenuItem.Click += new System.EventHandler(this.rolesToolStripMenuItem_Click);
            //
            // btnProbarDigito
            //
            this.btnProbarDigito.Location = new System.Drawing.Point(12, 40);
            this.btnProbarDigito.Name = "btnProbarDigito";
            this.btnProbarDigito.Size = new System.Drawing.Size(220, 30);
            this.btnProbarDigito.TabIndex = 1;
            this.btnProbarDigito.Text = "Probar digito verificador";
            this.btnProbarDigito.UseVisualStyleBackColor = true;
            this.btnProbarDigito.Click += new System.EventHandler(this.btnProbarDigito_Click);
            //
            // btnPruebaCrearUsuario
            //
            this.btnPruebaCrearUsuario.Location = new System.Drawing.Point(250, 40);
            this.btnPruebaCrearUsuario.Name = "btnPruebaCrearUsuario";
            this.btnPruebaCrearUsuario.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaCrearUsuario.TabIndex = 2;
            this.btnPruebaCrearUsuario.Text = "Prueba crear usuario";
            this.btnPruebaCrearUsuario.UseVisualStyleBackColor = true;
            this.btnPruebaCrearUsuario.Click += new System.EventHandler(this.btnPruebaCrearUsuario_Click);
            //
            // btnPruebaLogin
            //
            this.btnPruebaLogin.Location = new System.Drawing.Point(12, 80);
            this.btnPruebaLogin.Name = "btnPruebaLogin";
            this.btnPruebaLogin.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaLogin.TabIndex = 3;
            this.btnPruebaLogin.Text = "Prueba login";
            this.btnPruebaLogin.UseVisualStyleBackColor = true;
            this.btnPruebaLogin.Click += new System.EventHandler(this.btnPruebaLogin_Click);
            //
            // btnPruebaDesbloquearUsuario
            //
            this.btnPruebaDesbloquearUsuario.Location = new System.Drawing.Point(250, 80);
            this.btnPruebaDesbloquearUsuario.Name = "btnPruebaDesbloquearUsuario";
            this.btnPruebaDesbloquearUsuario.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaDesbloquearUsuario.TabIndex = 4;
            this.btnPruebaDesbloquearUsuario.Text = "Prueba desbloquear usuario";
            this.btnPruebaDesbloquearUsuario.UseVisualStyleBackColor = true;
            this.btnPruebaDesbloquearUsuario.Click += new System.EventHandler(this.btnPruebaDesbloquearUsuario_Click);
            //
            // btnPruebaModificarUsuario
            //
            this.btnPruebaModificarUsuario.Location = new System.Drawing.Point(12, 120);
            this.btnPruebaModificarUsuario.Name = "btnPruebaModificarUsuario";
            this.btnPruebaModificarUsuario.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaModificarUsuario.TabIndex = 5;
            this.btnPruebaModificarUsuario.Text = "Prueba modificar usuario";
            this.btnPruebaModificarUsuario.UseVisualStyleBackColor = true;
            this.btnPruebaModificarUsuario.Click += new System.EventHandler(this.btnPruebaModificarUsuario_Click);
            //
            // btnPruebaCambiarClave
            //
            this.btnPruebaCambiarClave.Location = new System.Drawing.Point(250, 120);
            this.btnPruebaCambiarClave.Name = "btnPruebaCambiarClave";
            this.btnPruebaCambiarClave.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaCambiarClave.TabIndex = 6;
            this.btnPruebaCambiarClave.Text = "Pruebas cambiar clave";
            this.btnPruebaCambiarClave.UseVisualStyleBackColor = true;
            this.btnPruebaCambiarClave.Click += new System.EventHandler(this.btnPruebaCambiarClave_Click);
            //
            // btnPruebaLogout
            //
            this.btnPruebaLogout.Location = new System.Drawing.Point(12, 160);
            this.btnPruebaLogout.Name = "btnPruebaLogout";
            this.btnPruebaLogout.Size = new System.Drawing.Size(220, 30);
            this.btnPruebaLogout.TabIndex = 7;
            this.btnPruebaLogout.Text = "Pruebas logout";
            this.btnPruebaLogout.UseVisualStyleBackColor = true;
            this.btnPruebaLogout.Click += new System.EventHandler(this.btnPruebaLogout_Click);
            //
            // FormMenuPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.ForestGreen;
            this.ClientSize = new System.Drawing.Size(762, 248);
            this.Controls.Add(this.btnPruebaLogout);
            this.Controls.Add(this.btnPruebaCambiarClave);
            this.Controls.Add(this.btnPruebaModificarUsuario);
            this.Controls.Add(this.btnPruebaDesbloquearUsuario);
            this.Controls.Add(this.btnPruebaLogin);
            this.Controls.Add(this.btnPruebaCrearUsuario);
            this.Controls.Add(this.btnProbarDigito);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMenuPrincipal";
            this.Text = "FormMenuPrincipal";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem usuarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacoraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarContrasenaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarLenguajeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem familiasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolesToolStripMenuItem;
        private System.Windows.Forms.Button btnProbarDigito;
        private System.Windows.Forms.Button btnPruebaCrearUsuario;
        private System.Windows.Forms.Button btnPruebaLogin;
        private System.Windows.Forms.Button btnPruebaDesbloquearUsuario;
        private System.Windows.Forms.Button btnPruebaModificarUsuario;
        private System.Windows.Forms.Button btnPruebaCambiarClave;
        private System.Windows.Forms.Button btnPruebaLogout;
    }
}

