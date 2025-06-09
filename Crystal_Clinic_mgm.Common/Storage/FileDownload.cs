namespace Crystal_Clinic_Mgm.Common.Storage
{
    public class JsonModel
    {
        public MemoryStream? memory { get; set; }
        public string? path { get; set; } = string.Empty;
        public string? ContentType { get; set; } = string.Empty;
    }
    public class FileDownloader
    {
        public static async Task<JsonModel> CreateAsync(string filePath)
        {
            if (filePath != null)
            {
                var path = Path.Combine(filePath);
                var memory1 = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory1);
                }
                memory1.Position = 0;
                var types = GetMimeTypes();
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return new JsonModel
                {
                    memory = memory1,
                    path = path,
                    ContentType = types[ext]
                };
            }
            else
            {
                return new JsonModel
                {
                    memory = null,
                    path = null,
                    ContentType = null
                };
            }
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
