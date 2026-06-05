using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Images {
    public class ProductImageDto {
        public string Url { get; private set; }

        public ProductImageDto(string url) {
            Url = url;
        }
    }
}
