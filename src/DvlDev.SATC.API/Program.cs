using DvlDev.SATC.API.Helpers.Extensions;
using DvlDev.SATC.API.Mappers;
using DvlDev.SATC.Application;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Configure Kestrel to listen on HTTPS

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

#pragma warning disable ASP0000
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
#pragma warning restore ASP0000
{
	var context = scope.ServiceProvider.GetRequiredService<DataContext>();
	context.Database.EnsureCreated(); // Ensures the database is initialized
	context.SaveChanges(); // Explicitly save data, in case some changes were made
}


// Adding Application Layer
builder.Services.AddApplication();
builder.Services.AddHttpClients(config);



builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
	});
});

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
app.UseCors("AllowAll");

app.UseMiddleware<ValidationMapperMiddleware>();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.Run();