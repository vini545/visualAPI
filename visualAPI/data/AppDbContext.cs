using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using visualAPI.Entities;

namespace visualAPI.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    }
}
