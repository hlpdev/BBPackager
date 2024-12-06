using System.IO.Compression;
using Microsoft.Win32;

namespace updater;

public static class Program {
    public static async Task Main() {
        DateTime start = DateTime.Now;
        
        Log("Downloading bbpackager binaries...");
        
        HttpClient httpClient = new HttpClient();
        Stream bbPackagerStream =
            await httpClient.GetStreamAsync(
                "https://github.com/hlpdev/bbpackager/releases/latest/download/installer-contents.zip");

        Log("Downloaded bbpackager binaries.");

        Log("Creating install directory...");

        string[] files = Directory.GetFiles(@"C:\Program Files (x86)\bbpackager\").Where(f => f != "updater.exe")
            .ToArray();

        foreach (string file in files) {
            Directory.Delete(file, false);
        }

        Log("Created install directory.");
        
        Log("Extracting to install directory...");
        
        var bbPackagerArchive = new ZipArchive(bbPackagerStream);
        bbPackagerArchive.ExtractToDirectory(@"C:\Program Files (x86)\bbpackager");

        Log("Extracted to install directory.");

        Log("Downloading rcedit dependency...");
        Stream rceditStream =
            await httpClient.GetStreamAsync("https://github.com/electron/rcedit/releases/latest/download/rcedit-x86.exe");
        FileStream rceditFile = File.Create(@"C:\Program Files (x86)\bbpackager\rcedit-x86.exe");
        rceditFile.Seek(0, SeekOrigin.Begin);
        await rceditStream.CopyToAsync(rceditFile);
        rceditFile.Close();
        Log("Downloaded rcedit dependency.");

        await Task.Delay(500);
        
        Log("Setting environment variables...");

        string existingValue = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine)!;
        existingValue += @";C:\Program Files (x86)\bbpackager";
        Environment.SetEnvironmentVariable("PATH", existingValue, EnvironmentVariableTarget.Machine);

        Log("Set environment variable.");

        Log("Finalizing registry information...");
        RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true)!;
        key = key.CreateSubKey("bbpackager");
        key.SetValue("InstallPath", @"C:\Program Files (x86)\bbpackager");
        key.SetValue("Version", await File.ReadAllTextAsync(@"C:\Program Files (x86)\bbpackager\version.txt"));

        Log("Update completed.");

        DateTime end = DateTime.Now;

        Log($"Update took {new TimeSpan(0, 0, (int)(end - start).TotalSeconds).TotalSeconds} seconds.");
    }

    private static void Log(string text) {
        Console.WriteLine(text);
    }
}