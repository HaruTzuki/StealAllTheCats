using DvlDev.SATC.API.Mappers;
using DvlDev.SATC.Application;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

// Adding Application Layer
builder.Services.AddApplication();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Starts the pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationMapperMiddleware>();
app.MapControllers();

app.Run();