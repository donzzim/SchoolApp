using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Infra.Data.Migrations
{
    public partial class RenameClassToSchoolClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Enrollment_Classes_ClassId", table: "Enrollment");
            migrationBuilder.DropForeignKey(name: "FK_Classes_Course_CourseId", table: "Classes");
            migrationBuilder.DropPrimaryKey(name: "PK_Classes", table: "Classes");
            migrationBuilder.RenameTable(name: "Classes", newName: "SchoolClasses");
            migrationBuilder.RenameColumn(name: "ClassId", table: "Enrollment", newName: "SchoolClassId");
            migrationBuilder.RenameIndex(name: "IX_Enrollment_ClassId", table: "Enrollment", newName: "IX_Enrollment_SchoolClassId");
            migrationBuilder.RenameIndex(name: "IX_Classes_CourseId", table: "SchoolClasses", newName: "IX_SchoolClasses_CourseId");
            migrationBuilder.AddPrimaryKey(name: "PK_SchoolClasses", table: "SchoolClasses", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_SchoolClasses_Course_CourseId", table: "SchoolClasses",
                column: "CourseId", principalTable: "Course", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Enrollment_SchoolClasses_SchoolClassId", table: "Enrollment",
                column: "SchoolClassId", principalTable: "SchoolClasses", principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Enrollment_SchoolClasses_SchoolClassId", table: "Enrollment");
            migrationBuilder.DropForeignKey(name: "FK_SchoolClasses_Course_CourseId", table: "SchoolClasses");
            migrationBuilder.DropPrimaryKey(name: "PK_SchoolClasses", table: "SchoolClasses");
            migrationBuilder.RenameTable(name: "SchoolClasses", newName: "Classes");
            migrationBuilder.RenameColumn(name: "SchoolClassId", table: "Enrollment", newName: "ClassId");
            migrationBuilder.RenameIndex(name: "IX_Enrollment_SchoolClassId", table: "Enrollment", newName: "IX_Enrollment_ClassId");
            migrationBuilder.RenameIndex(name: "IX_SchoolClasses_CourseId", table: "Classes", newName: "IX_Classes_CourseId");
            migrationBuilder.AddPrimaryKey(name: "PK_Classes", table: "Classes", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Classes_Course_CourseId", table: "Classes",
                column: "CourseId", principalTable: "Course", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_Enrollment_Classes_ClassId", table: "Enrollment",
                column: "ClassId", principalTable: "Classes", principalColumn: "Id");
        }
    }
}
