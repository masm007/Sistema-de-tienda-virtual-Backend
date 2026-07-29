using Application.DTOs.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Products {
    public class CreateProductResponseDto {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public CreateProductResponseDto(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
