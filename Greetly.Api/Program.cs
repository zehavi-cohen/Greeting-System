using Greetly.Api.Endpoints;
using Greetly.Application.Interfaces.Services;
using Greetly.Infrastructure.Persistence;
using Greetly.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Services ???? ??????
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IOccasionService, OccasionService>();
builder.Services.AddScoped<IStyleService, StyleService>();
builder.Services.AddScoped<IGreetingService, GreetingService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IDraftAgentService, DraftAgentService>();
builder.Services.AddSingleton<IOccasionCacheService, OccasionCacheService>();

// TODO (???? ???): ????? ??? ?? ???????? ????? ?? ?????? -
// IPasswordHasher, IJwtTokenService, IFileStorageService,
// IContentAgentClient, IDesignAgentClient
// + AddAuthentication().AddJwtBearer(...) + AddAuthorization(policy "AdminOnly")

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ????? ??? ???????? ?? ????? ????
using (var scope = app.Services.CreateScope())
{
    var cache = scope.ServiceProvider.GetRequiredService<IOccasionCacheService>();
    await cache.RefreshAsync();
}

app.MapAuthEndpoints();
app.MapOccasionEndpoints();
app.MapStyleEndpoints();
app.MapGreetingEndpoints();
app.MapFavoriteEndpoints();
app.MapDraftEndpoints();

app.Run();
