using Tomlet.Attributes;

namespace bbpackager;

internal struct Project {
#pragma warning disable CS8618, CS9264
    // ReSharper disable once UnusedMember.Global
    public Project() { }
#pragma warning restore CS8618, CS9264

    // About Category
    [TomlProperty("about.title")] public string Title { get; set; }
    [TomlProperty("about.designation")] public string Designation { get; set; }
    [TomlProperty("about.version")] public string Version { get; set; }
    [TomlProperty("about.company")] public string Company { get; set; }
    [TomlProperty("about.copyright")] public string Copyright { get; set; }
    
    // Blitz Category
    [TomlProperty("blitz.blitz_dir")] public string BlitzDirectory { get; set; }
    [TomlProperty("blitz.source_dir")] public string SourceDirectory { get; set; }
    [TomlProperty("blitz.entry")] public string EntryPoint { get; set; }
    
    // Build Category
    [TomlProperty("build.output_dir")] public string BuildDirectory { get; set; }
    [TomlProperty("build.assets_dir")] public string AssetsDirectory { get; set; }
    [TomlProperty("build.libraries_dir")] public string LibrariesDirectory { get; set; }
    
    [TomlProperty("build.icon")] public string Icon { get; set; }
    [TomlProperty("build.binary")] public string BinaryName { get; set; }
}