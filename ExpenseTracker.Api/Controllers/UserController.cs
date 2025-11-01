using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.UserDTOs;
using ExpenseTracker.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(
        IMediator mediator,
        IUserService userService) : base(mediator)
    {
        _userService = userService;
    }

    [HttpGet("get-user")]
    
    public async Task<IActionResult> GetUser()
    {
        try
        {
            // var command = new PullProfileCommand();
            // var result = await Mediator.Send(command);
            var result = await _userService.GetUserAsync();
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