using Data.Persistence;
using Domain.Entity;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories {
    public class OrderRepository : IOrderRepository<OrderEntity, string> {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext dbContext) {
            _context = dbContext;
        }

        public async Task CreateAsync(OrderEntity entity) {
            if (entity == null) throw new ArgumentNullException();
            await _context.Orders.AddAsync(entity);
        }

        public Task DeleteAsync(OrderEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Orders.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync() {
            return await _context.Orders.AsNoTracking()
                .Include(ord => ord.User)
                .Include(ord => ord.OrderDetails)
                .OrderBy(ord => ord.Id).ToListAsync();
        }

        public async Task<IEnumerable<OrderEntity>> GetAllByUserIdAsync(int userId) {
            return await _context.Orders.AsNoTracking()
                .Include(ord => ord.OrderDetails)
                .Where(ord => ord.UserId == userId).OrderBy(ord => ord.Id).ToListAsync();
        }

        public async Task<OrderEntity?> GetByOrderNumberForAdminAsync(string orderNumber) {
            return await _context.Orders.Include(ord => ord.User)
            .Include(ord => ord.OrderDetails).ThenInclude(detail => detail.Product)
            .FirstOrDefaultAsync(ord => ord.OrderNumber == orderNumber);
        }

        public async Task<OrderEntity?> GetByOrderNumberForUserAsync(string orderNumber, int userId) {
            return await _context.Orders.Include(ord => ord.User)
                .Include(ord => ord.OrderDetails)
                    .ThenInclude(detail => detail.Product)
                .FirstOrDefaultAsync(ord =>
                    ord.OrderNumber == orderNumber &&
                    ord.UserId == userId);
        }

        //fue agregado aqui por motivos de manejar una sola transaccion en un solo lado
        public async Task CreateWithNextNumberAsync(OrderEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            await using var transaction =
                await _context.Database.BeginTransactionAsync();
            try {
                // Obtener y bloquear la fila de la secuencia
                var sequence = await _context.OrderNumbers
                    .FromSqlRaw("""
                        SELECT *
                        FROM ordernumbersequence
                        WHERE Id = 1
                        LIMIT 1
                        FOR UPDATE
                        """)
                    .SingleAsync();
                sequence.Increment();
                string orderNumber = $"FAC-{sequence.LastNumber:D8}";
                // Asignar el número a la orden
                entity.SetOrderNumber(orderNumber);

                // Descontar el stock de los productos comprados
                foreach (var detail in entity.OrderDetails) {
                    // Obtener y bloquear la fila del producto
                    var product = await _context.Products
                        .FromSqlRaw("""
                            SELECT *
                            FROM Products
                            WHERE Id = {0}
                            LIMIT 1
                            FOR UPDATE
                            """, detail.ProductId)
                        .SingleOrDefaultAsync();

                    if (product == null) {
                        throw new InvalidOperationException(
                            $"El producto con id {detail.ProductId} no existe.");
                    }

                    product.DecreaseStock(detail.Quantity);
                }

                // Agregar la orden al contexto
                await _context.Orders.AddAsync(entity);
                // Guardar cambios
                await _context.SaveChangesAsync();
                // Confirmar transacción
                await transaction.CommitAsync();
            } catch {
                // Si algo falla, deshacer toda la operación
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> SaveChangesAsync() {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(OrderEntity entity) {
            if (entity == null) {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Orders.Update(entity);
            return Task.CompletedTask;
        }
    }
}
