using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.Json;
using CommandLine;
using Microsoft.Win32;
using Newtonsoft.Json;
using Tomlet;

namespace bbpackager;

public static class Entry {
    private static string ProjectDirectory { get; set; } = null!;
    private static string ProjectFile { get; set; } = null!;
    private static Project Project { get; set; }
    private static bool Debug { get; set; }

    public static async Task<int> Main(params string[] args) {
        Arguments options = null!;
        
        Parser.Default.ParseArguments<Arguments>(args).WithParsed(void (o) => {
            options = o;
        });
        
        Logger.VerboseLogging = options.Verbose;
        ProjectDirectory = options.ProjectDirectory ?? Directory.GetCurrentDirectory();
        ProjectFile = options.ProjectFile ?? "project.toml";

        try {
            HttpClient httpClient = new HttpClient();
            
            string responseString = await httpClient.GetStringAsync("https://api.github.com/repos/hlpdev/BBPackager/releases/latest");
            dynamic? response = JsonConvert.DeserializeObject(responseString);

            if (Assembly.GetExecutingAssembly().GetName().Version!.ToString() != response.tag_name!) {
                Logger.Warn("You are running an outdated version of BBPackager.");
                Logger.Warn($"You are running {Assembly.GetExecutingAssembly().GetName().Version!.ToString()} while the latest version is {response.tag_name}.");
                Logger.Warn("Update by running \"bbpackager --update\"");
            }
        } catch {
            // ignored
        }

        if (options.ShouldUpdate) {
            Process.Start(@"C:\Program Files (x86)\bbpackager\updater.exe");
            Environment.Exit(0);
        }
        
        if (options.DisplayVersion) {
            Logger.Log("BBPackager (C) 2024 HNT8");
            Logger.Log(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
            Environment.Exit(0);
        }
        
        if (options.Init) {
            await InitializeEmptyProject();
            Environment.Exit(0);
        }
            
        Debug = options.Debug;
        
        Logger.Verbose($"Set verbose logging flag to \"{Logger.VerboseLogging}\"");
        Logger.Verbose($"Set project directory to \"{ProjectDirectory}\"");
        Logger.Verbose($"Set project file to \"{ProjectFile}\"");

        try {
            string projectFilePath = Path.Combine(ProjectDirectory, "project.toml");
            Logger.Verbose($"Reading project file from \"{projectFilePath}\"");
            
            string projectFile = await File.ReadAllTextAsync(projectFilePath);
            Project = TomletMain.To<Project>(projectFile);
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("Failed to load project configuration file!");
        }
        
        Logger.Verbose($"Set blitz compiler directory to \"{Project.BlitzDirectory}\"");
        Logger.Verbose($"Set build directory to \"{Project.BuildDirectory}\"");
        Logger.Verbose($"Set source directory to \"{Project.SourceDirectory}\"");
        Logger.Verbose($"Set debug flag to \"{Debug}\"");
        
        try {
            Logger.Log("Copying additional libraries to blitz userlibs directory...");
            string[] additionalLibraries =
                Directory.GetFiles(Path.Combine(ProjectDirectory, Project.LibrariesDirectory), "*.decls");

            foreach (string decls in additionalLibraries) {
                File.Copy(decls, Path.Combine(ProjectDirectory, Project.BlitzDirectory, "userlibs", Path.GetFileName(decls)), true);
                Logger.Verbose(
                    $"Copied {decls} to {Path.Combine(ProjectDirectory, Project.BlitzDirectory, "userlibs", Path.GetFileName(decls))}");
            }
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("Failed to copy additional libraries to output directory!");
        }

        try {
            Process compilerProcess = new Process {
                StartInfo = {
                    FileName = Path.Combine(ProjectDirectory, Project.BlitzDirectory, "bin", "blitzcc.exe"),
                    WorkingDirectory = Path.Combine(ProjectDirectory, Project.BlitzDirectory),
                    CreateNoWindow = true,
                    Arguments =
                        $"{(Debug ? "-d " : "")}-o \"{Path.Combine(ProjectDirectory, Project.BuildDirectory, Project.BinaryName)}\" \"{ProjectDirectory}\\{Project.SourceDirectory}\\{Project.EntryPoint}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    EnvironmentVariables = {
                        ["blitzpath"] = Path.Combine(ProjectDirectory, Project.BlitzDirectory)
                    }
                },
                EnableRaisingEvents = true
            };

            Logger.Verbose($"Added \"blitzpath\" environment variable with value \"{compilerProcess.StartInfo.EnvironmentVariables["blitzpath"]}\"");
            
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, Project.BuildDirectory));
            Logger.Verbose("Created build directory");

            Logger.Verbose($"Running compiler with arguments \"{compilerProcess.StartInfo.Arguments}\"");
            bool startResult = compilerProcess.Start();
            
            if (!startResult) {
                Logger.Fatal("Failed to start compiler process!");
                return 1;
            }

            Logger.Verbose("Waiting for compiler to exit...");
            await compilerProcess.WaitForExitAsync();

            if (compilerProcess.ExitCode != 0) {
                Logger.Fatal("Compilation failed!");
                return 1;
            }

