using Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Storage {
    public interface IImageStorageService {
        Task<UploadedImageDto> UploadImageAsync(Stream fileStream, string fileName);
        Task DeleteImageAsync(string publicId);

    }
}
