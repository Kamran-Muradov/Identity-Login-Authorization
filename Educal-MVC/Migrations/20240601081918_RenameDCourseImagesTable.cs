using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educal_MVC.Migrations
{
    public partial class RenameDCourseImagesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseImage_Courses_CourseId",
                table: "CourseImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseImage",
                table: "CourseImage");

            migrationBuilder.RenameTable(
                name: "CourseImage",
                newName: "CourseImages");

            migrationBuilder.RenameIndex(
                name: "IX_CourseImage_CourseId",
                table: "CourseImages",
                newName: "IX_CourseImages_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseImages",
                table: "CourseImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseImages_Courses_CourseId",
                table: "CourseImages",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseImages_Courses_CourseId",
                table: "CourseImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseImages",
                table: "CourseImages");

            migrationBuilder.RenameTable(
                name: "CourseImages",
                newName: "CourseImage");

            migrationBuilder.RenameIndex(
                name: "IX_CourseImages_CourseId",
                table: "CourseImage",
                newName: "IX_CourseImage_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseImage",
                table: "CourseImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseImage_Courses_CourseId",
                table: "CourseImage",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
