using System.IO.Compression;
using Microsoft.Win32;

namespace installer;

public partial class Installer : Form {
    public Installer() {
        InitializeComponent();
    }

    private async Task SwitchView(int view) {
        await InvokeAsync(() => {
            ViewController.SelectTab(view);
        });
    }

    private async Task UpdateProgressBar(int value) {
        await InvokeAsync(() => {
            InstallProgressBar.Value = value;
        });
    }

    private async Task Log(string message) {
        await InvokeAsync(() => {
            List<string> lines = [
                message
            ];
            lines.AddRange(InstallLogs.Lines);
            InstallLogs.Lines = lines.ToArray();
        });
    }

    private void CancelButton_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void InstallButton_Click(object sender, EventArgs e) {
        _ = SwitchView(1);
        Task.Run(async () => await SwitchView(1));
        Task.Run(async () => await InstallProcess());
    }

    private async Task InstallProcess() {
        await Log("Downloading bbpackager binaries...");
        
        await Task.Delay(500);
        
        await UpdateProgressBar(1);
        HttpClient httpClient = new HttpClient();
        Stream bbPackagerStream =
            await httpClient.GetStreamAsync(
                "https://github.com/hlpdev/bbpackager/releases/latest/download/installer-contents.zip");
        await UpdateProgressBar(55);
        await Log("Downloaded bbpackager binaries.");

        await Log("Creating install directory...");
        
        await Task.Delay(500);

        if (Directory.Exists(@"C:\Program Files (x86)\bbpackager")) {
            Directory.Delete(@"C:\Program Files (x86)\bbpackager");
        }
        
        Directory.CreateDirectory(@"C:\Program Files (x86)\bbpackager");
        await UpdateProgressBar(65);
        await Log("Created install directory.");
        
        await Log("Extracting to install directory...");

        await Task.Delay(500);
        
        var bbPackagerArchive = new ZipArchive(bbPackagerStream);
        bbPackagerArchive.ExtractToDirectory(@"C:\Program Files (x86)\bbpackager");
        await UpdateProgressBar(85);
        await Log("Extracted to install directory.");

        await Task.Delay(500);

        await Log("Downloading rcedit dependency...");
        Stream rceditStream =
            await httpClient.GetStreamAsync("https://github.com/electron/rcedit/releases/latest/download/rcedit-x86.exe");
        FileStream rceditFile = File.Create(@"C:\Program Files (x86)\bbpackager\rcedit-x86.exe");
        rceditFile.Seek(0, SeekOrigin.Begin);
        await rceditStream.CopyToAsync(rceditFile);
        rceditFile.Close();
        await Log("Downloaded rcedit dependency.");
        await UpdateProgressBar(95);

        await Task.Delay(500);
        
        if (EnvVarsCheckbox.Checked) {
            await Log("Setting environment variables...");

            string existingValue = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine)!;
            existingValue += @";C:\Program Files (x86)\bbpackager";
            Environment.SetEnvironmentVariable("PATH", existingValue, EnvironmentVariableTarget.Machine);

            await Log("Set environment variable.");
        }

        await UpdateProgressBar(98);

        await Log("Finalizing registry information...");
        RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true)!;
        key = key.CreateSubKey("bbpackager");
        key.SetValue("InstallPath", @"C:\Program Files (x86)\bbpackager");
        key.SetValue("Version", await File.ReadAllTextAsync(@"C:\Program Files (x86)\bbpackager\version.txt"));

        await Log("Installation completed.");
        await UpdateProgressBar(100);
        
        await Task.Delay(2000);
        
        await SwitchView(2);
    }
}