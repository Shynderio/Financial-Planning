using Microsoft.EntityFrameworkCore;
using FinancialPlanningDAL.Data;
using FinancialPlanningDAL.Repositories;
using FinancialPlanningAPI.Helpers;
using FinancialPlanningBAL.Services;
using FinancialPlanningBAL.ScheduleTasks;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ITermRepository, TermRepository>();
builder.Services.AddScoped<TermService>();
builder.Services.AddSingleton<IHostedService, StartTerm>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FinancialPlanningDAL")));


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