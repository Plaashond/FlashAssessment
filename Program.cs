using Microsoft.Data.SqlClient;
using System.Data;
using FlashGroupTechAssessment.Repositories.SensitiveWord;
using FlashGroupTechAssessment.Repositories.Message;
using FlashGroupTechAssessment.Services;

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
builder.Services.AddTransient<IDbConnection>(sp =>
		new SqlConnection(configuration.GetConnectionString("DefaultConnection")));
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
