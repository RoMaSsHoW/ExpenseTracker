using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Services;

public interface IDocumentService
{
    Task<List<TransactionCreateFromDocumentDTO>> ReadAsync(IFormFile file);
    Task<MemoryStream> WriteAsync(List<TransactionViewDTO> data, DocumentExtension format);
    Task<MemoryStream> GetTemplateAsync(DocumentExtension format);
}