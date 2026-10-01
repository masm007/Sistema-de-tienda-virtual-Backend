using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Images {
    public class ProductImageDto {
        public int Id { get; private set; }
        public string Url { get; private set; }

        public ProductImageDto(int id, string url) {
            Id = id;
            Url = url;
        }

        // Para respuestas públicas donde el Id de la imagen no debe exponerse
        public ProductImageDto(string url) {
            Url = url;
        }
    }
}
