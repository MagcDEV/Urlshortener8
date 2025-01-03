using Microsoft.EntityFrameworkCore;
using Urlshortener.Api;
using Urlshortener.Api.Entities;
using Urlshortener.Api.Models;
using Urlshortener.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var useLocalConnection = builder.Configuration.GetValue<bool>("UseLocalConnection");
var connectionString = useLocalConnection 
    ? builder.Configuration.GetConnectionString("LocalConnection") 
    : builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(o => 
    o.UseSqlServer(connectionString));
builder.Services.AddScoped<UrlShorteningService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShorteningService urlShorteningService,
    ApplicationDbContext dbContext,
    HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Invalid URL");
    }
    
    var code = await urlShorteningService.GenerateUniqueCode();
    
    var shortenedUrl = new ShortenedUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}" ,
        CreatedAtUtc = DateTime.Now
    };
    
    await dbContext.ShortenedUrls.AddAsync(shortenedUrl);
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});

app.MapGet("api/{code}", async (
    string code,
    UrlShorteningService urlShorteningService) =>
{
    var url = await urlShorteningService.GetUrlByCode(code);
    
    return url == null ? Results.NotFound() : Results.Redirect(url);
});


app.Run();
