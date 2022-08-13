using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    // *
    public class Category
    {
        // [key] la hace primary key, como sea si una prop se llama Id => entity F la hace
        // automaticamente una primary key, asi q en este caso [Key] es opcional
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order value must be between 1 and 100 only.")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}

// * 
// ESTO REPRESENTA UNA TABLA EN LA DB, EL NOMBRE DE LA DB SE DA EN EL CONNECTION
// STRING EN appsetings.json
// el nombre q se le da a la tabla se establece en Data/ApplicationDbContext, en la linea
// public DbSet<Category> Categories { get; set; }
// ocupa el modelo de la clase Category y le da el nombre Categories


// creo el modelo
// saco el connectionString de la db y lo pongo en appsettings.json
// instalo NuGet de entityF para conectar mi modelo a la DB ( en video 50 lo muestra para razor pages )
// Microsoft.EntityFrameworkCore y Microsoft.EntityFrameworkCore.SqlServer
// RECORDAR Q LOS PAQUETES Q INSTALO DEBEN SER DE LA MISMA VERSION
// lUEGO
// creo el dbContext para interactuar con entityF core
// para esto creo la carpeta data y luego la clase ApplicationDbContext
// este archivo, ApplicationDbContext es el q conecta la aplicacion con la db
// alla pongo
// public DbSet<Category> Category { get; set; }
// para q cree la tabla, y luego configuro para q use el connectionString
// 
// para decirle a la aplicacion que use applicationDbContext para interactuar como dbcontext y tienes q usar sql
// server con connection string q esta en appsetings.json
// esto se hace en program.cs, añadiendo services al container ( builder.Services. ... )
// la razon para añadirlo ahi es que de esta manera se va poder usar con dependency injection a traves de la aplicacion
// esto se hacia distinto en .net5
// ya finalmente para conectar con entityF y crear las tablas y se pongan las filas hay que poner las migraciones
// que van a crear el script para que suceda
// video 56 create database
// en tools->NuGet package manager->package manager console
// PM> add-migration AddCategoryToDb
// para poder ocupar el comando PRIMERO instalo el paquete NuGet
// Microsoft.EntityFrameworkCore.Tools
// y ya en la misma consola corro
// update-database
// y se crea la db y la tabla

