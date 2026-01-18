using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using visualAPI.Entities;

namespace visualAPI.data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<User>();

                var usuarios = new List<User>()
                {
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Password = hasher.HashPassword(null, "senha123"),
                        UserName = "visualAPI"
                    }
                };
            
                context.Users.AddRange(usuarios);
            }


            if (!context.Pessoas.Any())
            {
                var pessoas = new List<Pessoa>
                {
                    new Pessoa
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Vinicius Cruz",
                        Conta = new Conta
                        {
                            Id = Guid.NewGuid(),
                            Saldo = 500
                        }
                    },
                    new Pessoa
                    {
                        Id = Guid.NewGuid(),
                        Nome = "João Silva",
                        Conta = new Conta
                        {
                            Id = Guid.NewGuid(),
                            Saldo = 1000
                        }
                    },
                    new Pessoa
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Maria Santos",
                        Conta = new Conta
                        {
                            Id = Guid.NewGuid(),
                            Saldo = 2500
                        }
                    }
                };
                
                context.Pessoas.AddRange(pessoas);
            }

            context.SaveChanges();
        }
    }
}
