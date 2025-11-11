using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Infrastructure.Strategies;

public class CsvStrategy : IDocumentStrategy
{
    private const string TimeZoneId = "Central Asia Standard Time";
    private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

    private static readonly string[] Headers =
    {
        "Name",
        "Amount income",
        "Amount expense",
        "Currency",
        "Description",
        "Category",
        "Date"
    };
    
    public List<TransactionCreateFromDocumentDTO> ReadAsync(Stream stream)
    {
        var transactions = new List<TransactionCreateFromDocumentDTO>();
        
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null
        };
        
        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var name = csv.GetField<string>(0)?.Trim();
            if (string.IsNullOrWhiteSpace(name) || name.StartsWith("Итого"))
                continue;

            var amountIncome = csv.GetField<double?>(1);
            var amountExpense = csv.GetField<double?>(2);
            var description = csv.GetField<string?>(4);
            var category = csv.GetField<string?>(5);
            var dateString = csv.GetField<string?>(6);

            DateTime? date = null;
            if (!string.IsNullOrWhiteSpace(dateString) && 
                DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out var localDate))
            {
                var localZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                date = TimeZoneInfo.ConvertTimeToUtc(localDate, localZone);
            }

            var amount = amountIncome ?? amountExpense;
            if (amount == null)
                continue;

            var typeId = amountIncome is null ? 2 : 1;

            transactions.Add(new TransactionCreateFromDocumentDTO
            {
                Name = name,
                Amount = (decimal)amount.Value,
                Description = description,
                TypeId = typeId,
                Date = date,
                CategoryName = category
            });
        }

        return transactions;
    }

    public async Task<MemoryStream> WriteAsync(List<TransactionViewDTO> data)
    {
        var stream = new MemoryStream();
        await using var writer = new StreamWriter(stream, leaveOpen: true);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        await using var csv = new CsvWriter(writer, config);

        // Write headers
        csv.WriteField("Name");
        csv.WriteField("Amount income");
        csv.WriteField("Amount expense");
        csv.WriteField("Currency");
        csv.WriteField("Description");
        csv.WriteField("Category");
        csv.WriteField("Date");
        await csv.NextRecordAsync();

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

        // Write data rows
        foreach (var transaction in data)
        {
            csv.WriteField(transaction.Name);

            if (transaction.Type == TransactionType.Income)
            {
                csv.WriteField(transaction.Amount.ToString("F2", CultureInfo.InvariantCulture));
                csv.WriteField("");
            }
            else
            {
                csv.WriteField("");
                csv.WriteField(transaction.Amount.ToString("F2", CultureInfo.InvariantCulture));
            }

            csv.WriteField(transaction.Currency.Name);
            csv.WriteField(transaction.Description ?? "");
            csv.WriteField(transaction.CategoryName ?? "");

            var dateInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(transaction.Date, timeZone);
            csv.WriteField(dateInTimeZone.ToString(DateFormat, CultureInfo.InvariantCulture));

            await csv.NextRecordAsync();
        }

        // Write total row
        if (data.Count != 0)
        {
            csv.WriteField($"Итого: {data.Count}");

            var totalIncome = data.Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);
            var totalExpense = data.Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            csv.WriteField(totalIncome.ToString("F2", CultureInfo.InvariantCulture));
            csv.WriteField(totalExpense.ToString("F2", CultureInfo.InvariantCulture));
            csv.WriteField("");
            csv.WriteField("");
            csv.WriteField("");
            csv.WriteField("");

            await csv.NextRecordAsync();
        }

        await writer.FlushAsync();
        stream.Position = 0;
        return stream;
    }
}