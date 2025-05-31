using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestingTask.Data;
using TestingTask.Models;
using TestingTask.Repositories;
using TestingTask.Services;

namespace TestingTask.Tests
{
    public class HouseTest
    {
        private IHouseService _service;
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "HouseDb")
                .Options;

            _context = new AppDbContext(options);
            var repo = new HouseRepository(_context);
            _service = new HouseService(repo);
        }

        [Test]
        public async Task AddAsync_ShouldAddHouse()
        {
            var house = new House { Title = "Test House", Description = "Nice", Price = 100 };
            var result = await _service.AddAsync(house);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("Test House"));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnHouses()
        {
            await _service.AddAsync(new House { Title = "House1", Description = "Desc1", Price = 80 });
            await _service.AddAsync(new House { Title = "House2", Description = "Desc2", Price = 90 });

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }


        [Test]
        public async Task UpdateAsync_ShouldUpdateHouse()
        {
            var house = await _service.AddAsync(new House { Title = "Old", Description = "Old", Price = 50 });
            house.Title = "Updated";

            var updated = await _service.UpdateAsync(house);

            Assert.That(updated.Title, Is.EqualTo("Updated"));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveHouse()
        {
            var house = await _service.AddAsync(new House { Title = "ToDelete", Description = "Test", Price = 60 });
            var deleted = await _service.DeleteAsync(house.Id);

            Assert.That(deleted, Is.True);
        }
    }
}
