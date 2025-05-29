using TestingTask.Models;

namespace TestingTask.Repositories
{
    public interface IHouseRepository
    {
        Task<List<House>> GetAllAsync();
        Task<House> GetByIdAsync(int id);
        Task<House> AddAsync(House house);
        Task<House> UpdateAsync(House house);
        Task<bool> DeleteAsync(int id);
    }
}