            Logger.Log("Compilation finished!");
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("An unknown error occured during compilation!");
        }

        try {
            Logger.Log("Copying runtime libraries to output directory...");
            string[] blitzDlls = Directory
                .GetFiles(Path.Combine(ProjectDirectory, Project.BlitzDirectory, "bin"), "*.dll")
                .Where(f => !f.EndsWith("linker.dll") && !f.EndsWith("runtime.dll") && !f.EndsWith("debugger.dll"))
                .ToArray();

            foreach (string dll in blitzDlls) {
                File.Copy(dll, Path.Combine(ProjectDirectory, Project.BuildDirectory, Path.GetFileName(dll)), true);
                Logger.Verbose(
                    $"Copied {dll} to {Path.Combine(ProjectDirectory, Project.BuildDirectory, Path.GetFileName(dll))}");
            }
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("Failed to copy runtime libraries!");
        }

        try {
            Logger.Log("Copying assets to output directory...");
            DirectoryCopy(Path.Combine(ProjectDirectory, Project.AssetsDirectory), Project.BuildDirectory, true);
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("Failed to copy assets to output directory!");
        }

        try {
            Logger.Log("Copying additional libraries to output directory...");
            string[] additionalLibraries =
                Directory.GetFiles(Path.Combine(ProjectDirectory, Project.LibrariesDirectory), "*.dll");

            foreach (string dll in additionalLibraries) {
                File.Copy(dll, Path.Combine(ProjectDirectory, Project.BuildDirectory, Path.GetFileName(dll)), true);
                Logger.Verbose(
                    $"Copied {dll} to {Path.Combine(ProjectDirectory, Project.BuildDirectory, Path.GetFileName(dll))}");
            }
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("Failed to copy additional libraries to output directory!");
        }

        Logger.Verbose("Overwriting executable resource information...");

#pragma warning disable CA1416
        string installPath = (Registry.LocalMachine.OpenSubKey("Software")!.OpenSubKey("bbpackager")!.GetValue("InstallPath") as string)!;
#pragma warning restore CA1416

        Process.Start(Path.Combine(installPath, "rcedit-x86.exe"), [
            $"\"{Path.Combine(ProjectDirectory, Project.BuildDirectory, Project.BinaryName)}\"",
            "--set-file-version", Project.Version,
            "--set-product-version", $"\"{Project.Version} {Project.Designation}\"",
            "--set-version-string", $"\"LegalCopyright\" \"{Project.Copyright}\"",
            "--set-version-string", $"\"ProductName\" \"{Project.Title}\"",
            "--set-version-string", $"\"Company\" \"{Project.Company}\"",
        ]);

        Logger.Log("Successfully built project!");
        
        return 0;
    }
    
    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) {
        DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);

        if (!directoryInfo.Exists) {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = directoryInfo.GetDirectories();
        if (!Directory.Exists(destDirName)) {
            Directory.CreateDirectory(destDirName);
        }

        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo file in files) {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true);
        }

        if (!copySubDirs) return;
        
        foreach (DirectoryInfo subDirectory in dirs) {
            string tempPath = Path.Combine(destDirName, subDirectory.Name);
            DirectoryCopy(subDirectory.FullName, tempPath, copySubDirs);
        }
    }

    private static async Task InitializeEmptyProject() {
        try {
            // Create project file
            const string defaultToml =
                "[about]\ntitle = \"New Project\"\ndesignation = \"stable\"\nversion = \"1.0.0.0\"\ncompany = \"no company\"\ncopyright = \"none\"\n\n[blitz]\nblitz_dir = \"blitz\"\nsource_dir = \"src\"\nentry = \"entry.bb\"\n\n[build]\noutput_dir = \"build\"\nassets_dir = \"assets\"\nlibraries_dir = \"libraries\"\nicon = \"icon.ico\"\nbinary = \"binary.exe\"";
            
            await File.WriteAllTextAsync(Path.Combine(ProjectDirectory, "project.toml"), defaultToml);

            // Create directories
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, "assets"));
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, "libraries"));
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, "src"));
            Directory.CreateDirectory(Path.Combine(ProjectDirectory, "blitz"));
            
            // Create readme for assets
            await File.WriteAllTextAsync(Path.Combine(ProjectDirectory, "assets", "readme.txt"),
                "Place any files in this directory for them to be\ncopied to the build directory when the project is built.",
                Encoding.UTF8);
            
            // Create readme for libraries
            await File.WriteAllTextAsync(Path.Combine(ProjectDirectory, "libraries", "readme.txt"),
                "Place additional *.dll files and *.decls files to be included into the final build.",
                Encoding.UTF8);
            
            // Create readme for src
            await File.WriteAllTextAsync(Path.Combine(ProjectDirectory, "src", "readme.txt"),
                "This directory should contain all of your *.bb source code files.",
                Encoding.UTF8);
            
            // Download blitz tss as the default Blitz3D version
            Stream release =
                await new HttpClient().GetStreamAsync(
                    "https://github.com/ZiYueCommentary/Blitz3D/releases/latest/download/English.zip");
            
            new ZipArchive(release, ZipArchiveMode.Read).ExtractToDirectory(Path.Combine(ProjectDirectory, "blitz"));
            
            // Create entry file
            await File.WriteAllTextAsync(Path.Combine(ProjectDirectory, "src", "entry.bb"), "While True\n    Print(\"Hello world!\")\n    Delay(1000)\nWend", new UTF8Encoding());
            
            Logger.Log("Successfully initialized empty project.");
        } catch (Exception e) {
            Logger.Error(e.Message);
            Logger.Error(e.StackTrace ?? string.Empty);
            Logger.Fatal("An error occured while initializing an empty project.");
        }
    }
}