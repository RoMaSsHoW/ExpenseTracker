using ExpenseTracker.Application.Models.TransactionDTOs;

namespace ExpenseTracker.Infrastructure.Strategies;

public interface IDocumentStrategy
{
    Task<List<TransactionCreateFromDocumentDTO>> ReadAsync(Stream stream);
    Task<MemoryStream> WriteAsync(List<TransactionViewDTO> data);
}