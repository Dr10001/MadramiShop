using MadramiShop.Api.Data;
using MadramiShop.Api.Data.Entities;
using MadramiShop.Api.Endpoints;
using MadramiShop.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connectionString);
});
builder.Services.AddTransient<AuthService>()
    .AddTransient<OrderService>()
    .AddTransient<ProductService>()
    .AddTransient<UserService>()
    .AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var issuer = builder.Configuration.GetValue<string>("Jwt: Issuer");

        var secretKey = builder.Configuration.GetValue<string>("Jwt: SecretKey");
        if (secretKey == null)
            secretKey = "itwasstupid";
        var securityKey = System.Text.Encoding.UTF8.GetBytes(secretKey);
        var symetricKey = new SymmetricSecurityKey(securityKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidateIssuer = true,
            IssuerSigningKey = symetricKey,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // only for development next line
    AutoMigrateDb(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthentication().UseAuthorization();

app.MapAuthEndpoints()
    .MapOrderEndpoints()
    .MapProductEndpoints()
    .MapUserEndpoints();

app.Run();

//only for development
static void AutoMigrateDb(IServiceProvider sp)
{
   using var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    if (context.Database.GetAppliedMigrations().Any())
        context.Database.Migrate();
}