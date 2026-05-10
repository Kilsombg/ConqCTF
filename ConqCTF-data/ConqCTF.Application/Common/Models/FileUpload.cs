namespace ConqCTF.Application.Common.Models
{
    public class FileUpload
    {
        public string? FileName { get; init; }
        
        public Stream? Content { get; init; }

        public long Size { get; init; }
    }
}
