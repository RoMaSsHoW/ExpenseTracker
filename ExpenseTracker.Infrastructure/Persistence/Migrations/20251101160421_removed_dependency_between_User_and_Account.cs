using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class removed_dependency_between_User_and_Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_users_user_Id",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_user_Id",
                table: "accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_accounts_user_Id",
                table: "accounts",
                column: "user_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_users_user_Id",
                table: "accounts",
                column: "user_Id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
