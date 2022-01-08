using CommandLine;

class Options
{
    [Option('d', "directory", HelpText = "The directory to serve")]
    public string? Directory { get; set; }

    [Option('p', "prefix", Default = "http://localhost:5000/", HelpText = "The prefix to serve on")]
    public string Prefix { get; set; }

    [Option('r', "read-only", HelpText = "Serves in read only mode")]
    public bool ReadOnly { get; set; }

    [Option('l', "log-requests", HelpText = "Enables request (access) logging")]
    public bool LogRequests { get; set; }
}