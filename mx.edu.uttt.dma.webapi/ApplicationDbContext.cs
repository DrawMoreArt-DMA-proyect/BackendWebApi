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
        public DbSet<Generos> Generos { get; set; }
        public DbSet<ElementoGuardado> ElementosGuardados { get; set; }
        public DbSet<TipoRedesSocial> TipoRedesSociales { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RedSocial> RedesSociales { get; set; }
    }
}
