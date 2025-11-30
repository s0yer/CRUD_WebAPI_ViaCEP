using CRUD_WebAPI_ViaCEP.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CRUD_WebAPI_ViaCEP.Data
{
   

        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<EnderecoAPI> Enderecos { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Opcional: Configurações adicionais
                base.OnModelCreating(modelBuilder);
            }
        }
    
}
