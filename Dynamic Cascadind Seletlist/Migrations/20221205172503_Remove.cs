using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dynamic_Cascadind_Seletlist.Migrations
{
    public partial class Remove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
          name:"FK_Cities_Countries_CountryID",  
          table: "CITIES");
            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "CITIES",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn:"Id",
                onDelete:ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
              name: "FK_Cities_Countries_CountryID",
              table: "CITIES");
            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Countries_CountryID",
                table: "CITIES",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
