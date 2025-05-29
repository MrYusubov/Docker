using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TestingTask.Models;

namespace TestingTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<House> Houses { get; set; }
    }
}
