using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuimiOSHub.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExistingUsersWithPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SHA256 hash of "password"
            var defaultPasswordHash = "XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=";

            migrationBuilder.Sql($@"
                UPDATE users
                SET password_hash = '{defaultPasswordHash}'
                WHERE password_hash IS NULL OR password_hash = ''
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
