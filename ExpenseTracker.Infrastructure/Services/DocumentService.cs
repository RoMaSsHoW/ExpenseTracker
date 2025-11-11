using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using ExpenseTracker.Infrastructure.Strategies;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Infrastructure.Services;

public class DocumentService : IDocumentService
{
    private readonly IReadOnlyDictionary<DocumentExtension, IDocumentStrategy> _strategies;

    public DocumentService()
    {
        _strategies = new Dictionary<DocumentExtension, IDocumentStrategy>
        {
            { DocumentExtension.XLSX, new ExcelStrategy() },
            { DocumentExtension.CSV, new CsvStrategy() }
        };
    }

    public List<TransactionCreateFromDocumentDTO> ReadAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).TrimStart('.').ToUpperInvariant();;
        
        var documentExtension = Enumeration.FromName<DocumentExtension>(extension);
        if (documentExtension is null)
            throw new NotSupportedException($"The extension {extension} is not supported.");
        
        var strategy = GetStrategy(documentExtension);
        using var stream = file.OpenReadStream();
        return strategy.ReadAsync(stream);
    }  

    public async Task<MemoryStream> WriteAsync(List<TransactionViewDTO> data, DocumentExtension format)
    {
        var strategy = GetStrategy(format);
        return await strategy.WriteAsync(data);
    }

    public async Task<MemoryStream> GetTemplateAsync(DocumentExtension format)
    {
        var strategy = GetStrategy(format);
        
        var data = new List<TransactionViewDTO>();
        
        return await strategy.WriteAsync(data);
    }
    
    private IDocumentStrategy GetStrategy(DocumentExtension extension)
    {
        if (!_strategies.TryGetValue(extension, out var strategy))
            throw new NotSupportedException($"The format {extension} is not supported.");
        
        return strategy;
    }
}