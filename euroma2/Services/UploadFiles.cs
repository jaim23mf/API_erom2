using euroma2.Models;
using Microsoft.Extensions.Options;

namespace euroma2.Services
{
    public class UploadFiles
    {
        private readonly PtaInfo _options;

        public UploadFiles(PtaInfo options) {
            this._options = options;
        }

        public string url { get; set; }

        public async Task<UploadFiles> UploadFileToAsync(string path, IFormFile file) { 
         var basePath = Path.Combine(Directory.GetCurrentDirectory(),path);
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
 
                }

            //this.url = filePath;
            this.url = $"{this._options.BaseFileUrl}/{path}/{Path.GetFileName(filePath)}";
            return this;
        }

    }
}
