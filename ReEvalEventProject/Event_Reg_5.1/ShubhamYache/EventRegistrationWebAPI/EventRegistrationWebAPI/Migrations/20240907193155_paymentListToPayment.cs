using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventRegistrationWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class paymentListToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Registrations_RegistrationId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RegistrationId",
                table: "Payments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproveDate",
                table: "Registrations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Registrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_PaymentId",
                table: "Registrations",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Payments_PaymentId",
                table: "Registrations",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Payments_PaymentId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_PaymentId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Registrations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproveDate",
                table: "Registrations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RegistrationId",
                table: "Payments",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Registrations_RegistrationId",
                table: "Payments",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "RegistrationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
