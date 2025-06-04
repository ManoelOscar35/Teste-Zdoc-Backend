//using System.Text.Json.Serialization;
//using GestaoPessoalApi.Data;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// 1) Registra o DbContext (ajuste a connection string no appsettings.json)
//builder.Services.AddDbContext<AppDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

//// 2) Configura Controllers + ignora ciclos de referência
//builder.Services.AddControllers()
//	.AddJsonOptions(opts =>
//	{
//		opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//		opts.JsonSerializerOptions.MaxDepth = 64;
//	});

//// 3) Swagger (opcional, mas recomendado em dev)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();

//app.MapControllers();

//app.Run();


using System.Text.Json.Serialization;
using GestaoPessoalApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== 1) Adicione o CORS SERVICES antes de AddControllers =====
builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "AllowAngular",
		policy =>
		{
			policy
				.WithOrigins("http://localhost:4200") // <- origem da sua app Angular
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
});

// ===== 2) DbContext, Controllers, Swagger etc. =====
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers()
	.AddJsonOptions(opts =>
	{
		opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		opts.JsonSerializerOptions.MaxDepth = 64;
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ===== 3) UseCors precisa vir antes de MapControllers =====
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");

app.MapControllers();

app.Run();
