using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstGear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vehicleType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "BrandLogo",
            //    table: "Brand",
            //    type: "nvarchar(max)",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AddColumn<string>(
            //    name: "CreatedBy",
            //    table: "Brand",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CreatedOn",
            //    table: "Brand",
            //    type: "datetime2",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "ModifiedBy",
            //    table: "Brand",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "ModifiedOn",
            //    table: "Brand",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "VehicleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleType", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleType");

            //migrationBuilder.DropColumn(
            //    name: "CreatedBy",
            //    table: "Brand");

            //migrationBuilder.DropColumn(
            //    name: "CreatedOn",
            //    table: "Brand");

            //migrationBuilder.DropColumn(
            //    name: "ModifiedBy",
            //    table: "Brand");

            //migrationBuilder.DropColumn(
            //    name: "ModifiedOn",
            //    table: "Brand");

            //migrationBuilder.AlterColumn<string>(
            //    name: "BrandLogo",
            //    table: "Brand",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    defaultValue: "",
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)",
            //    oldNullable: true);
        }
    }
}
