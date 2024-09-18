using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
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

        app.MapGet("/api/transactions/get-transactions", GetTransactions)
        .WithName("get transactions")
        .Produces<IEnumerable<ChaseTransactionsDTO>>(StatusCodes.Status200OK);

        // Use IEnumerable<string> directly from the request body
        app.MapPost("/api/transactions/filter-transactions", FilterTransactions)
        .WithName("filter transactions")
        .Accepts<IEnumerable<string>>("application/json")
        .Produces<IEnumerable<ChaseTransactionsDTO>>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetTransactions(ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {
        var transactions = await transactionsRepository.GetAllTransactionsAsync();
        return Results.Ok(transactions);
    }

    private static async Task<IResult> FilterTransactions([FromBody] IEnumerable<string> keywords, 
            ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {
        if (keywords == null || !keywords.Any())
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: "No keywords provided."));
        }

        var filteredTransactions = await transactionsRepository.GetTransactionsByKeywordsAsync(keywords);

        return Results.Ok(filteredTransactions);
    }


    private static async Task<IResult> PostTransactions(IFormFile file, ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
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

        try
        {
            IAsyncEnumerable<ChaseTransactionsDTO> records = records = csvReader.GetRecordsAsync<ChaseTransactionsDTO>();

            // add to db
            await foreach (var record in records)
            {
                var response = await transactionsRepository.PostTransaction(record);

                if (!response.Flag)
                {
                    throw new Exception(response.Message);
                }
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

        return Results.Ok(new GeneralResponse(Flag: true, Message: "success!"));
    }
}