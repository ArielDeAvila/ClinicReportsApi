using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicReportsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class CambioEnLaRelacionDeHospital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospitalServices");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MedicalServices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditDateCreated",
                table: "MedicalServices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 20, 16, 58, 41, 694, DateTimeKind.Local).AddTicks(9477),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 5, 16, 36, 53, 481, DateTimeKind.Local).AddTicks(6407));

            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "MedicalServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalServices_HospitalId",
                table: "MedicalServices",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hospital_MedicalService",
                table: "MedicalServices",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospital_MedicalService",
                table: "MedicalServices");

            migrationBuilder.DropIndex(
                name: "IX_MedicalServices_HospitalId",
                table: "MedicalServices");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "MedicalServices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuditDateCreated",
                table: "MedicalServices",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 5, 5, 16, 36, 53, 481, DateTimeKind.Local).AddTicks(6407),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 5, 20, 16, 58, 41, 694, DateTimeKind.Local).AddTicks(9477));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MedicalServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HospitalServices",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalServices", x => new { x.ServiceId, x.HospitalId });
                    table.ForeignKey(
                        name: "FK_Hospital_HospitalService",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Service_HospitalService",
                        column: x => x.ServiceId,
                        principalTable: "MedicalServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HospitalServices_HospitalId",
                table: "HospitalServices",
                column: "HospitalId");
        }
    }
}
