using AI_Content_Assistant.Clients;
using AI_Content_Assistant.Data;
using AI_Content_Assistant.Extensions;
using AI_Content_Assistant.Filters;
using AI_Content_Assistant.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});


// Services
builder.Services.AddScoped<IAiContentService, AiContentService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("AiContentDb");
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExecutionTimeFilter>();
});


// Typed HttpClient for calling Service B
builder.Services.AddHttpClient<LlmProxyClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7013/"); // Service B HTTPS port
});

var app = builder.Build();

app.UseCustomExceptionHandling();

// Swagger
//app.UseSwagger();
//app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // Generate OpenAPI JSON
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });

    // Enable Scalar UI
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
