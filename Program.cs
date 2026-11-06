using Microsoft.EntityFrameworkCore;
using MusicaApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MusicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MusicaDB")));

// CORS con un origen concreto (nunca AllowAnyOrigin)
var origenPermitido = builder.Configuration["Cors:Origen"] ?? "http://localhost:3000";
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins(origenPermitido)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Swagger visible también en la API publicada
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Musica API v1");
    options.RoutePrefix = "swagger";
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();