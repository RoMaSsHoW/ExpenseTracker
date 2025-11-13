using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Commands.AccountCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.AccountDTOs;
using ExpenseTracker.Application.Queries.AccountQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class AccountController : BaseApiController
{
    public AccountController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("get-user-accounts")]
    public async Task<IActionResult> GetUserAccounts()
    {
        try
        {
            var query = new GetAllAccountsQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<AccountViewDTO>>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 500);
            return StatusCode(response.StatusCode, response);
        }
    }
    
    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDTO accountDto)
    {
        try
        {
            var command = new CreateAccountCommand(accountDto);
            var result = await Mediator.Send(command);
            var response = Response<AccountViewDTO>.Success(result, 201);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPut("edit-account")]
    public async Task<IActionResult> EditAccount([FromBody] AccountEditDTO accountDto)
    {
        try
        {
            var command = new EditAccountCommand(accountDto);
            var result = await Mediator.Send(command);
            var response = Response<AccountViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpDelete("delete-account")]
    public async Task<IActionResult> DeleteAccount([FromBody] AccountDeleteDTO accountDto)
    {
        try
        {
            var command = new DeleteAccountCommand(accountDto);
            await Mediator.Send(command);
            var response = Response<object>.Success(204, "Account deleted successfully");
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors);
            return StatusCode(response.StatusCode, response);
        }
    }
}