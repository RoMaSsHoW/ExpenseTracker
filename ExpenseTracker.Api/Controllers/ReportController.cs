using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Application.Models.TransactionDTOs;
using ExpenseTracker.Application.Queries.CategoryQueries;
using ExpenseTracker.Application.Queries.TransactionQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

public class ReportController : BaseApiController
{
    public ReportController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("get-total-expenses-and-incomes-per-month")]
    public async Task<IActionResult> GetTotalExpensesAndIncomesPerMonth()
    {
        try
        {
            var query = new GetTotalExpensesAndIncomesPerMonthQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<MonthlyReportDTO>>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 500);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpGet("get-monthly-savings")]
    public async Task<IActionResult> GetMonthlySavings()
    {
        try
        {
            var query = new GetMonthlySavingQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<MonthlySavingsReportDTO>>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 500);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpGet("get-top-categories")]
    public async Task<IActionResult> GetTopCategories()
    {
        try
        {
            var query = new GetTopCategoriesQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<TopCategoryDTO>>.Success(result);
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