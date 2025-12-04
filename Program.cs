using CRUD_WebAPI_ViaCEP.Data;
using CRUD_WebAPI_ViaCEP.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container.

// Configuração do MySQL com EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    // Usa Pomelo para MySQL
    options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// Configuração do HttpClient para o ViaCEP (Good Practice)
builder.Services.AddHttpClient<IViaCepService, ViaCepService>();

// Injeção de Dependência da Camada de Serviço
// 3. Divisão das Camadas e suas responsabilidades: Garante que o Controller use a interface do Service.
builder.Services.AddScoped<IEnderecoService, EnderecoService>();

// Configuração padrão do .NET para Controllers, Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger (Documentação para Postman/Scalar)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
