# Philiprehberger.MimeDetect

[![CI](https://github.com/philiprehberger/dotnet-mime-detect/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-mime-detect/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.MimeDetect.svg)](https://www.nuget.org/packages/Philiprehberger.MimeDetect)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-mime-detect)](https://github.com/philiprehberger/dotnet-mime-detect/commits/main)

Detect MIME types from file signatures (magic bytes) and file extensions — covers 200+ common file types.

## Installation

```bash
dotnet add package Philiprehberger.MimeDetect
```

## Usage

```csharp
using Philiprehberger.MimeDetect;

byte[] data = File.ReadAllBytes("photo.png");
string? mime = MimeDetect.FromBytes(data);
// "image/png"
```

### Detect from Bytes

```csharp
using Philiprehberger.MimeDetect;

byte[] header = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
string? mime = MimeDetect.FromBytes(header);
// "application/pdf"

MimeResult? result = MimeDetect.DetectFromBytes(header);
// result.MimeType  => "application/pdf"
// result.Extension => "pdf"
// result.Category  => "document"
```

### Detect from Extension

```csharp
using Philiprehberger.MimeDetect;

string? mime = MimeDetect.FromExtension(".mp4");
// "video/mp4"

string? mime2 = MimeDetect.FromExtension("json");
// "application/json"
```

### Detect from File

```csharp
using Philiprehberger.MimeDetect;

string? mime = MimeDetect.FromFile("archive.zip");
// "application/zip"

MimeResult? result = MimeDetect.DetectFromFile("song.mp3");
// result.MimeType  => "audio/mpeg"
// result.Extension => "mp3"
// result.Category  => "audio"
```

### Type-Checking Shortcuts

```csharp
using Philiprehberger.MimeDetect;

bool image = MimeDetect.IsImage("photo.png");    // true
bool video = MimeDetect.IsVideo("clip.mp4");      // true
bool audio = MimeDetect.IsAudio("song.mp3");      // true
bool archive = MimeDetect.IsArchive("backup.zip"); // true

MimeDetect.IsImage("readme.txt"); // false
```

## API

### `MimeDetect`

| Method | Description |
|--------|-------------|
| `FromBytes(ReadOnlySpan<byte> data)` | Detect MIME type from raw bytes via magic byte matching |
| `FromExtension(string extension)` | Look up MIME type by file extension |
| `FromFile(string path)` | Detect MIME type from a file (magic bytes with extension fallback) |
| `DetectFromBytes(ReadOnlySpan<byte> data)` | Detect MIME type from raw bytes, returns full `MimeResult` |
| `DetectFromFile(string path)` | Detect MIME type from a file, returns full `MimeResult` |
| `IsImage(string path)` | Check if a file is an image |
| `IsVideo(string path)` | Check if a file is a video |
| `IsAudio(string path)` | Check if a file is an audio file |
| `IsArchive(string path)` | Check if a file is an archive |

### `MimeResult`

| Property | Type | Description |
|----------|------|-------------|
| `MimeType` | `string` | The MIME type string (e.g., "image/png") |
| `Extension` | `string` | The canonical file extension without a leading dot |
| `Category` | `string` | Broad category: image, video, audio, document, archive, application, text |

## Development

```bash
dotnet build src/Philiprehberger.MimeDetect.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-mime-detect)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-mime-detect/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-mime-detect/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
