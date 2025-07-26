using Iltuttofare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Iltuttofare.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
       public DbSet<SessaoCadastro> SessoesCadastro { get; set; }
       public DbSet<Profissional> Profissionais { get; set; }
       public DbSet<Categoria> Categorias { get; set; }
       public DbSet<Subcategoria> Subcategorias { get; set; }
       public DbSet<ProfissionalSubcategoria> ProfissionalSubcategorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfissionalSubcategoria>()
                .HasKey(ps => new { ps.ProfissionalId, ps.SubcategoriaId });
        }


    }
}
