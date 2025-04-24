using Microsoft.EntityFrameworkCore;
using SegundoExamenAzure.Models;

namespace SegundoExamenAzure.Data
{
    public class CubosContext:DbContext
    {
        public CubosContext(DbContextOptions<CubosContext>options)
            :base(options) { }

        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<Usuario> Usuarios{ get; set; }
    }
}
