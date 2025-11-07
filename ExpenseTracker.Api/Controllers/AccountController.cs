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
            var response = Response<AccountViewDTO>.Success(result.ToList());
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message);
            return StatusCode(response.StatusCode, response);
        }
    }
    
    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] AccountCreateDTO accountDto)
    {
        try
        {
            var command = new CreateNewAccountCommand(accountDto);
            var result = await Mediator.Send(command);
            var response = Response<AccountViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message);
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
            var response = Response<object>.Fail(ex.Message);
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
            var response = Response<object>.Success();
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message);
            return StatusCode(response.StatusCode, response);
        }
    }
}