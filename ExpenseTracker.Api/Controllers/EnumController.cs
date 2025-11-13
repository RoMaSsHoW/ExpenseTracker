using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Application.Queries.CategoryQueries;
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

    [HttpGet("get-document-extension")]
    public IActionResult GetDocumentExtension()
    {
        var result = Enumeration.GetAll<DocumentExtension>();
        var response = Response<IEnumerable<DocumentExtension>>.Success(result);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("get-categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var query = new GetAllCategoriesLikeEnumQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<CategoryEnumViewDTO>>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 500);
            return StatusCode(response.StatusCode, response);
        }
    }
}