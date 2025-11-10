using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Application.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

public class CategoryController : BaseApiController
{
    public CategoryController(IMediator mediator) 
        : base(mediator)
    { }

    [HttpGet("get-user-categories")]
    public async Task<IActionResult> GetUserCategories()
    {
        try
        {
            var query = new GetAllCategoriesQuery();
            var result = await Mediator.Send(query);
            var response = Response<IEnumerable<CategoryViewDTO>>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var response = Response<object>.Fail(ex.Message);
            return StatusCode(response.StatusCode, response);
        }
    }
}