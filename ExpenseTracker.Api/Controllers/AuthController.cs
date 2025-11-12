using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Commands.AuthCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.AuthDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var command = new RegistrationCommand(registerDto);
            var result = await Mediator.Send(command);
            var response = Response<AuthResponse>.Success(result, "Success", 201);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 401);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshAccessTiken([FromBody] string refreshToken)
    {
        try
        {
            var command = new RefreshAccessTokenCommand(refreshToken);
            var result = await Mediator.Send(command);
            var response = Response<AuthResponse>.Success(result, "Success", 201);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message, 401);
            return StatusCode(response.StatusCode, response);
        }        
    }
}