using ClosedXML.Excel;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;

namespace ExpenseTracker.Infrastructure.Strategies;

public class ExcelStrategy : IDocumentStrategy
{
    private const string TimeZoneId = "Central Asia Standard Time";
    private const string WorksheetName = "Transactions";
    private const string NumberFormat = "#,##0.00";
    private const string DateFormat = "yyyy-mm-dd HH:mm:ss";
    
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
    
    private static readonly Dictionary<int, double> ColumnMinWidths = new()
    {
        { 1, 20 },
        { 3, 15 },
        { 5, 25 },
        { 7, 20 }
    };
    
    public List<TransactionCreateFromDocumentDTO> ReadAsync(Stream stream)
    {
        var transactions = new List<TransactionCreateFromDocumentDTO>();
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();
        foreach (var row in worksheet.Rows().Skip(1))
        {
            var name = row.Cell(1).GetValue<string>()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;
            
            var amountIncome = row.Cell(2).GetValue<double?>();
            var amountExpense = row.Cell(3).GetValue<double?>();
            var description = row.Cell(5).GetValue<string?>();
            var category = row.Cell(6).GetValue<string?>();
            var localDate = row.Cell(7).GetValue<DateTime?>();

            DateTime? date = null;
            if (localDate.HasValue)
            {
                var localZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
                date = TimeZoneInfo.ConvertTimeToUtc(localDate.Value, localZone);
            }

            var amount = amountIncome ?? amountExpense;
            if (amount == null)
                continue;
            var typeId = amountIncome is null ? 2 : 1;
            
            transactions.Add(new TransactionCreateFromDocumentDTO()
            {
                Name = name,
                Amount = (decimal)amount.Value,
                Description = description,
                TypeId = typeId,
                Date = date ?? null,
                CategoryName = category
            });
        }
        
        return transactions;
    }

    public async Task<MemoryStream> WriteAsync(List<TransactionViewDTO> data)
    {
        var stream = new MemoryStream();
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(WorksheetName);

        CreateHeaders(worksheet);
        FillDataRows(worksheet, data);
        if(data.Count != 0)
            CreateTotalRow(worksheet, data.Count);
        AdjustColumnWidths(worksheet);

        workbook.SaveAs(stream);
        stream.Position = 0;
        
        return stream;
    }
    
    private void CreateHeaders(IXLWorksheet worksheet)
    {
        for (int i = 0; i < Headers.Length; i++)
        {
            var cell = worksheet.Cell(1, i + 1);
            cell.Value = Headers[i];
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            cell.Style.Font.Bold = true;
        }
    }
    
    private void FillDataRows(IXLWorksheet worksheet, List<TransactionViewDTO> data)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);

        for (int i = 0; i < data.Count; i++)
        {
            var rowNumber = i + 2;
            var transaction = data[i];

            FillTransactionRow(worksheet, rowNumber, transaction, timeZone);
        }
    }

    private void FillTransactionRow(IXLWorksheet worksheet, int rowNumber, TransactionViewDTO transaction, TimeZoneInfo timeZone)
    {
        worksheet.Cell(rowNumber, 1).Value = transaction.Name;

        FillAmountCells(worksheet, rowNumber, transaction);

        worksheet.Cell(rowNumber, 4).Value = transaction.Currency.Name;
        worksheet.Cell(rowNumber, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        worksheet.Cell(rowNumber, 5).Value = transaction.Description;

        worksheet.Cell(rowNumber, 6).Value = transaction.CategoryName ?? string.Empty;
        worksheet.Cell(rowNumber, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        FillDateCell(worksheet, rowNumber, transaction.Date, timeZone);
    }
    
    private void FillAmountCells(IXLWorksheet worksheet, int rowNumber, TransactionViewDTO transaction)
    {
        var columnIndex = transaction.Type == TransactionType.Income ? 2 : 3;
        var cell = worksheet.Cell(rowNumber, columnIndex);
        
        cell.Value = transaction.Amount;
        cell.Style.NumberFormat.Format = NumberFormat;
    }
    
    private void FillDateCell(IXLWorksheet worksheet, int rowNumber, DateTime date, TimeZoneInfo timeZone)
    {
        var dateInTimeZone = TimeZoneInfo.ConvertTimeFromUtc(date, timeZone);
        var cell = worksheet.Cell(rowNumber, 7);
        cell.Value = dateInTimeZone;
        cell.Style.DateFormat.Format = DateFormat;
        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
    }

    private void CreateTotalRow(IXLWorksheet worksheet, int dataCount)
    {
        var totalRow = dataCount + 2;

        CreateTotalLabel(worksheet, totalRow, dataCount);
        CreateTotalFormula(worksheet, totalRow, 2, dataCount);
        CreateTotalFormula(worksheet, totalRow, 3, dataCount);
    }

    private void CreateTotalLabel(IXLWorksheet worksheet, int totalRow, int dataCount)
    {
        var cell = worksheet.Cell(totalRow, 1);
        cell.Value = $"Итого: {dataCount}";
        cell.Style.Font.SetBold();
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
    }
    
    private void CreateTotalFormula(IXLWorksheet worksheet, int totalRow, int columnIndex, int dataCount)
    {
        var columnLetter = GetColumnLetter(columnIndex);
        var cell = worksheet.Cell(totalRow, columnIndex);
        cell.FormulaA1 = $"=SUM({columnLetter}2:{columnLetter}{dataCount + 1})";
        cell.Style.NumberFormat.Format = NumberFormat;
        cell.Style.Font.SetBold();
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }

    private void AdjustColumnWidths(IXLWorksheet worksheet)
    {
        worksheet.Columns().AdjustToContents();

        foreach (var (columnIndex, minWidth) in ColumnMinWidths)
        {
            worksheet.Column(columnIndex).Width = Math.Max(worksheet.Column(columnIndex).Width, minWidth);
        }
    }
    
    private string GetColumnLetter(int columnIndex)
    {
        return columnIndex switch
        {
            2 => "B",
            3 => "C",
            _ => throw new ArgumentException($"Unexpected column index: {columnIndex}")
        };
    }
}