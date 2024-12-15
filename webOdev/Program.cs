using Microsoft.EntityFrameworkCore;
using webOdev.NewFolder;
using webOdev.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for User model
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support for managing login state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true; // Güvenlik için sadece HTTP üzerinden erişilebilir
    options.Cookie.IsEssential = true; // Çerez zorunlu
});

builder.Services.AddScoped<EmailService>(); // EmailService'i Dependency Injection'a ekleyin

// Add controllers and views
builder.Services.AddControllersWithViews();




var app = builder.Build();

// Middleware for session and routing
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Session middleware'i aktif et

// Map routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Veritabanında Admin kullanıcısı var mı kontrol edin
    if (!context.Users.Any(u => u.Username == "admin"))
    {
        context.Users.Add(new User
        {
            Username = "admin",
            Password = "admin123", // NOT: Gerçek uygulamalarda şifre hashing ile saklanmalıdır
            Role = "Admin"
        });
        context.SaveChanges();
    }
}


app.Run();
