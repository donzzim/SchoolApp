using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UseSchoolClassTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_SchoolClasses_SchoolClassId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClasses_Course_CourseId",
                table: "SchoolClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolClasses",
                table: "SchoolClasses");

            migrationBuilder.RenameTable(
                name: "SchoolClasses",
                newName: "SchoolClass");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolClasses_CourseId",
                table: "SchoolClass",
                newName: "IX_SchoolClass_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolClass",
                table: "SchoolClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_SchoolClass_SchoolClassId",
                table: "Enrollment",
                column: "SchoolClassId",
                principalTable: "SchoolClass",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClass_Course_CourseId",
                table: "SchoolClass",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_SchoolClass_SchoolClassId",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolClass_Course_CourseId",
                table: "SchoolClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SchoolClass",
                table: "SchoolClass");

            migrationBuilder.RenameTable(
                name: "SchoolClass",
                newName: "SchoolClasses");

            migrationBuilder.RenameIndex(
                name: "IX_SchoolClass_CourseId",
                table: "SchoolClasses",
                newName: "IX_SchoolClasses_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchoolClasses",
                table: "SchoolClasses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_SchoolClasses_SchoolClassId",
                table: "Enrollment",
                column: "SchoolClassId",
                principalTable: "SchoolClasses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolClasses_Course_CourseId",
                table: "SchoolClasses",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
