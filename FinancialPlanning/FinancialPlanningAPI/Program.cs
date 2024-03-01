using Microsoft.EntityFrameworkCore;
using FinancialPlanningDAL.Data;
using FinancialPlanningBAL.Services;
using FinancialPlanningBAL.ScheduleTasks;
using FinancialPlanningDAL.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ITermRepository, TermRepository>();
builder.Services.AddScoped<TermService>();
builder.Services.AddSingleton<IHostedService, StartTerm>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FinancialPlanningDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));


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
