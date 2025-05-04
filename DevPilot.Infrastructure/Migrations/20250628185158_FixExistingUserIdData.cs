using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixExistingUserIdData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set a valid default userId for existing Projects and TodoItems with empty or null UserId
            migrationBuilder.Sql("UPDATE Projects SET UserId = 'system@example.com' WHERE UserId IS NULL OR UserId = '';");
            migrationBuilder.Sql("UPDATE TodoItems SET UserId = 'system@example.com' WHERE UserId IS NULL OR UserId = '';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Optionally revert to empty string (not recommended)
            migrationBuilder.Sql("UPDATE Projects SET UserId = '' WHERE UserId = 'system@example.com';");
            migrationBuilder.Sql("UPDATE TodoItems SET UserId = '' WHERE UserId = 'system@example.com';");
        }
    }
}
