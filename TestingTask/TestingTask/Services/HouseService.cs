using TestingTask.Models;
using TestingTask.Repositories;

namespace TestingTask.Services
{
    public class HouseService : IHouseService
    {
        private readonly IHouseRepository _repo;

        public HouseService(IHouseRepository repo)
        {
            _repo = repo;
        }

        public Task<List<House>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<House> GetByIdAsync(int id)
        { 
            return _repo.GetByIdAsync(id);
        }

        public Task<House> AddAsync(House house)
        {
            return _repo.AddAsync(house);
        }

        public Task<House> UpdateAsync(House house)
        { 
            return _repo.UpdateAsync(house);
        }

        public Task<bool> DeleteAsync(int id)
        { 
            return _repo.DeleteAsync(id);
        }
    }

}
