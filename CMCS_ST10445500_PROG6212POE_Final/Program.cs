//ST10445500 - PROG6212POE - Final Submission

//References: Microsoft Learn, Swagger Documentation, ASP.NET Core Documentation,
//              Stack Overflow, GitHub, and various NuGet package documentation.

//CMCS: Program

//.......................................o0oSTART OF FILEo0o........................................//

using CMCS_ST10445500_PROG6212POE_Final.Models;
using Microsoft.EntityFrameworkCore;

//...........................................................//

var builder = WebApplication.CreateBuilder(args);

//SERVICES
//...........................................................//

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "CMCS Claims API", Version = "v1" });
});

//register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//...........................................................//

var app = builder.Build();

//middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CMCS Claims API V1"));
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); //critical for auth

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

//...................................................................//

//seeding HR
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // ensures latest migration

    if (!db.Users.Any(u => u.Email == "hr@cmcs.ac.za"))
    {
        db.Users.Add(new AppUser
        {
            Name = "HR Administrator",
            Email = "hr@cmcs.ac.za",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Hr@2025"),
            Role = UserRole.HR
        });
        db.SaveChanges();
        Console.WriteLine("HR account created: hr@cmcs.ac.za / Hr@2025");
    }
}

//...................................................................//

app.Run();

//........................................o0oEND OF FILEo0o.........................................//