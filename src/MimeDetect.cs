namespace Philiprehberger.MimeDetect;

/// <summary>
/// Detects MIME types from file signatures (magic bytes) and file extensions.
/// </summary>
public static class MimeDetect
{
    /// <summary>
    /// Detects the MIME type from raw bytes by matching known file signatures.
    /// </summary>
    /// <param name="data">The file data to inspect. Only the first several hundred bytes are needed.</param>
    /// <returns>The MIME type string if a match is found; otherwise, <c>null</c>.</returns>
    public static string? FromBytes(ReadOnlySpan<byte> data)
    {
        var result = DetectFromBytes(data);
        return result?.MimeType;
    }

    /// <summary>
    /// Looks up the MIME type for a file extension.
    /// </summary>
    /// <param name="extension">
    /// The file extension to look up. May include a leading dot (e.g., ".png" or "png").
    /// </param>
    /// <returns>The MIME type string if the extension is known; otherwise, <c>null</c>.</returns>
    public static string? FromExtension(string extension)
    {
        ArgumentNullException.ThrowIfNull(extension);

        var ext = extension.TrimStart('.');

        if (ext.Length == 0)
        {
            return null;
        }

        return ExtensionMap.Map.TryGetValue(ext, out var mime) ? mime : null;
    }

    /// <summary>
    /// Detects the MIME type of a file by reading its first bytes and falling back to the file extension.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>The MIME type string if detected; otherwise, <c>null</c>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    public static string? FromFile(string path)
    {
        var result = DetectFromFile(path);
        return result?.MimeType;
    }

    /// <summary>
    /// Detects the MIME type from raw bytes and returns a full <see cref="MimeResult"/> with metadata.
    /// </summary>
    /// <param name="data">The file data to inspect.</param>
    /// <returns>A <see cref="MimeResult"/> if a match is found; otherwise, <c>null</c>.</returns>
    public static MimeResult? DetectFromBytes(ReadOnlySpan<byte> data)
    {
        if (data.IsEmpty)
        {
            return null;
        }

        foreach (var sig in MagicBytes.Signatures)
        {
            if (data.Length < sig.Offset + sig.Bytes.Length)
            {
                continue;
            }

            var slice = data.Slice(sig.Offset, sig.Bytes.Length);

            if (slice.SequenceEqual(sig.Bytes))
            {
                return new MimeResult(sig.MimeType, sig.Extension, sig.Category);
            }
        }

        return null;
    }

    /// <summary>
    /// Detects the MIME type of a file by reading its signature bytes and falling back to the file extension.
    /// Returns a full <see cref="MimeResult"/> with metadata.
    /// </summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>A <see cref="MimeResult"/> if detected; otherwise, <c>null</c>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    public static MimeResult? DetectFromFile(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The specified file was not found.", path);
        }

        // Read enough bytes to cover all signatures (including TAR at offset 257 + 5 bytes)
        var buffer = new byte[300];
        int bytesRead;

        using (var stream = File.OpenRead(path))
        {
            bytesRead = stream.Read(buffer, 0, buffer.Length);
        }

        var result = DetectFromBytes(buffer.AsSpan(0, bytesRead));

        if (result is not null)
        {
            return result;
        }

        // Fall back to extension-based detection
        var ext = Path.GetExtension(path);

        if (string.IsNullOrEmpty(ext))
        {
            return null;
        }

        var mime = FromExtension(ext);

        if (mime is null)
        {
            return null;
        }

        var extNoDot = ext.TrimStart('.');

        var category = InferCategory(mime);

        return new MimeResult(mime, extNoDot, category);
    }

    private static string InferCategory(string mimeType)
    {
        var type = mimeType.Split('/')[0];

        return type switch
        {
            "image" => "image",
            "video" => "video",
            "audio" => "audio",
            "text" => "text",
            "font" => "application",
            _ => mimeType switch
            {
                _ when mimeType.Contains("document") || mimeType.Contains("pdf") ||
                       mimeType.Contains("rtf") || mimeType.Contains("epub") ||
                       mimeType.Contains("msword") || mimeType.Contains("powerpoint") ||
                       mimeType.Contains("excel") || mimeType.Contains("spreadsheet") ||
                       mimeType.Contains("presentation") || mimeType.Contains("opendocument") => "document",
                _ when mimeType.Contains("zip") || mimeType.Contains("compress") ||
                       mimeType.Contains("tar") || mimeType.Contains("rar") ||
                       mimeType.Contains("7z") || mimeType.Contains("gzip") ||
                       mimeType.Contains("bzip") || mimeType.Contains("xz") ||
                       mimeType.Contains("zstd") || mimeType.Contains("lz") ||
                       mimeType.Contains("archive") || mimeType.Contains("cab") ||
                       mimeType.Contains("iso") || mimeType.Contains("dmg") ||
                       mimeType.Contains("cpio") => "archive",
                _ => "application",
            },
        };
    }
}
