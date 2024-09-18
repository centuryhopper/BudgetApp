


using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using static Shared.Models.ServiceResponses;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this WebApplication app)
    {
        app.MapGet("/api/account/nlog-test", TestLogging)
           .WithName("TestLogging")
           .Produces<string>(StatusCodes.Status200OK);

        app.MapPost("/api/account/login", Login)
           .WithName("Login")
           .Produces<LoginResponse>(StatusCodes.Status200OK);
    }

    private static IResult TestLogging([FromServices] ILogger<string> logger)
    {
        logger.LogWarning("warning!");
        logger.LogInformation("information!");
        logger.LogError("error!");
        logger.LogCritical("critical");
        logger.LogDebug("debug");

        return Results.Ok("Logging test completed. Check your PostgreSQL LOGS table.");
    }

    private static async Task<IResult> Login(LoginDTO loginDTO, IAccountRepository accountRepository)
    {
        var response = await accountRepository.LoginAccount(loginDTO);
        return Results.Ok(response);
    }
}
