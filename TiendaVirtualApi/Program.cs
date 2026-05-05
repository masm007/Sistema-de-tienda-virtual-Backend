using Application.Interfaces.Configuration;
using Application.Interfaces.Security;
using Application.UseCases.Users;
using Data.Persistence;
using Data.Repositories;
using Domain.Entity;
using Domain.Repository;
using Infrastructure.Configurations;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TiendaVirtualApi.Endpoints;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("No hay ninguna cadena de conexion a la bd");

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        policy => {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options => {
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
builder.Services.AddAuthentication(options => {
    //usa JWT para intentar identificar al usuario
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //responde usando JWT
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//inyeccion de dependencias
builder.Services.AddScoped<IRepository<UserEntity, int>, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

//inyeccion de casos de usos
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
//servicios
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenSettings, RefreshTokenSettings>();
builder.Services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

var app = builder.Build();
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//para asegurarse aunque maneja el routing automáticamente al ser minimal api
app.UseRouting();
//identifica quien eres
app.UseAuthentication();
//verifica que puedes hacer
app.UseAuthorization();

app.MapUsersEndpoints();

app.Run();

