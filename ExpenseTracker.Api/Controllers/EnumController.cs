using ExpenseTracker.Application.Models;
using ExpenseTracker.Domain.AccountAggregate.ValueObjects;
using ExpenseTracker.Domain.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class EnumController : BaseApiController
{
    public EnumController(IMediator mediator)
        : base(mediator)
    { }

    [HttpGet("get-currency")]
    public IActionResult GetCurrency()
    {
        var result = Enumeration.GetAll<Currency>();
        var response = Response<IEnumerable<Currency>>.Success(result);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-transaction-type")]
    public IActionResult GetTransactionType()
    {
        var result = Enumeration.GetAll<TransactionType>();
        var response = Response<IEnumerable<TransactionType>>.Success(result);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-recurring-frequency")]
    public IActionResult GetRecurringFrequency()
    {
        var result = Enumeration.GetAll<RecurringFrequency>();
        var response = Response<IEnumerable<RecurringFrequency>>.Success(result);
        return StatusCode(response.StatusCode, response);
    }
}