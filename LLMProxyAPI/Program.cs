using LLM_Proxy_API.Clients;
using LLM_Proxy_API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Typed HttpClient for Ollama
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434/"); // Ollama server
});

var app = builder.Build();

app.UseApiKeyValidation();

// Swagger
//app.UseSwagger();
//app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });

    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
