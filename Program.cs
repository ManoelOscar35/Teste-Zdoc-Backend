using System.Text.Json.Serialization;
using GestaoPessoalApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Registra o DbContext (ajuste a connection string no appsettings.json)
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2) Configura Controllers + ignora ciclos de referência
builder.Services.AddControllers()
	.AddJsonOptions(opts =>
	{
		opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		opts.JsonSerializerOptions.MaxDepth = 64;
	});

// 3) Swagger (opcional, mas recomendado em dev)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
