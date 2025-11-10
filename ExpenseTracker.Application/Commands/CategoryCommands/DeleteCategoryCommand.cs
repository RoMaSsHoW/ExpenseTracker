using ExpenseTracker.Application.Common.Messaging;
using ExpenseTracker.Application.Models.CategoryDTOs;

namespace ExpenseTracker.Application.Commands.CategoryCommands;

public record DeleteCategoryCommand(CategoryDeleteDTO CategoryDto) 
    : ICommand;