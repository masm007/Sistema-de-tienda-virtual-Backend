using Domain.Entity;
using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Categories {
    public class CategoryDto {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public CategoryDto(int id, string name, string description) {
            Id = id;
            Name = name;
            Description = description;
        }

    }
}
