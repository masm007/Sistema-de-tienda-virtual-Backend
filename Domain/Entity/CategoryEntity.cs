using Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class CategoryEntity {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<ProductEntity> Products { get; private set; } = new List<ProductEntity>();


        private CategoryEntity() {
        }

        public CategoryEntity(int id, string name, string description) {
            FieldsValidator.ValidateText(name, "nombre", 3, 50);
            FieldsValidator.ValidateText(description, "descripcion", 20, 100);
            Id = id;
            Name = name;
            Description = description;
        }
        public CategoryEntity(string name, string description) {
            FieldsValidator.ValidateText(name, "nombre", 3, 50);
            FieldsValidator.ValidateText(description, "descripcion", 20, 100);
            Name = name;
            Description = description;
        }
    }
}
