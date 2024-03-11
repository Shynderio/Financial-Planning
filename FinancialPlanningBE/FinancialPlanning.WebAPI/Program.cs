using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Repositories;
using FinancialPlanning.WebAPI.Helpers;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Service.ScheduleTasks;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ITermRepository, TermRepository>();
builder.Services.AddScoped<TermService>();
builder.Services.AddSingleton<IHostedService, StartTerm>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("FinancialPlanning.Data")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAWSService<IAmazonS3>(new AWSOptions
{
    Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKey"], builder.Configuration["AWS:SecretKey"]),
    Region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"])
});

builder.Services.AddScoped<FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Apply CORS policy here

app.UseAuthorization();

app.MapControllers();

app.Run();
