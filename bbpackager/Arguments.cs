using CommandLine;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace bbpackager;

internal class Arguments {
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }

    [Option('d', "directory", Required = false, HelpText = "Specify the project directory.")]
    public string? ProjectDirectory { get; set; }

    [Option('p', "project", Required = false, HelpText = "Specify the project config file.")]
    public string? ProjectFile { get; set; }

    [Option('d', "debug", Required = false, HelpText = "Compile the project with debugging information.")]
    public bool Debug { get; set; }

    [Option('i', "init", Required = false, HelpText = "Initialize a new empty project.")]
    public bool Init { get; set; }
}