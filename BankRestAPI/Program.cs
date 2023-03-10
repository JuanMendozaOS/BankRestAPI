using BankRestAPI.Data;
using BankRestAPI.Models.Bank;
using BankRestAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add database context
builder.Services.AddDbContext<BankDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("BankAPIConnectionString"))
);

// Inject service dependency 
builder.Services.AddScoped(typeof(IEntityService<Bank>), typeof(BankService));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
