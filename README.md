1.2. Pré-requisitos
.NET 7 SDK instalado

SQL Server (ou outro provedor configurado no appsettings.json)

Visual Studio 2022/2019 ou VS Code (com extensão C#)

(Opcional) SQL Server Management Studio para visualizar/dar manutenção no banco

1.3. Configuração inicial
Clone este repositório ou acesse a pasta da API:

bash
Copiar
Editar
git clone https://github.com/ManoelOscar35/Teste-Zdoc-Backend.git
cd Zdoc-API
Abra o appsettings.json e ajuste a string de conexão:

jsonc
Copiar
Editar
{
  "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestaoPessoalDb;Trusted_Connection=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
Se usar autenticação local no SQL Server: Server=.;Database=ZdocDb;User Id=sa;Password=SuaSenha;

Ajuste o nome do banco (ZdocDb) conforme desejar.

Abra um terminal na pasta do projeto e execute:

bash
Copiar
Editar
dotnet restore
1.4. Migrations e criação do banco
Crie a migration inicial (se ainda não existir):

bash
Copiar
Editar
dotnet ef migrations add InitialCreate
Aplique a migration para criar o esquema:

bash
Copiar
Editar
dotnet ef database update
Isso criará as tabelas: Funcionarios, Ferias, HistoricoAlteracao.

1.5. Habilitar CORS e configurar JSON
O código já inclui em Program.cs:

csharp
Copiar
Editar
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.MaxDepth = 64;
    });

app.UseCors("AllowAngular");
app.MapControllers();
Isso garante que chamadas AJAX vindas do Angular não sejam bloqueadas.

A configuração de JSON ignora ciclos de referência (por exemplo, Funcionario → Ferias → Funcionario).

1.6. Executando a API
No terminal, a partir da pasta da API:

bash
Copiar
Editar
dotnet run
Por padrão, será iniciado em http://localhost:5117 (ou outra porta se configurada).

Acesse http://localhost:5117/swagger para visualizar e testar endpoints via Swagger UI.

1.7. Endpoints principais
Funcionários

GET /api/Funcionarios

GET /api/Funcionarios/{id}

POST /api/Funcionarios

PUT /api/Funcionarios/{id}

DELETE /api/Funcionarios/{id}

(Internamente registra histórico em HistoricoAlteracao)

Férias

GET /api/Ferias

GET /api/Ferias/{id}

POST /api/Ferias

PUT /api/Ferias/{id}

DELETE /api/Ferias/{id}

Relatório

O relatório consome /api/Funcionarios para listar todos os funcionários.
