using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Shared.Models;
using static Server.Utils.Helpers;
using static Shared.Models.ServiceResponses;

public static class TransactionsEndpoints
{
    public static void MapTransactionsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/transactions/post-data", PostTransactions)
           .WithName("post-data")
           .Accepts<IFormFile>("multipart/form-data")
           .Produces<GeneralResponse>(StatusCodes.Status200OK);
    }


    private static async Task<IResult> PostTransactions(IFormFile file, ITransactionsRepository<ChaseTransactionsDTO> transactionsRepository)
    {
        if (file is null)
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

        // List<string> errors = [];
        // List<ChaseTransactionsDTO> recordsToAdd = [];
        // int rowIdx = 0;

        try
        {

            IAsyncEnumerable<ChaseTransactionsDTO> records = records = csvReader.GetRecordsAsync<ChaseTransactionsDTO>();

            // add to db


        }

        catch (CsvHelperException ex)
        {
            return Results.BadRequest(new GeneralResponse(Flag: false, Message: ex.Message));
        }

        return Results.Ok(new GeneralResponse(Flag: true, Message: "success!"));

    }
}