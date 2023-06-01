using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicReportsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CambiosParaReportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "GenerateDate",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Patients",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifyToken",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditDateCreated",
                table: "MedicalServices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 6, 1, 14, 27, 17, 563, DateTimeKind.Local).AddTicks(1021),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 20, 16, 58, 41, 694, DateTimeKind.Local).AddTicks(9477));

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Hospitals",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifyToken",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFirstLogin",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Doctors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifyToken",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenerateDate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "VerifyToken",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "VerifyToken",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "IsFirstLogin",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "VerifyToken",
                table: "Doctors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditDateCreated",
                table: "MedicalServices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 20, 16, 58, 41, 694, DateTimeKind.Local).AddTicks(9477),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 6, 1, 14, 27, 17, 563, DateTimeKind.Local).AddTicks(1021));
        }
    }
}
