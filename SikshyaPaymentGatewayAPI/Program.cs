using Microsoft.EntityFrameworkCore;
using SikshyaPaymentGatewayAPI.Data;
using SikshyaPaymentGatewayAPI.Repositories;
using SikshyaPaymentGatewayAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// MediatR added to service collection
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add DbConnection
builder.Services.AddDbContext<SikshyaDatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MsSqlConnectionString")
    )
);

// Map and register services
builder.Services.AddSingleton<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


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
