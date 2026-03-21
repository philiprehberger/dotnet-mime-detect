namespace Philiprehberger.MimeDetect;

/// <summary>
/// Internal registry of file signatures (magic bytes) used for MIME type detection.
/// </summary>
internal static class MagicBytes
{
    /// <summary>
    /// A known file signature entry.
    /// </summary>
    internal readonly record struct Signature(byte[] Bytes, int Offset, string MimeType, string Extension, string Category);

    /// <summary>
    /// All known file signatures, ordered longest-first for greedy matching.
    /// </summary>
    internal static readonly Signature[] Signatures = GetSignatures();

    private static Signature[] GetSignatures()
    {
        var signatures = new Signature[]
        {
            // Images
            new(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, 0, "image/png", "png", "image"),
            new(new byte[] { 0xFF, 0xD8, 0xFF }, 0, "image/jpeg", "jpg", "image"),
            new(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, 0, "image/gif", "gif", "image"),
            new(new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, 0, "image/gif", "gif", "image"),
            new(new byte[] { 0x42, 0x4D }, 0, "image/bmp", "bmp", "image"),
            new(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, "image/webp", "webp", "image"), // RIFF....WEBP checked below
            new(new byte[] { 0x49, 0x49, 0x2A, 0x00 }, 0, "image/tiff", "tiff", "image"),
            new(new byte[] { 0x4D, 0x4D, 0x00, 0x2A }, 0, "image/tiff", "tiff", "image"),
            new(new byte[] { 0x00, 0x00, 0x01, 0x00 }, 0, "image/x-icon", "ico", "image"),
            new(new byte[] { 0x00, 0x00, 0x02, 0x00 }, 0, "image/x-icon", "ico", "image"),

            // SVG (text-based, detected via XML/svg markers)
            new(new byte[] { 0x3C, 0x73, 0x76, 0x67 }, 0, "image/svg+xml", "svg", "image"), // <svg
            new(new byte[] { 0x3C, 0x3F, 0x78, 0x6D, 0x6C }, 0, "image/svg+xml", "svg", "image"), // <?xml (heuristic)

            // HEIF / AVIF
            new(new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x68, 0x65, 0x69, 0x63 }, 0, "image/heic", "heic", "image"),
            new(new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66 }, 0, "image/avif", "avif", "image"),

