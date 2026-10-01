using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Categories {
    public class CategorySummaryDto {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CategorySummaryDto(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
