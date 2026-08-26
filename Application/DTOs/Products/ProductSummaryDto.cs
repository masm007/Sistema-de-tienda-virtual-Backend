using Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class ProductSummaryDto {
        //esta clase es lo que enviara el product detail
        public int Id { get; private set; }
        public string Name { get; private set; }

        public ProductSummaryDto(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
