using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    private const string hashedKey = "89a9ad1d0d06fd19e2dbb349f02b4d640e60027a3f51a285a9159c2d8154bd68a92dd8efa87b274ef922b6682cb2867a60bf4e03018fdf0425580f38256588ce";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"].ToString();
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        var hashedInput = Convert.ToHexString(
            SHA512.HashData(Encoding.UTF8.GetBytes(apiKeyHeader))
        ).ToLower();

        if (hashedInput != hashedKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        await next(context);
        return;
    }
}