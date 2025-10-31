using ExpenseTracker.Application.Commands.AuthCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.AccountDTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return BadRequest(Response<object>.Fail(errors));
        }
        
        try
        {
            var command = new LoginCommand(loginDto);
            var result = await Mediator.Send(command);
            var response = Response<AuthResponse>.Success(result); 
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }
    }
    
    [HttpPost("test")]
    [Authorize(Roles = "Admin")]
    public ActionResult Test()
    {
        return Ok("hello");
    }
}