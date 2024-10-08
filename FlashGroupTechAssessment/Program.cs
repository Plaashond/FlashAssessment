using Microsoft.Data.SqlClient;
using System.Data;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using FlashGroupTechAssessment.Repositories.Message;
using FlashGroupTechAssessment.Services;
using FlashGroupTechAssessment.Wrappers;

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
builder.Services.AddTransient<IDbConnectionWrapper>(sp =>
{
	var configuration = sp.GetRequiredService<IConfiguration>();
	var connectionString = configuration.GetConnectionString("DefaultConnection");
	var dbConnection = new SqlConnection(connectionString);
	return new DbConnectionWrapper(dbConnection);
});
builder.Services.AddScoped<ISensitiveWordRepository,SensitiveWordRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
else
{
	app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
