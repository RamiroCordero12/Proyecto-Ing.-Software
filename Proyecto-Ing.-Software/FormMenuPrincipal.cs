using System;
using System.Linq;
using System.Windows.Forms;
using Servicios;
using Servicios.Localization;

namespace Proyecto_Ing._Software
{
    public partial class FormMenuPrincipal : Form, ILocalizationObserver
    {
        private readonly LocalizationService _loc = LocalizationService.Instance;
        private bool _esAdmin = false;

        // Fixed test-user identity shared across all "Prueba ..." buttons, so
        // they can run as a connected lifecycle: crear -> login -> desbloquear
        // -> modificar -> cambiar clave -> logout.
        private const int DniPrueba = 90000001;
        private const string NombrePrueba = "Prueba";
        private const string ApellidoPrueba = "Apellido";
        private const string EmailPrueba = "prueba@test.com";
        private static readonly string NombreUsuarioPrueba = NombrePrueba + DniPrueba;
        private static readonly string ClavePrueba = ApellidoPrueba + DniPrueba;

        public FormMenuPrincipal()
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
            this.Text = _loc["FormMenuPrincipal", "Title"];
            usuarioToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuUsuarios"];
            logoutToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuLogout"];
            bitacoraToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuBitacora"];
            cambiarLenguajeToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuCambiarLenguaje"];
            cambiarContrasenaToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuCambiarContrasena"];
            reloginToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuRelogin"];
            familiasToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuFamilias"];
            rolesToolStripMenuItem.Text = _loc["FormMenuPrincipal", "MenuRoles"];
            btnProbarDigito.Text = _loc["FormMenuPrincipal", "ButtonProbarDigito"];
            btnPruebaCrearUsuario.Text = _loc["FormMenuPrincipal", "ButtonPruebaCrearUsuario"];
            btnPruebaLogin.Text = _loc["FormMenuPrincipal", "ButtonPruebaLogin"];
            btnPruebaDesbloquearUsuario.Text = _loc["FormMenuPrincipal", "ButtonPruebaDesbloquearUsuario"];
            btnPruebaModificarUsuario.Text = _loc["FormMenuPrincipal", "ButtonPruebaModificarUsuario"];
            btnPruebaCambiarClave.Text = _loc["FormMenuPrincipal", "ButtonPruebaCambiarClave"];
            btnPruebaLogout.Text = _loc["FormMenuPrincipal", "ButtonPruebaLogout"];
            chkMostrarPruebas.Text = _loc["FormMenuPrincipal", "ChkMostrarPruebas"];
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _loc.Unsubscribe(this);
            base.OnFormClosed(e);
        }

        // ─────────────────────────────────────────
        //  Load: enforce role-based visibility
        // ─────────────────────────────────────────

        private void Form1_Load(object sender, EventArgs e)
        {
            AplicarVisibilidadPermisos();
        }

        // Re-applies menu visibility for whichever user is currently logged in.
        // Must be called not just on first load but also after every relogin/
        // logout-then-login, since FormMenuPrincipal is reused (Hide/Show)
        // across sessions rather than recreated — otherwise a user who logs
        // out and a different user who logs back in would still see the
        // previous user's menu items.
        private void AplicarVisibilidadPermisos()
        {
            Usuario usuarioLogueado = SessionManager.GetInstance.usuario;
            Permisos permisos = SessionManager.GetInstance.Permisos;

            // Defensive fallback: if permissions weren't loaded for some reason,
            // reload them now instead of crashing or silently hiding everything.
            if (permisos == null)
            {
                var rolCompleto = new BLL.RolesBLL().ObtenerRolConPermisos(usuarioLogueado.IdRol);
                permisos = new Permisos(rolCompleto);
                SessionManager.GetInstance.SetPermisos(permisos);
            }

            usuarioToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);
            bitacoraToolStripMenuItem.Visible = permisos.Tiene(Patente.Bitacora);
            cambiarContrasenaToolStripMenuItem.Visible = permisos.Tiene(Patente.CambiarContrasena);
            reloginToolStripMenuItem.Visible = permisos.Tiene(Patente.ReiniciarSesion);
            logoutToolStripMenuItem.Visible = permisos.Tiene(Patente.CerrarSesion);

