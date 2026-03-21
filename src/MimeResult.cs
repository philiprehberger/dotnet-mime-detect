namespace Philiprehberger.MimeDetect;

/// <summary>
/// Represents a detected MIME type with its associated metadata.
/// </summary>
/// <param name="MimeType">The MIME type string (e.g., "image/png").</param>
/// <param name="Extension">The canonical file extension without a leading dot (e.g., "png").</param>
/// <param name="Category">The broad category of the file type (image, video, audio, document, archive, application, text).</param>
public record MimeResult(string MimeType, string Extension, string Category);
