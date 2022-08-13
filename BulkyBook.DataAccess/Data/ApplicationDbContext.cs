using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    // DbContext es lo q me permite acceder y manejar la db
    public class ApplicationDbContext : DbContext
    {
        // para establecer la conexion con entityF
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
            // para q ocupe el modelo Category en la DB
            // Categories es el nombre con el q va a crear la tabla
            public DbSet<Category> Categories { get; set; }
            public DbSet<CoverType> CoverTypes { get; set; }
    }
}