            // PDF
            new(new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D }, 0, "application/pdf", "pdf", "document"),

            // ZIP-based formats (check specific types before generic ZIP)
            // DOCX
            new(new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 }, 0, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx", "document"),
            // Generic ZIP
            new(new byte[] { 0x50, 0x4B, 0x03, 0x04 }, 0, "application/zip", "zip", "archive"),
            new(new byte[] { 0x50, 0x4B, 0x05, 0x06 }, 0, "application/zip", "zip", "archive"),
            new(new byte[] { 0x50, 0x4B, 0x07, 0x08 }, 0, "application/zip", "zip", "archive"),

            // GZIP
            new(new byte[] { 0x1F, 0x8B }, 0, "application/gzip", "gz", "archive"),

            // TAR (ustar at offset 257)
            new(new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72 }, 257, "application/x-tar", "tar", "archive"),

            // 7-Zip
            new(new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, 0, "application/x-7z-compressed", "7z", "archive"),

            // RAR
            new(new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00 }, 0, "application/vnd.rar", "rar", "archive"),
            new(new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 }, 0, "application/vnd.rar", "rar", "archive"),

            // Bzip2
            new(new byte[] { 0x42, 0x5A, 0x68 }, 0, "application/x-bzip2", "bz2", "archive"),

            // XZ
            new(new byte[] { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00 }, 0, "application/x-xz", "xz", "archive"),

            // Zstandard
            new(new byte[] { 0x28, 0xB5, 0x2F, 0xFD }, 0, "application/zstd", "zst", "archive"),

            // Audio
            new(new byte[] { 0x49, 0x44, 0x33 }, 0, "audio/mpeg", "mp3", "audio"), // ID3
            new(new byte[] { 0xFF, 0xFB }, 0, "audio/mpeg", "mp3", "audio"),
            new(new byte[] { 0xFF, 0xF3 }, 0, "audio/mpeg", "mp3", "audio"),
            new(new byte[] { 0xFF, 0xF2 }, 0, "audio/mpeg", "mp3", "audio"),
            new(new byte[] { 0x4F, 0x67, 0x67, 0x53 }, 0, "audio/ogg", "ogg", "audio"),
            new(new byte[] { 0x66, 0x4C, 0x61, 0x43 }, 0, "audio/flac", "flac", "audio"),
            new(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, "audio/wav", "wav", "audio"), // RIFF....WAVE checked contextually

            // Video
            new(new byte[] { 0x00, 0x00, 0x00, 0x1C, 0x66, 0x74, 0x79, 0x70 }, 0, "video/mp4", "mp4", "video"),
            new(new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 }, 0, "video/mp4", "mp4", "video"),
            new(new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70 }, 0, "video/mp4", "mp4", "video"),
            new(new byte[] { 0x1A, 0x45, 0xDF, 0xA3 }, 0, "video/webm", "webm", "video"),
            new(new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x71, 0x74 }, 0, "video/quicktime", "mov", "video"),
            new(new byte[] { 0x46, 0x4C, 0x56, 0x01 }, 0, "video/x-flv", "flv", "video"),
            new(new byte[] { 0x52, 0x49, 0x46, 0x46 }, 0, "video/x-msvideo", "avi", "video"), // RIFF....AVI

            // Microsoft Office legacy (OLE2 compound document)
            new(new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }, 0, "application/msword", "doc", "document"),

            // Executables
            new(new byte[] { 0x4D, 0x5A }, 0, "application/vnd.microsoft.portable-executable", "exe", "application"),
            new(new byte[] { 0x7F, 0x45, 0x4C, 0x46 }, 0, "application/x-elf", "elf", "application"),

            // WebAssembly
            new(new byte[] { 0x00, 0x61, 0x73, 0x6D }, 0, "application/wasm", "wasm", "application"),

            // SQLite
            new(new byte[] { 0x53, 0x51, 0x4C, 0x69, 0x74, 0x65, 0x20, 0x66, 0x6F, 0x72, 0x6D, 0x61, 0x74, 0x20, 0x33, 0x00 }, 0, "application/vnd.sqlite3", "sqlite", "application"),

            // Fonts
            new(new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00 }, 0, "font/ttf", "ttf", "application"),
            new(new byte[] { 0x4F, 0x54, 0x54, 0x4F }, 0, "font/otf", "otf", "application"),
            new(new byte[] { 0x77, 0x4F, 0x46, 0x46 }, 0, "font/woff", "woff", "application"),
            new(new byte[] { 0x77, 0x4F, 0x46, 0x32 }, 0, "font/woff2", "woff2", "application"),

            // Java
            new(new byte[] { 0xCA, 0xFE, 0xBA, 0xBE }, 0, "application/java-vm", "class", "application"),

            // Flash (SWF)
            new(new byte[] { 0x46, 0x57, 0x53 }, 0, "application/x-shockwave-flash", "swf", "application"),
            new(new byte[] { 0x43, 0x57, 0x53 }, 0, "application/x-shockwave-flash", "swf", "application"),

            // Postscript
            new(new byte[] { 0x25, 0x21, 0x50, 0x53 }, 0, "application/postscript", "ps", "application"),

            // RTF
            new(new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66 }, 0, "application/rtf", "rtf", "document"),

            // MIDI
            new(new byte[] { 0x4D, 0x54, 0x68, 0x64 }, 0, "audio/midi", "mid", "audio"),

            // Windows Installer
            new(new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }, 0, "application/x-msi", "msi", "application"),

            // Photoshop
            new(new byte[] { 0x38, 0x42, 0x50, 0x53 }, 0, "image/vnd.adobe.photoshop", "psd", "image"),

            // OpenEXR
            new(new byte[] { 0x76, 0x2F, 0x31, 0x01 }, 0, "image/x-exr", "exr", "image"),

            // CAB
            new(new byte[] { 0x4D, 0x53, 0x43, 0x46 }, 0, "application/vnd.ms-cab-compressed", "cab", "archive"),

            // DMG
            new(new byte[] { 0x78, 0x01, 0x73, 0x0D, 0x62, 0x62, 0x60 }, 0, "application/x-apple-diskimage", "dmg", "archive"),

            // AIFF
            new(new byte[] { 0x46, 0x4F, 0x52, 0x4D }, 0, "audio/aiff", "aiff", "audio"),

            // ASF / WMV / WMA
            new(new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 }, 0, "video/x-ms-asf", "asf", "video"),

            // LZH
            new(new byte[] { 0x2D, 0x6C, 0x68 }, 2, "application/x-lzh-compressed", "lzh", "archive"),

            // MPEG Transport Stream
            new(new byte[] { 0x47 }, 0, "video/mp2t", "ts", "video"),
        };

        // Sort by signature length descending for greedy matching
        Array.Sort(signatures, (a, b) => b.Bytes.Length.CompareTo(a.Bytes.Length));
        return signatures;
    }
}
