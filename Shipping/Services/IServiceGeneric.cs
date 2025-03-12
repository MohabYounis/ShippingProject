namespace Shipping.Services
{

    public interface IServiceGeneric<Tentity> where Tentity : class
    {
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task<Tentity> GetByIdAsync(int id);
        Task AddAsync(Tentity entity);
        Task UpdateAsync(int id);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }

}
