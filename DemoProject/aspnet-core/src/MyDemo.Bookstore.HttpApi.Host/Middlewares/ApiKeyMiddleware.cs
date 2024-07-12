using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyDemo.BookStore.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace MyDemo.BookStore.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration["ApiSettings:ApiKey"];
    }

    public async Task InvokeAsync(HttpContext context, BookStoreDbContext dbContext)
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/swagger") || path.StartsWithSegments("/api-docs") || path.StartsWithSegments("/api/abp/api-definition"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("BookStore-Api-Key", out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        
        if (_apiKey == null || _apiKey != extractedApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}
