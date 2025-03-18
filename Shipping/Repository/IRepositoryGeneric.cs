using Shipping.Models;

namespace Shipping.Repository
{
    public interface IRepositoryGeneric<Tentity> where Tentity : class
    {
   
        Task<Tentity> GetByIdAsync(int id);
        Task<IQueryable<Tentity>> GetAllAsync();// to use linq
        Task<IQueryable<Tentity>> GetAllExistAsync();
        Task AddAsync(Tentity entity);
        Task UpdateById(int id);
        Task DeleteByID(int id);
        void Update(Tentity entity);
        void Delete(Tentity entity);
    }
   
}
