# restdir
restdir is a small utility for serving a directory on HTTP, allowing upload to and download from it.

It is useful for modifying contents of hard to reach file systems like Docker volumes.

The interface is plain and simple REST. Upload: `PUT`. Download: `GET`. Delete: `DELETE`

**This is designed as a testing/dev utility and not for production use**

Available as a Docker image [here](https://hub.docker.com/repository/docker/jamoyjamie/restdir)

## Building
```bash
dotnet publish -r win-x64
```
For other runtime monikers see [here](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).

## Usage
```bash
restdir
```
That will serve the current directory on `http://localhost:5000` (note that this only accepts requests from localhost!).

Here are some options if you want to make it more complicated:
|   | Description | Default | Example |
|---|-------------|---------|---------|
| `-d`, `--directory` | The path to the directory to serve | Current directory | `-d www/images` |
| `-p`, `--prefix` | The prefix to serve on (see [here](https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=net-6.0#remarks) for more info) | `http://localhost:5000` | `-p http://my-domain.com:8080/images/` |
| `-r`, `--read-only` | Serves in read only mode | Off | `-r` |
| `-l`, `--log-requests` | Enables request (access) logging | Off | `-l ` |
| `--allow-recursive-delete` | Allows deletion of whole directories and all their content. Yikes! | Off | `--allow-recursive-delete` |

## TODO
- Docker image
- CI/CD
- Integation tests
- HTTPS support
- HEAD/OPTIONS support

## Why not POST?
An age old debate indeed.

`POST` isn't necessarily _wrong_, just `PUT` is a better fit here because you know the file name you're uploading:
```
http://localhost:5000/things/thing-12
```

`POST` is much better when you're uploading something you want a name/ID to be generated for you by the server
```
http://localhost:5000/things/
```
In which case the server should be returning you that generated name/ID in a `Location` header in the response
```
Location: http://localhost:5000/things/thing-34
```
