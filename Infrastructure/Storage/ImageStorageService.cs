using Application.DTOs.Images;
using Application.Interfaces.Storage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage {
    public class ImageStorageService : IImageStorageService {
        private readonly Cloudinary _cloudinary;
        public ImageStorageService(IConfiguration configuration) {
            /*
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );
            */
            var account = new Account(
                configuration["CLOUDINARY_CLOUD_NAME"],
                configuration["CLOUDINARY_API_KEY"],
                configuration["CLOUDINARY_API_SECRET"]
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<UploadedImageDto> UploadImageAsync(Stream fileStream, string fileName) {
            var uploadParams = new ImageUploadParams {
                File = new FileDescription(fileName, fileStream)
            };
            var result = await _cloudinary.UploadAsync(uploadParams);
            return new UploadedImageDto(result.PublicId , result.SecureUrl.ToString()) ;
        }

        public async Task DeleteImageAsync(string publicId) {
            await _cloudinary.DestroyAsync(new DeletionParams(publicId));
        }
    }
}
