using LibraryAPI.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.Entities;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var DbConnectionString = MyConfig.GetValue<string>("ConnectionStrings:DefaultConnection");


// Header Context

builder.Services.AddScoped<IHeaderContextService, HeaderContextService>();
builder.Services.AddHttpContextAccessor();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILibraryBooksRentalService, LibraryBooksRentalService>();
builder.Services.AddScoped<ILibraryBooksService, LibraryBooksService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(DbConnectionString, builder => {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
