using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class addingApproveDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guests",
                table: "Events",
                newName: "Description");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "Registrations",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "Registrations");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Events",
                newName: "Guests");
        }
    }
}
