using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    // DbContext es lo q me permite acceder y manejar la db
    public class ApplicationDbContext : IdentityDbContext
    {
        // para establecer la conexion con entityF
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // para q ocupe el modelo Category en la DB
        // Categories es el nombre con el q va a crear la tabla
        // un DbSet representa una tabla en la BD, xlo q cada q ocupo _db.Categories o cualquier
        // _db.algo, estoy haciendo referencia a una tabla en la BD
        // DbSet< el-modelo-xel-q-se-rige > el-nombre-q-tomo-aca { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}

// tras agregar un DbSet hay que hacer el 
// PM> add-migration _nombre_