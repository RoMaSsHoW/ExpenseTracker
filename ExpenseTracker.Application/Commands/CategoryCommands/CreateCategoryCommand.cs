using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Commands.CategoryCommands;

public record CreateCategoryCommand(CategoryCreateDTO CategoryDto) : ICommand<CategoryViewDTO>;
