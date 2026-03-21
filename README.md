# Philiprehberger.MimeDetect

[![CI](https://github.com/philiprehberger/dotnet-mime-detect/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-mime-detect/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.MimeDetect.svg)](https://www.nuget.org/packages/Philiprehberger.MimeDetect)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-mime-detect)](LICENSE)

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

## API

### `MimeDetect`

| Method | Description |
|--------|-------------|
| `FromBytes(ReadOnlySpan<byte> data)` | Detect MIME type from raw bytes via magic byte matching |
| `FromExtension(string extension)` | Look up MIME type by file extension |
| `FromFile(string path)` | Detect MIME type from a file (magic bytes with extension fallback) |
| `DetectFromBytes(ReadOnlySpan<byte> data)` | Detect MIME type from raw bytes, returns full `MimeResult` |
| `DetectFromFile(string path)` | Detect MIME type from a file, returns full `MimeResult` |

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

## License

MIT
