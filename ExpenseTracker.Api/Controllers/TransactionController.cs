using ExpenseTracker.Application.Commands.TransactionCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Application.Queries.TransactionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class TransactionController : BaseApiController
{
    public TransactionController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("get-all-account-transactions")]
    public async Task<IActionResult> GetAllTransactions([FromQuery] TransactionFilterForGetAll filter)
    {
        try
        {
            var query = new GetAllTransactionsQuery(filter);
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
    public async Task<IActionResult> DeleteTransactions([FromQuery] TransactionDeleteDTO transaction)
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
}