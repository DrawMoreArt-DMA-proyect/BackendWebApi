using System;
using Microsoft.EntityFrameworkCore;
using mx.edu.uttt.dma.webapi.Entidades;

namespace mx.edu.uttt.dma.webapi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
