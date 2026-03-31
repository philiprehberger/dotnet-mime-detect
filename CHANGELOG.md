# Changelog

## 0.1.1 (2026-03-31)

- Standardize README to 3-badge format with emoji Support section
- Update CI actions to v5 for Node.js 24 compatibility
- Add GitHub issue templates, dependabot config, and PR template

## 0.1.0 (2026-03-21)

- Initial release
- Detect MIME types from file signatures (magic bytes) covering 50+ common formats
- Extension-based lookup for 200+ file extensions
- Full detection via `DetectFromFile` with magic bytes and extension fallback
- `MimeResult` record with MIME type, extension, and category metadata
