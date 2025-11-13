using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.UserDTOs;
using ExpenseTracker.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
public class UserController : BaseApiController
{
    public UserController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("get-user")]
    
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var query = new GetUserQuery();
            var result = await Mediator.Send(query);
            var response = Response<UserViewDTO>.Success(result);
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