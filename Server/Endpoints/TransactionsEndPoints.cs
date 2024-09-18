using System.Globalization;
using System.Security.Claims;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Entities;
using Shared.Models;
using static Server.Utils.Helpers;
using static Shared.Models.ServiceResponses;

public static class TransactionsEndpoints
{
    public static void MapTransactionsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/transactions/post-data", PostTransactions)
        .WithName("post-data")
        .DisableAntiforgery()
        //    .Accepts<IFormFile>("multipart/form-data")
        .Produces<GeneralResponse>(StatusCodes.Status200OK);

        app.MapGet("/api/transactions/get-transactions-currentyear", GetTransactionsInCurrentYear)
        .WithName("get transactions")
        .Produces<IEnumerable<decimal>>(StatusCodes.Status200OK);

        // Use IEnumerable<string> directly from the request body
        app.MapPost("/api/transactions/filter-transactions", FilterTransactions)
        .WithName("filter transactions")
        .Accepts<IEnumerable<string>>("application/json")
        .Produces<IEnumerable<ChaseTransactionsDTO>>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetTransactionsInCurrentYear(ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {
        var transactions = await transactionsRepository.GetTransactionsInCurrentYear(["CARDMEMBER SERV  WEB PYMT", "DISCOVER         E-PAYMENT", "Payment to Chase card "]);
        return Results.Ok(transactions);
    }

    private static async Task<IResult> FilterTransactions([FromBody] IEnumerable<string> keywords, 
            ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {
        if (keywords == null || !keywords.Any())
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: "No keywords provided."));
        }

        var filteredTransactions = await transactionsRepository.GetTransactionsByKeywordsAsync(["CARDMEMBER SERV  WEB PYMT", "DISCOVER         E-PAYMENT", "Payment to Chase card "]);

        return Results.Ok(filteredTransactions);
    }

    private static async Task<IResult> PostTransactions(
        HttpContext httpContext,
        IFormFile file,
        ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {        
        if (file is null || file.Length == 0)
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: "error in uploading file"));
        }

        // logger.LogWarning($"{uploadFileResult.ToString()}");
        // read csv file from designated location
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var streamReader = new StreamReader(file!.OpenReadStream());
        using var csvReader = new CsvReader(streamReader, config);
        csvReader.Context.RegisterClassMap<ChaseTransactionsDTOMapper>();

        Dictionary<string,string> results = new();

        try
        {
            IAsyncEnumerable<ChaseTransactionsDTO> records = records = csvReader.GetRecordsAsync<ChaseTransactionsDTO>();

            var userId = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "1";

            int i = 1;

            // add to db
            await foreach (var record in records)
            {
                record.Userid = Convert.ToInt32(userId);
                var response = await transactionsRepository.PostTransaction(record);
                results.Add($"row {i}", response.Message);
                i+=1;
            }
        }
        catch (CsvHelperException ex)
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: ex.Message));
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: ex.Message));
        }

        return Results.Ok(new GeneralResponse(Flag: true, Message: JsonConvert.SerializeObject(results, formatting: Formatting.Indented)));
    }
}