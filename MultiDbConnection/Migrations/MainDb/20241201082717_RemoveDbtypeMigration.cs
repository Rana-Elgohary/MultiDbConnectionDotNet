using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiDbConnection.Migrations.MainDb
{
    /// <inheritdoc />
    public partial class RemoveDbtypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DbType",
                table: "Domains");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DbType",
                table: "Domains",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
