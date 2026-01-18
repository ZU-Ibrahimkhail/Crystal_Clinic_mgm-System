using ImageMagick;
using Microsoft.AspNetCore.Http;
using System.Globalization;



namespace Crystal_Clinic_Mgm.Common.Storage
{
    public class FileHandler
    {
        private readonly List<string> ImageExtentions = new()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp"
        };
        /// <summary>
        /// Save the uploaded file to the given path in the given base folder
        /// </summary>
        /// <param name="file">Is the uploaded file</param>
        /// <param name="fileextension">The Extension of the file</param>
        /// <param name="fileBaseFolder">First/Base Directory to Save the file (e.g.: wwwroot)</param>
        /// <param name="path">The path after base folder (e.g.: users/profile/)</param>
        /// <returns>The full path with ecrypted name of the file (without file base folder)</returns>
        public async Task<string> CreateAsync(Stream file, string fileextension, string fileBaseFolder, string path, string? AdditionalName = null)
        {
            try
            {

                var additional = DateTime.Now.ToString("/yyyy/MMM", CultureInfo.InvariantCulture) + "/";
                var filename = GenerateFileName(fileextension);
                if (AdditionalName != null)
                {
                    filename = AdditionalName + "/" + filename;
                }
                var filepath = fileBaseFolder + path + additional + filename;
                var finfo = new FileInfo(filepath);
                if (finfo.Directory != null && !finfo.Directory.Exists)
                {
                    finfo.Directory.Create();
                }
                if (ImageExtentions.Contains(fileextension.ToLower()))
                {
                    var reducedcolorImage = ReduceColorDepth(file, 8);
                    File.WriteAllBytes(filepath, reducedcolorImage);
                }
                else
                {
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return path + additional + filename;

            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync($"{ex.Message} |  InnerException: {ex.InnerException},");
                throw;

            }
        }
        private string GenerateFileName(string extension)
        {
            // e.g.: e2384d8f-f24e-47c8-b1d0-d2c286dd9c1b-490x240.png
            return string.Format("{0}{1}", Guid.NewGuid(), extension);
        }
        /// <summary>
        /// Used to Remove a file acording to the given path in a base folder
        /// </summary>
        /// <param name="fileBaseFolder">First/Base Directory to Save the file (ex: wwwroot)</param>
        /// <param name="path">The path after base folder (e.g.: users/profile/)</param>
        /// <returns>Complete Task if there is no exception</returns>
        public Task RemoveFile(string fileBaseFolder, string? path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return Task.CompletedTask;
                }
                var oldattToremove = fileBaseFolder + path;
                File.Delete(oldattToremove);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        private byte[] ReduceColorDepth(Stream imageFile, int bitsPerPixel)
        {
            using var image = new MagickImage(imageFile);
            image.Quantize(new QuantizeSettings { Colors = 1 << bitsPerPixel });
            return image.ToByteArray();
        }
    }
}

