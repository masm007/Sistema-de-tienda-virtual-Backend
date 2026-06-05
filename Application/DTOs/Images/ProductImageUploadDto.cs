using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Images {
    public class ProductImageUploadDto {
        public Stream FileStream;
        public string FileName;

        public ProductImageUploadDto(Stream fileStream, string fileName) {
            FileStream = fileStream;
            FileName = fileName;
        }
    }
}
