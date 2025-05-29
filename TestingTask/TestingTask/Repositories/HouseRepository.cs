using Microsoft.EntityFrameworkCore;
using TestingTask.Data;
using TestingTask.Models;

namespace TestingTask.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private readonly AppDbContext _context;

        public HouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<House>> GetAllAsync() => await _context.Houses.ToListAsync();

        public async Task<House> GetByIdAsync(int id) => await _context.Houses.FindAsync(id);

        public async Task<House> AddAsync(House house)
        {
            _context.Houses.Add(house);
            await _context.SaveChangesAsync();
            return house;
        }

        public async Task<House> UpdateAsync(House house)
        {
            _context.Houses.Update(house);
            await _context.SaveChangesAsync();
            return house;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var house = await _context.Houses.FindAsync(id);
            if (house == null) return false;
            _context.Houses.Remove(house);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
