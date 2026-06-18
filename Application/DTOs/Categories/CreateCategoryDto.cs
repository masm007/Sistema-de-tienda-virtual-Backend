using Domain.Entity;
using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Categories {
    public class CreateCategoryDto {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CreateCategoryDto(string name, string description) {
            Name = name;
            Description = description;
        }
    }
}
