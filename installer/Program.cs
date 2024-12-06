using System.Security.Principal;

namespace installer;

static class Program {
    private static void AssertAdministrator() {
        try {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            if (principal.IsInRole(WindowsBuiltInRole.Administrator)) {
                return;
            }
        } catch {
            // ignored
        }

        MessageBox.Show("You must run the installer as administrator.", "BB Packager Installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Environment.Exit(-1);
    }
    
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
        AssertAdministrator();
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Installer());
    }
}