using Shipping.Models;

namespace Shipping.Repository
{
    public interface IRepositoryGeneric<Tentity> where Tentity : class
    {
   
        Task<Tentity> GetByIdAsync(int id);
        Task<Tentity> GetByNameAsync(string name);
        Task<IQueryable<Tentity>> GetAllAsync();// to use linq
        Task<IQueryable<Tentity>> GetAllExistAsync();
        Task AddAsync(Tentity entity);
        Task DeleteByID(int id);
        Task Update(Tentity entity);
        void Delete(Tentity entity);
        Task<Tentity> GetByUserIdAsync(string userId);
    }
   
}
