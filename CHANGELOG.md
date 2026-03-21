# Changelog

## 0.1.0 (2026-03-21)

- Initial release
- Detect MIME types from file signatures (magic bytes) covering 50+ common formats
- Extension-based lookup for 200+ file extensions
- Full detection via `DetectFromFile` with magic bytes and extension fallback
- `MimeResult` record with MIME type, extension, and category metadata
