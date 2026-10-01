using Domain.Entity;
using System.Threading.Tasks;

namespace Domain.Repository {
    public interface ITaxSettingRepository {
        Task<TaxSettingEntity?> GetAsync();
        Task UpdateAsync(TaxSettingEntity entity);
        Task<int> SaveChangesAsync();
    }
}
