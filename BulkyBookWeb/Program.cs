using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// CUANDO SE QUIERA REGISTRAR CUALQUIER COSA EN EL DEPENDENCY INJECTION CONTAINER, SE HACE
// ACA COMO CON EL "AddDbContext"
// para q ocupe ApplicationDbContext y el connectionString de appsetings
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation(); para el hotreload x si estoy trabajando en previo 
// .net Core 6

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); // para q al pedir un IUnitOfWork devuelva un UnitOfWork

var app = builder.Build();

// * * * * * * * * * * * Configure the HTTP request pipeline. ( COMO SE RESPONDE A UN WEB REQUEST )
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}  ORIGINAL

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
// localhost.../Category/index/3
// localhost.../{controller}/{action}/{id}
app.Run();
