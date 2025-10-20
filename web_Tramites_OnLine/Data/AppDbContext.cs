using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace web_Tramites_OnLine.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Documento> Documentos { get; set; }
        //public DbSet<TipoDocumento> TiposDocumento { get; set; }
    }

}
