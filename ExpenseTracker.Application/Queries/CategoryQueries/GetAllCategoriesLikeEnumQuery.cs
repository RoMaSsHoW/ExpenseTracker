using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Queries.CategoryQueries;

public record GetAllCategoriesLikeEnumQuery() : IQuery<IEnumerable<CategoryEnumViewDTO>>;
