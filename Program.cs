using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new [] { @"bin\" }, StringSplitOptions.None)[0];
IConfigurationRoot configuration = new ConfigurationBuilder()
		.SetBasePath(projectPath)
		.AddJsonFile("appsettings.json")
		.Build();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(sp =>
		new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

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
