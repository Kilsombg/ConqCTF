using ConqCTF.Application.Common.Exceptions;
using ConqCTF.Application.Common.Models;

namespace ConqCTF.Infrastructure.Security
{
    public static class FileUploadValidator
    {
        public const long MaxFileSizeBytes = 100L * 1024 * 1024; // 100 MB

        // Extension allowlist — all lowercased
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            // Archives — common CTF challenge distribution format
            ".zip", ".tar", ".gz", ".7z",
            // Documents / data
            ".txt", ".pdf", ".md", ".csv", ".json", ".xml",
            // Images — steganography challenges
            ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".tiff", ".webp",
            // Audio / video — multimedia forensics
            ".mp4", ".mp3", ".wav", ".avi", ".pcap", ".pcapng",
            // Binary / reverse engineering
            ".bin", ".exe", ".elf", ".so", ".out", ".hex",
            // Source / scripts
            ".py", ".c", ".cpp", ".h", ".java", ".js", ".ts", ".sh",
            // Disk / memory forensics
            ".img", ".iso", ".vmdk", ".mem",
        };

        // Magic bytes table — (offset, signature bytes, description)
        // Extension is checked first; magic bytes provide a second layer for common types
        private static readonly List<(int Offset, byte[] Magic, string[] Extensions)> MagicSignatures = new()
        {
            (0, new byte[] { 0x50, 0x4B, 0x03, 0x04 }, new[] { ".zip" }),           // ZIP / jar / docx
            (0, new byte[] { 0x50, 0x4B, 0x05, 0x06 }, new[] { ".zip" }),           // ZIP empty
            (0, new byte[] { 0x50, 0x4B, 0x07, 0x08 }, new[] { ".zip" }),           // ZIP spanned
            (0, new byte[] { 0x1F, 0x8B },             new[] { ".gz", ".tar" }),    // gzip
            (0, new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, new[] { ".7z" }),
            (0, new byte[] { 0x25, 0x50, 0x44, 0x46 }, new[] { ".pdf" }),           // %PDF
            (0, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, new[] { ".png" }),
            (0, new byte[] { 0xFF, 0xD8, 0xFF },        new[] { ".jpg", ".jpeg" }), // JPEG
            (0, new byte[] { 0x47, 0x49, 0x46, 0x38 }, new[] { ".gif" }),           // GIF8
            (0, new byte[] { 0x42, 0x4D },              new[] { ".bmp" }),           // BM
            (0, new byte[] { 0x49, 0x49, 0x2A, 0x00 }, new[] { ".tiff" }),          // TIFF LE
            (0, new byte[] { 0x4D, 0x4D, 0x00, 0x2A }, new[] { ".tiff" }),          // TIFF BE
            (0, new byte[] { 0x00, 0x00, 0x00, 0x00 }, new[] { ".mp4" }),           // ftyp (checked via offset 4)
            (0, new byte[] { 0xD4, 0xC3, 0xB2, 0xA1 }, new[] { ".pcap" }),         // pcap LE
            (0, new byte[] { 0xA1, 0xB2, 0xC3, 0xD4 }, new[] { ".pcap" }),         // pcap BE
            (0, new byte[] { 0x0A, 0x0D, 0x0D, 0x0A }, new[] { ".pcapng" }),       // pcapng
            (0, new byte[] { 0x4D, 0x5A },              new[] { ".exe" }),           // MZ — Windows PE
            (0, new byte[] { 0x7F, 0x45, 0x4C, 0x46 }, new[] { ".elf", ".so", ".out" }), // ELF
            (0, new byte[] { 0xCD, 0x23, 0x01, 0x00 }, new[] { ".vmdk" }),
            (0, new byte[] { 0x43, 0x44, 0x30, 0x30, 0x31 }, new[] { ".iso" }),    // CD001
        };

        public static async Task ValidateAsync(FileUpload file)
        {
            if (file.Content is null || string.IsNullOrWhiteSpace(file.FileName))
                throw new InvalidFileException("File is missing or has no name.");

            // 1. Size check — done before reading any bytes
            if (file.Size > MaxFileSizeBytes)
                throw new InvalidFileException(
                    $"File '{file.FileName}' exceeds the 100 MB limit " +
                    $"({file.Size / (1024 * 1024)} MB).");

            // 2. Extension allowlist
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
                throw new InvalidFileException(
                    $"File type '{ext}' is not allowed. " +
                    $"Permitted extensions: {string.Join(", ", AllowedExtensions.Order())}");

            // 3. Magic bytes — read the first 16 bytes, rewind, then check
            var header = new byte[16];
            var bytesRead = await file.Content.ReadAsync(header, 0, header.Length);
            file.Content.Seek(0, SeekOrigin.Begin);

            // Text-based and script files have no reliable magic bytes — skip magic check for them
            var textOrScriptExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".txt", ".md", ".csv", ".json", ".xml",
                ".py", ".c", ".cpp", ".h", ".java", ".js", ".ts", ".sh", ".hex"
            };

            if (!textOrScriptExtensions.Contains(ext))
            {
                var matched = false;

                foreach (var (offset, magic, exts) in MagicSignatures)
                {
                    if (!exts.Contains(ext, StringComparer.OrdinalIgnoreCase))
                        continue;

                    if (bytesRead < offset + magic.Length)
                        continue;

                    if (header.Skip(offset).Take(magic.Length).SequenceEqual(magic))
                    {
                        matched = true;
                        break;
                    }
                }

                // Binary/image types that are in the magic table but didn't match → reject
                var magicCheckedExtensions = MagicSignatures
                    .SelectMany(s => s.Extensions)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (magicCheckedExtensions.Contains(ext) && !matched)
                    throw new InvalidFileException(
                        $"File '{file.FileName}' content does not match its extension. " +
                        "The file may be corrupted or misnamed.");
            }
        }
    }
}