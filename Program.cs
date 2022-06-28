using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//gerando uma chave simétrica, recebendo a chave criada no settings e transformando em bytes
var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


//Adicionando nosso DataBase InMemory
//builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));

/*Injeção de dependência:
AddScoped garante que só haja um DataContext por requisição,
para não ficar gerando várias conexões no db, 
destruindo essa requisição logo após ela acabar.*/
builder.Services.AddScoped<DataContext, DataContext>();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//força o uso https, segurança na aplicação.
app.UseHttpsRedirection();

//acrescentamos a autenticação
app.UseAuthentication();

app.UseAuthorization();

//como mapear as rotas pelo controllers
app.MapControllers();

app.Run();
