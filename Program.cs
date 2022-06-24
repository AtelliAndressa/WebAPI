using WebAPI.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Adicionando nosso DataBase InMemory
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));

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


app.UseAuthorization();

//como mapear as rotas pelo controllers
app.MapControllers();

app.Run();
