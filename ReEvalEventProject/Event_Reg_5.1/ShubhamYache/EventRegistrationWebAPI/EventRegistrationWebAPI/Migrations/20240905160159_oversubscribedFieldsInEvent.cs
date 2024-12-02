using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class oversubscribedFieldsInEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GoldTicketsOversubscribed",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlatinumTicketsOversubscribed",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SilverTicketsOversubscribed",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoldTicketsOversubscribed",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PlatinumTicketsOversubscribed",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SilverTicketsOversubscribed",
                table: "Events");
        }
    }
}
