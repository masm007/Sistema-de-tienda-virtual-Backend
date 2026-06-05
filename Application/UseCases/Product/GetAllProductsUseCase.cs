using Domain.Entity;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Product {
    public class GetAllProductsUseCase {
        private IProductRepository<ProductEntity, int> _repository;

        public GetAllProductsUseCase(IProductRepository<ProductEntity, int> repository) {
            _repository = repository;
        }
    }
}
