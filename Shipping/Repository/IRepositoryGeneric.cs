namespace Shipping.Repository
{
    public interface IRepositoryGeneric<Tentity> where Tentity : class
    {
        Task<Tentity> GetByIdAsync(int id);
        Task<IEnumerable<Tentity>> GetAllAsync();
        Task AddAsync(Tentity entity);
        Task UpdateById(int id);
        Task DeleteByID(int id);
        void Update(Tentity entity);
        void Delete(Tentity entity);
        void SaveDB();
    }
   
}