            // Familias/Roles management is part of the same "Gestor de usuarios" patent family
            // (administering the permission system itself) — gate it the same way.
            familiasToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);
            rolesToolStripMenuItem.Visible = permisos.Tiene(Patente.GestorUsuarios);

            // Integrity-check tool and "Prueba ..." smoke-test buttons: admin-only,
            // and only shown at all when chkMostrarPruebas is checked — non-admins
            // never see the checkbox or any test button, regardless of its state.
            _esAdmin = permisos.Tiene(Patente.GestorUsuarios);
            chkMostrarPruebas.Visible = _esAdmin;
            if (!_esAdmin)
                chkMostrarPruebas.Checked = false;

            ActualizarVisibilidadPruebas();
        }

        // Single chokepoint for the test buttons' Visible state: on whenever
        // the user is an admin AND has the toggle checked, off otherwise.
        private void ActualizarVisibilidadPruebas()
        {
            bool mostrar = _esAdmin && chkMostrarPruebas.Checked;

            btnProbarDigito.Visible = mostrar;
            btnPruebaCrearUsuario.Visible = mostrar;
            btnPruebaLogin.Visible = mostrar;
            btnPruebaDesbloquearUsuario.Visible = mostrar;
            btnPruebaModificarUsuario.Visible = mostrar;
            btnPruebaCambiarClave.Visible = mostrar;
            btnPruebaLogout.Visible = mostrar;
        }

        private void chkMostrarPruebas_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadPruebas();
        }

        // ─────────────────────────────────────────
        //  Menu handlers
        // ─────────────────────────────────────────

        private void usuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormUsuarios().Show();
        }

        private void bitacoraToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            new FormBitacora().Show();
        }

        private void cambiarContrasenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCambiarContrasena().Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmar = MessageBox.Show(
                _loc["FormMenuPrincipal", "LogoutConfirm"],
                _loc["FormMenuPrincipal", "LogoutTitle"],
                MessageBoxButtons.YesNo);

            if (confirmar != DialogResult.Yes) return;

            this.Hide();
            SessionManager.GetInstance.Logout();

            using (var login = new FormLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }
            AplicarVisibilidadPermisos();
            this.Show();
        }

        private void reloginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resp = MessageBox.Show(
                _loc["FormMenuPrincipal", "ReloginConfirm"],
                _loc["FormMenuPrincipal", "ReloginTitle"],
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (resp != DialogResult.Yes) return;

            SessionManager.GetInstance.Logout();
            this.Hide();

            using (var login = new FormLogin())
            {
                if (login.ShowDialog() != DialogResult.OK) return;
            }
            AplicarVisibilidadPermisos();
            this.Show();
        }

        private void cambiarLenguajeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormCambiarLenguaje().Show();
        }

        private void familiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormFamilias().Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormRoles().Show();
        }

        // ─────────────────────────────────────────
        //  Integrity-check tool (DigitoVerificador)
        // ─────────────────────────────────────────

        private void btnProbarDigito_Click(object sender, EventArgs e)
        {
            if (!PedirDni(_loc["FormMenuPrincipal", "PromptDniTitle"],
                          _loc["FormMenuPrincipal", "PromptDniLabel"],
                          _loc["FormMenuPrincipal", "PromptDniOk"],
                          _loc["FormMenuPrincipal", "PromptDniCancel"], out int dni))
                return;

            var simular = MessageBox.Show(
                _loc["FormMenuPrincipal", "SimularAlteracionConfirm"],
                _loc["FormMenuPrincipal", "SimularAlteracionTitle"],
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            var usuarioBLL = new BLL.UsuarioBLL();

            if (simular == DialogResult.Yes)
            {
                try
                {
                    usuarioBLL.SimularAlteracion(dni);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, _loc["FormMenuPrincipal", "TestDigitoAlertaTitle"],
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string detalle;
            bool valido = usuarioBLL.VerificarIntegridad(dni, out detalle);

            MessageBox.Show(detalle,
                valido ? _loc["FormMenuPrincipal", "TestDigitoOkTitle"] : _loc["FormMenuPrincipal", "TestDigitoAlertaTitle"],
                MessageBoxButtons.OK,
                valido ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        // Minimal inline prompt dialog (no separate Designer-backed Form needed)
        // for entering a DNI to test/repair.
        private static bool PedirDni(string titulo, string etiqueta, string textoOk, string textoCancel, out int dni)
        {
            dni = 0;

            using (var prompt = new Form())
            {
                prompt.Text = titulo;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.ClientSize = new System.Drawing.Size(300, 110);
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                var lbl = new Label { Left = 12, Top = 15, Width = 270, Text = etiqueta };
                var txt = new TextBox { Left = 12, Top = 38, Width = 270 };
                var btnOk = new Button { Text = textoOk, Left = 120, Width = 75, Top = 70, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = textoCancel, Left = 200, Width = 80, Top = 70, DialogResult = DialogResult.Cancel };

                prompt.Controls.Add(lbl);
                prompt.Controls.Add(txt);
                prompt.Controls.Add(btnOk);
                prompt.Controls.Add(btnCancel);
                prompt.AcceptButton = btnOk;
                prompt.CancelButton = btnCancel;

                if (prompt.ShowDialog() != DialogResult.OK)
                    return false;

                return int.TryParse(txt.Text.Trim(), out dni);
            }
        }

        // ─────────────────────────────────────────
        //  "Prueba ..." smoke-test buttons
        //
        //  All six exercise the real BLL/SessionManager functions already
        //  used elsewhere in this form, against one fixed test account
        //  (DniPrueba), so they form a connected lifecycle: crear -> login ->
        //  desbloquear -> modificar -> cambiar clave -> logout.
        // ─────────────────────────────────────────

        // Runs accion() without leaving the real admin's session altered —
        // some tests call UsuarioBLL.Login(), which (like the real login flow)
        // overwrites the active SessionManager session on success.
        private static void EjecutarSinAfectarSesion(Action accion)
        {
            Usuario usuarioOriginal = SessionManager.GetInstance.usuario;
            Permisos permisosOriginal = SessionManager.GetInstance.Permisos;
            try
            {
                accion();
            }
            finally
            {
                SessionManager.GetInstance.Login(usuarioOriginal);
                SessionManager.GetInstance.SetPermisos(permisosOriginal);
            }
        }

        private void btnPruebaCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                bool yaExiste = new BLL.UsuarioBLL().ListarUsuarios().Any(u => u.DNI == DniPrueba);
                if (yaExiste)
                {
                    MessageBox.Show(
                        "El usuario de prueba '" + NombreUsuarioPrueba + "' ya existe (de una corrida anterior). " +
                        "Las demás pruebas pueden ejecutarse igual sobre él.",
                        "Prueba: Crear usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var roles = new BLL.RolesBLL().ListarRoles();
                if (roles.Count == 0)
                    throw new Exception("No hay roles creados; cree un rol antes de ejecutar esta prueba.");

                var usuario = new Usuario
                {
                    DNI = DniPrueba,
                    Nombre = NombrePrueba,
                    Apellido = ApellidoPrueba,
                    Email = EmailPrueba,
                    IdRol = roles[0].IdRol
                };

                bool ok = new BLL.UsuarioBLL().CrearUsuario(usuario, SessionManager.GetInstance.usuario.DNI);

                MessageBox.Show(
                    ok ? "PRUEBA OK: usuario de prueba '" + NombreUsuarioPrueba + "' creado."
                       : "PRUEBA FALLIDA: no se pudo crear el usuario de prueba.",
                    "Prueba: Crear usuario", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PRUEBA FALLIDA: " + ex.Message, "Prueba: Crear usuario",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPruebaLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario logueado = null;
                EjecutarSinAfectarSesion(() =>
                {
                    logueado = new BLL.UsuarioBLL().Login(NombreUsuarioPrueba, ClavePrueba);
                });

                MessageBox.Show("PRUEBA OK: login exitoso para '" + logueado.NombreUsuario + "'.",
                    "Prueba: Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PRUEBA FALLIDA: " + ex.Message, "Prueba: Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPruebaDesbloquearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                // Force a lockout: 3 wrong attempts in a row (mirrors the real
                // lockout behaviour exercised by the login form).
                EjecutarSinAfectarSesion(() =>
                {
                    var bll = new BLL.UsuarioBLL();
                    for (int i = 0; i < 3; i++)
                    {
                        try { bll.Login(NombreUsuarioPrueba, "claveincorrecta"); }
                        catch { /* expected: wrong password, account locks on the 3rd try */ }
                    }
                });

                var usuarioBloqueado = new BLL.UsuarioBLL().ListarUsuarios().FirstOrDefault(u => u.DNI == DniPrueba);
                if (usuarioBloqueado == null)
                    throw new Exception("Primero ejecute la prueba de Crear usuario.");
                if (usuarioBloqueado.Estado)
                    throw new Exception("No se logró bloquear la cuenta antes de probar el desbloqueo.");

                bool desbloqueado = new BLL.UsuarioBLL().HabilitarUsuario(DniPrueba);

                bool puedeLoguear = false;
                EjecutarSinAfectarSesion(() =>
                {
                    try
                    {
                        new BLL.UsuarioBLL().Login(NombreUsuarioPrueba, ClavePrueba);
                        puedeLoguear = true;
                    }
                    catch { puedeLoguear = false; }
                });

                bool ok = desbloqueado && puedeLoguear;
                MessageBox.Show(
                    ok ? "PRUEBA OK: la cuenta se bloqueó tras 3 intentos fallidos y se desbloqueó correctamente; el login vuelve a funcionar."
                       : "PRUEBA FALLIDA: no se pudo desbloquear la cuenta o el login posterior falló.",
                    "Prueba: Desbloquear usuario", MessageBoxButtons.OK,
                    ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PRUEBA FALLIDA: " + ex.Message, "Prueba: Desbloquear usuario",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPruebaModificarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                var usuarioActual = new BLL.UsuarioBLL().ListarUsuarios().FirstOrDefault(u => u.DNI == DniPrueba);
                if (usuarioActual == null)
                    throw new Exception("Primero ejecute la prueba de Crear usuario.");

                string nuevoEmail = "modificado" + (DateTime.Now.Ticks % 10000) + "@test.com";

                var usuarioModificado = new Usuario
                {
                    DNI = DniPrueba,
                    Nombre = usuarioActual.Nombre,
                    Apellido = usuarioActual.Apellido,
                    Email = nuevoEmail,
                    IdRol = usuarioActual.IdRol,
                    Lenguaje = usuarioActual.Lenguaje
                };

                bool ok = new BLL.UsuarioBLL().ModificarUsuario(usuarioModificado, DniPrueba);

                var verificacion = new BLL.UsuarioBLL().ListarUsuarios().FirstOrDefault(u => u.DNI == DniPrueba);
                bool persistido = verificacion != null && verificacion.Email == nuevoEmail;

                bool exito = ok && persistido;
                MessageBox.Show(
                    exito ? "PRUEBA OK: usuario modificado; email actualizado a " + nuevoEmail + "."
                          : "PRUEBA FALLIDA: la modificación no se aplicó correctamente.",
                    "Prueba: Modificar usuario", MessageBoxButtons.OK,
                    exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PRUEBA FALLIDA: " + ex.Message, "Prueba: Modificar usuario",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPruebaCambiarClave_Click(object sender, EventArgs e)
        {
            string claveNueva = "ClaveNueva" + DniPrueba;
            try
            {
                bool ok = new BLL.UsuarioBLL().CambiarContrasena(DniPrueba, ClavePrueba, claveNueva, claveNueva);

                bool puedeLoguearConNueva = false;
                EjecutarSinAfectarSesion(() =>
                {
                    try
                    {
                        new BLL.UsuarioBLL().Login(NombreUsuarioPrueba, claveNueva);
                        puedeLoguearConNueva = true;
                    }
                    catch { puedeLoguearConNueva = false; }
                });

                // Revert back to the known test password so the other "Prueba ..."
                // buttons keep working afterward.
                if (puedeLoguearConNueva)
                    new BLL.UsuarioBLL().CambiarContrasena(DniPrueba, claveNueva, ClavePrueba, ClavePrueba);

                bool exito = ok && puedeLoguearConNueva;
                MessageBox.Show(
                    exito ? "PRUEBA OK: la contraseña se cambió y el login con la nueva clave funcionó (luego se revirtió)."
                          : "PRUEBA FALLIDA: el cambio de contraseña no funcionó como se esperaba.",
                    "Prueba: Cambiar clave", MessageBoxButtons.OK,
                    exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PRUEBA FALLIDA: " + ex.Message, "Prueba: Cambiar clave",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reuses the real logout flow verbatim — this *is* the same function
        // wired to the Logout menu item, just triggered from a test button.
        private void btnPruebaLogout_Click(object sender, EventArgs e)
        {
            logoutToolStripMenuItem_Click(sender, e);
        }
    }
}