using ExpenseTracker.Api.Extentions;
using ExpenseTracker.Application.Commands.CategoryCommands;
using ExpenseTracker.Application.Models;
using ExpenseTracker.Application.Models.CategoryDTOs;
using ExpenseTracker.Application.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers;

[Authorize]
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
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors, 500);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPost("create-category")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO category)
    {
        try
        {
            var command = new CreateCategoryCommand(category);
            var result = await Mediator.Send(command);
            var response = Response<CategoryViewDTO>.Success(result, 201);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpPut("edit-category")]
    public async Task<IActionResult> EditCategory([FromBody] CategoryEditDTO category)
    {
        try
        {
            var command = new EditCategoryCommand(category);
            var result = await Mediator.Send(command);
            var response = Response<CategoryViewDTO>.Success(result);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            var allErrors = ex.GetAllMessages();
            
            var response = Response<object>.Fail(allErrors);
            return StatusCode(response.StatusCode, response);
        }
    }

    [HttpDelete("delete-category")]
    public async Task<IActionResult> DeleteCategory([FromBody] CategoryDeleteDTO category)
    {
        try
        {
            var command = new DeleteCategoryCommand(category);
            await Mediator.Send(command);
            var response = Response<bool>.Success(204, "Category deleted successfully");
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