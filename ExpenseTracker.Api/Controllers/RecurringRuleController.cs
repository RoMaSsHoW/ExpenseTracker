using ExpenseTracker.Application.Commands.RecurringRuleCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.RecurringRuleDTOs;
using ExpenseTracker.Application.Queries.RecurringRuleQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

public class RecurringRuleController : BaseApiController
{
    public RecurringRuleController(IMediator mediator)
        : base(mediator)
    { }

    [HttpGet("get-all-account-recurring_rules")]
    public async Task<IActionResult> GetAllRecurringRules([FromQuery] FilterForGetAllRecurringRule filter)
    {
        try
        {
            var query = new GetAllRecurringRulesQuery(filter);
            var result = await Mediator.Send(query);
            var response = Response<PaginatedRecurringRulesDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPost("create-recurring-rule")]
    public async Task<IActionResult> CreateRecurringRule([FromBody] RecurringRuleCreateDTO recurringRule)
    {
        try
        {
            var command = new CreateRecurringRuleCommand(recurringRule);
            var result = await Mediator.Send(command);
            var response = Response<RecurringRuleViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPut("edit-recurring-rule")]
    public async Task<IActionResult> EditRecurringRule([FromBody] RecurringRuleEditDTO recurringRule)
    {
        try
        {
            var command = new EditRecurringRuleCommand(recurringRule);
            var result = await Mediator.Send(command);
            var response = Response<RecurringRuleViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpDelete("delete-recurring-rules")]
    public async Task<IActionResult> DeleteRecurringRules([FromBody] RecurringRuleDeleteDTO recurringRule)
    {
        try
        {
            var command = new DeleteRecurringRuleCommand(recurringRule);
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