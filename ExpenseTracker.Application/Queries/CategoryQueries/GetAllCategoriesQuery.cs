using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Queries.CategoryQueries;

public record GetAllCategoriesQuery() : IQuery<IEnumerable<CategoryViewDTO>>;