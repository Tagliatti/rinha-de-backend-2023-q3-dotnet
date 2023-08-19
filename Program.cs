using System.Data;
using Dapper;
using Npgsql;
using RinhaBackend2023Q3.Handlers;
using RinhaBackend2023Q3.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(_ => new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddScoped<PersonRepository>();

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();