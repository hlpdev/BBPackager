namespace bbpackager;

internal static class Logger {

    public static bool VerboseLogging { get; set; } = false;
    
    public static void Verbose(string message) {
        if (!VerboseLogging) return;
        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("VERBOSE: " + message);
        Console.ResetColor();
    }
    
    public static void Log(string message) => Console.WriteLine(message);

    public static void Warn(string message) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("WARNING: " + message);
        Console.ResetColor();
    }

    public static void Error(string message) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("ERROR: " + message);
        Console.ResetColor();
    }

    public static void Fatal(string message) {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("FATAL ERROR: " + message);
        Console.ResetColor();
        Environment.Exit(1);
    }
    
}