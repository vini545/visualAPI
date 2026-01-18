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

        public DbSet<Conta> Contas => Set<Conta>();

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .HasOne(p => p.Conta)
                .WithOne(c => c.Pessoa)
                .HasForeignKey<Conta>(c => c.PessoaId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
