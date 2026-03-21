using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddDbContext<ProjectApi.Persistence.VsaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Vertical Slice Architechture API";
        document.Info.Version = "v1";
        document.Info.Description = "Vertical Slice Architechture, DDD, ArgoCD and CI/CD Pipeline implemantation";

        document.Info.Contact = new()
        {
            Name = "Alihan Berat Çelik",
            Email = "alihancelik@gmail.com"
        };

        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Vertical Slice API")
        .WithTheme(ScalarTheme.Default)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
