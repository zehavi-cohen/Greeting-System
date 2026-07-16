using Greetly.Api.Endpoints;
using Greetly.Application.Interfaces.Services;
using Greetly.Infrastructure.Persistence;
using Greetly.Infrastructure.Services;
using Greetly.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Greetly.Application.Interfaces.Auth;
using Greetly.Application.Interfaces.Agents;
using Greetly.Application.Interfaces.Storage;
using Greetly.Infrastructure.Auth;
using Greetly.Infrastructure.Agents;
using Greetly.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=greetly.db";

    // מבטיח שהנתיב יהיה יחסי לתיקיית הפרויקט (Content Root), לא ל-working directory הנוכחי
    if (connectionString.Contains("Data Source=") && !Path.IsPathRooted(connectionString.Replace("Data Source=", "")))
    {
        var dbFileName = connectionString.Replace("Data Source=", "").Trim();
        var fullPath = Path.Combine(builder.Environment.ContentRootPath, dbFileName);
        connectionString = $"Data Source={fullPath}";
    }

    options.UseSqlite(connectionString);
});

// Services שכבר קיימים
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOccasionService, OccasionService>();
builder.Services.AddScoped<IStyleService, StyleService>();
builder.Services.AddScoped<IGreetingService, GreetingService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IDraftAgentService, DraftAgentService>();
builder.Services.AddSingleton<IOccasionCacheService, OccasionCacheService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IContentAgentClient, ContentAgentClient>();
builder.Services.AddScoped<IDesignAgentClient, DesignAgentClient>();

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed: משתמש אדמין קבוע, נוצר רק אם עוד לא קיים
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    var adminExists = await db.Users.AnyAsync(u => u.Email == "admin@greetly.com");
    if (!adminExists)
    {
        db.Users.Add(new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@greetly.com",
            PasswordHash = hasher.Hash("Admin123!"),
            DisplayName = "מנהל מערכת",
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }

    var cache = scope.ServiceProvider.GetRequiredService<IOccasionCacheService>();
    await cache.RefreshAsync();
}

// טעינת קאש האירועים עם עליית השרת
using (var scope = app.Services.CreateScope())
{
    var cache = scope.ServiceProvider.GetRequiredService<IOccasionCacheService>();
    await cache.RefreshAsync();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoints();
app.MapOccasionEndpoints();
app.MapStyleEndpoints();
app.MapGreetingEndpoints();
app.MapFavoriteEndpoints();
app.MapDraftEndpoints();

app.Run();
