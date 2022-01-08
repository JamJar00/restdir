using CommandLine;

class Options
{
    [Option('d', HelpText = "The directory to serve")]
    public string? Directory { get; set; }

    [Option('p', Default = "http://localhost:5000/", HelpText = "The prefix to serve on")]
    public string Prefix { get; set; }

    [Option('v', HelpText = "Enables verbose logging")]
    public bool Verbose { get; set; }

    [Option('r', HelpText = "Enables request logging")]
    public bool RequestLogging { get; set; }
}