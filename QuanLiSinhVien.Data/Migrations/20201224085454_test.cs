using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLiSinhVien.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Person",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Person",
                table: "Teachers");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Person",
                table: "Students",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Person",
                table: "Teachers",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Person",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Person",
                table: "Teachers");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Person",
                table: "Students",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Person",
                table: "Teachers",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
