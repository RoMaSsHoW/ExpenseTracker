using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class change_column_require_in_categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "categories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
