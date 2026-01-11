namespace Shipping.Services
{

    public interface IServiceGeneric<Tentity> where Tentity : class
    {
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task<IEnumerable<Tentity>> GetAllExistAsync();
        Task<Tentity> GetByIdAsync(int id);
        Task<Tentity> GetByNameAsync(string name);
        Task AddAsync(Tentity entity);
        Task UpdateAsync(Tentity entity);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }

}
