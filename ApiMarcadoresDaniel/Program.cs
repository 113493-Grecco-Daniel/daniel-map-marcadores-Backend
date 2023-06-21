using ApiMarcadoresDaniel.Service.Marcadores;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMediatR(typeof(GetMarcadores_Business.Manejador).Assembly); 

builder.Services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<GetMarcadores_Business>());  // una sola vez tambien y tira la magia




builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
        });
});





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();


app.UseAuthorization();

app.UseCors(); // en este lugar especifico, después de app.UseAuthorization() y antes de app.MapControllers()

app.MapControllers();

app.Run();
