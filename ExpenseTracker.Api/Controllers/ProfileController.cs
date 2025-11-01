using ExpenseTracker.Application.Commands.ProfileCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.ProfileDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class ProfileController : BaseApiController
{
    public ProfileController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("pull-profile")]
    
    public async Task<IActionResult> PullProfile()
    {
        try
        {
            var command = new PullProfileCommand();
            var result = await Mediator.Send(command);
            var response = Response<UserGetDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }
}