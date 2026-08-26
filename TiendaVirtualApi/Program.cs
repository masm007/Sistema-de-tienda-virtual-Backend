using Application.Interfaces.Configuration;
using Application.Interfaces.Security;
using Application.Interfaces.Storage;
using Application.UseCases.Category;
using Application.UseCases.Orders;
using Application.UseCases.Product;
using Application.UseCases.RefreshToken;
using Application.UseCases.Users;
using Data.Persistence;
using Data.Repositories;
using Domain.Entity;
using Domain.Repository;
using Infrastructure.Configurations;
using Infrastructure.Security;
using Infrastructure.Storage;
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

var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? 
        throw new InvalidOperationException("FRONTEND_URL no configurado");

builder.Services.AddCors(options => {
    options.AddPolicy("AllowFrontend",
        policy => {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
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
builder.Services.AddScoped<IUserRepository<UserEntity, int>, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICategoryRepository<CategoryEntity, int>, CategoryRepository>();
builder.Services.AddScoped<IProductRepository<ProductEntity, int>, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository<ProductImageEntity, int>, ProductImageRepository>();
builder.Services.AddScoped<IOrderRepository<OrderEntity, string>, OrderRepository>();

//inyeccion de casos de usos
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<LogoutUserUseCase>();
builder.Services.AddScoped<GeneralRefreshTokenUseCase>();

builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<GetAllProductsUseCase>();
builder.Services.AddScoped<GetAllActiveProductsUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();

builder.Services.AddScoped<CreateCategoryUseCase>();
builder.Services.AddScoped<GetAllCategoriesUseCase>();
builder.Services.AddScoped<GetCategoryByIdUseCase>();
builder.Services.AddScoped<UpdateCategoryUseCase>();
builder.Services.AddScoped<DeleteCategoryUseCase>();

builder.Services.AddScoped<CreateOrderUseCase>();
builder.Services.AddScoped<GetAllOrdersUseCase>();
builder.Services.AddScoped<GetAllOrdersByUserIdUseCase>();
builder.Services.AddScoped<GetOrderByOrderNumberForUserUseCase>();
builder.Services.AddScoped<GetOrderByOrderNumberForAdminUseCase>();
builder.Services.AddScoped<CancelOrderUseCase>();
builder.Services.AddScoped<UpdateOrderUseCase>();

//servicios
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenSettings, RefreshTokenSettings>();
builder.Services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

// -Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//para asegurarse aunque maneja el routing automáticamente al ser minimal api
app.UseRouting();
//habilita
app.UseCors("AllowFrontend");
//identifica quien eres
app.UseAuthentication();
//verifica que puedes hacer
app.UseAuthorization();

app.MapUsersEndpoints();
app.MapCategoriesEndpoints();
app.MapProductsEndpoints();
app.MapOrdersEndpoints();

app.Run();

