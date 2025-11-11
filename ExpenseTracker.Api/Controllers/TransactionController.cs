using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Commands.TransactionCommands;
using ExpenseTracker.Application.Common.Services;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Application.Queries.TransactionQueries;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class TransactionController : BaseApiController
{
    private readonly IDocumentService _documentService;

    public TransactionController(
        IDocumentService documentService,
        IMediator mediator)
        : base(mediator)
    {
        _documentService = documentService;
    }

    [HttpGet("get-all-account-transactions")]
    public async Task<IActionResult> GetAllTransactions([FromQuery] FilterForGetAllTransaction filter)
    {
        try
        {
            var query = new GetAllPaginatedTransactionsQuery(filter);
            var result = await Mediator.Send(query);
            var response = Response<PaginatedTransactionsDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPost("create-transaction")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDTO transaction)
    {
        try
        {
            var command = new CreateTransactionCommand(transaction);
            var result = await Mediator.Send(command);
            var response = Response<TransactionViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPut("edit-transaction")]
    public async Task<IActionResult> EditTransaction([FromBody] TransactionEditDTO transaction)
    {
        try
        {
            var command = new EditTransactionCommand(transaction);
            var result = await Mediator.Send(command);
            var response = Response<TransactionViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpDelete("delete-transactions")]
    public async Task<IActionResult> DeleteTransactions([FromBody] TransactionDeleteDTO transaction)
    {
        try
        {
            var command = new DeleteTransactionCommand(transaction);
            await Mediator.Send(command);
            var response = Response<object>.Success();
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpGet("export-transactions")]
    public async Task<IActionResult> ExportTransactions(
        [FromQuery] FilterForGetAllTransactionInDocument filter,
        [FromQuery] int DocumentExtensionId = 1)
    {
        try
        {   
            var query = new GetAllTransactionsQuery(filter);
            var result = await Mediator.Send(query);

            var format = Enumeration.FromId<DocumentExtension>(DocumentExtensionId);
            var stream = await _documentService.WriteAsync(result.ToList(), format);
            
            var contentType = format.Name == DocumentExtension.CSV.Name ? "text/csv" : 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            
            var fileName = format.Name == DocumentExtension.CSV.Name ? "Transactions.csv" : "Transactions.xlsx";

            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }        
    }

    [HttpGet("get-import-template")]
    public async Task<IActionResult> GetImportTemplate([FromQuery] int DocumentExtensionId = 1)
    {
        try
        {
            var format = Enumeration.FromId<DocumentExtension>(DocumentExtensionId);
            var stream = await _documentService.GetTemplateAsync(format);
            
            var contentType = format.Name == DocumentExtension.CSV.Name ? "text/csv" : 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            
            var fileName = format.Name == DocumentExtension.CSV.Name ? "Template.csv" : "Template.xlsx";

            return File(stream, contentType, fileName);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }        
    }

    [HttpPost("import-transactions")]
    public async Task<IActionResult> ImportTransactions(IFormFile file)
    {
        try
        {
            var command = new CreateTransactionsFromFileCommand(file);
            await Mediator.Send(command);
            var response = Response<object>.Success();
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 401);
            return StatusCode(response.StatusCode, response);
        }
    }
}