using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Images {
    public class UploadedImageDto {
        public string PublicId { get; set; }
        public string Url { get; set; }

        public UploadedImageDto(string publicId, string url) {
            PublicId = publicId;
            Url = url;
        }
    }
}
