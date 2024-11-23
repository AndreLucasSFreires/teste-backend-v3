using Aplication.DTO;
using Aplication.Services.Formatters;
using Aplication.Services.Interfaces;
using AutoMapper;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using TesteBackendV3;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPlayService, Aplication.Services.PlayService>();
builder.Services.AddScoped<IStatementService, Aplication.Services.StatementService>();

builder.Services.AddDbContext<TesteBackendV3DbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

PlayEndpoints.GetPlays(app);

StatementEndpoints.StatementText(app);

StatementEndpoints.StatementXml(app);

InvoiceEndpoints.InvoicePost(app);

StatementEndpoints.StatementSaved(app);

app.Run();
