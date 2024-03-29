using Microsoft.Extensions.Configuration;
using ServicioPAI.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IRepositorioPersonas, RepositorioPersonas>();
builder.Services.AddTransient<IRepositorioLog, RepositorioLog>();
builder.Services.AddTransient<IRepositorioVacunas, RepositorioVacunas>();
builder.Services.AddTransient<IRepositorioCohorte, RepositorioCohorte>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
