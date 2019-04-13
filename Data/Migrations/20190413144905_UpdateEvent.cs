using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkOutTogether.Data.Migrations
{
    public partial class UpdateEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentPeopleNumber",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HowManyPeople",
                table: "Event",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "Event",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "Event",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPeopleNumber",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "HowManyPeople",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Event");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "Event");
        }
    }
}
