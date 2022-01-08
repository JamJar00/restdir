﻿using System.Net;
using CommandLine;

Parser.Default.ParseArguments<Options>(args).WithParsed(Serve);

static void Serve(Options options)
{
    if (options.Directory == null)
        options.Directory = Directory.GetCurrentDirectory();

    using HttpListener listener = new();
    listener.Prefixes.Add(options.Prefix);
    listener.Start();
    Console.WriteLine($"Serving {options.Directory} on {options.Prefix}.");

    while (true)
    {
        HttpListenerContext ctx = listener.GetContext();

        HttpListenerRequest req = ctx.Request;
        using HttpListenerResponse resp = ctx.Response;

        string path = Path.Combine(options.Directory, req.Url.AbsolutePath[1..]);

        int statusCode;
        if (IsWithin(path, options.Directory))
        {
            switch (req.HttpMethod)
            {
                case "GET":
                {
                    if (File.Exists(path))
                    {
                        statusCode = resp.StatusCode = 200;
                        using FileStream stream = new(path, FileMode.Open);
                        stream.CopyTo(resp.OutputStream);
                    }
                    else
                    {
                        statusCode = resp.StatusCode = 404;
                    }

                    break;
                }

                case "POST":
                {
                    if (!options.ReadOnly)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            if (!File.Exists(path))
                            {
                                statusCode = resp.StatusCode = 201;
                                using FileStream stream = new(path, FileMode.CreateNew);
                                req.InputStream.CopyTo(stream);
                            }
                            else
                            {
                                statusCode = resp.StatusCode = 400;
                            }
                        }
                        else
                        {
                            statusCode = resp.StatusCode = 404;
                        }
                    }
                    else
                    {
                        statusCode = resp.StatusCode = 405;
                    }

                    break;
                }

                case "PUT":
                {
                    if (!options.ReadOnly)
                    {
                        if (Directory.Exists(Path.GetDirectoryName(path)))
                        {
                            if (File.Exists(path))
                                statusCode = resp.StatusCode = 204;
                            else
                                statusCode = resp.StatusCode = 201;
                            using FileStream stream = new(path, FileMode.Create);
                            req.InputStream.CopyTo(stream);
                        }
                        else
                        {
                            statusCode = resp.StatusCode = 404;
                        }
                    }
                    else
                    {
                        statusCode = resp.StatusCode = 405;
                    }

                    break;
                }

                case "DELETE":
                {
                    if (!options.ReadOnly)
                    {
                        if (File.Exists(path))
                        {
                            statusCode = resp.StatusCode = 204;
                            File.Delete(path);
                        }
                        else
                        {
                            statusCode = resp.StatusCode = 404;
                        }
                    }
                    else
                    {
                        statusCode = resp.StatusCode = 405;
                    }

                    break;
                }
                
                default:
                {
                    statusCode = resp.StatusCode = 405;
                    break;
                }
            }
        }
        else
        {
            statusCode = resp.StatusCode = 400;
        }
        
        if (options.RequestLogging)
            Console.WriteLine($"Request {req.HttpMethod} {req.Url} \"{req.UserAgent}\" || Response {statusCode}");
    }    
}

static bool IsWithin(string dir, string outerDir)
{
    DirectoryInfo dirInfo = new(dir);
    DirectoryInfo outerDirInfo = new(outerDir);
    while (dirInfo.Parent != null)
    {
        if (dirInfo.Parent.FullName == outerDirInfo.FullName)
            return true;
        else
            dirInfo = dirInfo.Parent;
    }

    return false;
}