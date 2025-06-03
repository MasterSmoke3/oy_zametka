using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZametkiApp.Data;
using Microsoft.Extensions.Configuration;
using ZametkiApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Подключение конфигурации строки подключения
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") 
    ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// Подключение EF Core + SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=notes.db"));

// Identity с отключённым подтверждением почты
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Razor Pages — по умолчанию требуют авторизацию
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/"); // защищаем весь сайт
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
    options.Conventions.AllowAnonymousToPage("/Account/ForgotPassword");
    options.Conventions.AllowAnonymousToPage("/Account/ResetPassword");
});

// Добавляем контроллеры (если будут API)
builder.Services.AddControllers();

builder.Services.AddHostedService<DeadlineNotifierService>();

// Запускаем приложение
var app = builder.Build();

// Ошибки только для Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Логирование запросов в консоль
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
});

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Маршруты
app.MapRazorPages();
app.MapControllers();

// Автоматическое применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
