using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagement.Migrations
{
    public partial class UpdateEnrollmentsTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_course_CourseId",
                table: "Enrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ccc1d95-270c-4e0b-aff8-a0aa267b88fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5ef5d90-30cc-48a2-b36f-2ff66e46ca2e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb41c873-3cfb-4884-8ac0-e16a72c72e1c");

            migrationBuilder.RenameTable(
                name: "Enrollments",
                newName: "enrollment");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollments_CourseId",
                table: "enrollment",
                newName: "IX_enrollment_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_enrollment",
                table: "enrollment",
                column: "EnrollmentId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1f1ffbef-3dd9-4b6a-b33d-d8b06b64bb3d", "2", "Member", "Member" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3994a65-92e1-4937-ac5b-906ec1895109", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bf82b172-13ed-45f4-93e0-44914beeaea2", "3", "Guest", "Guest" });

            migrationBuilder.AddForeignKey(
                name: "FK_enrollment_course_CourseId",
                table: "enrollment",
                column: "CourseId",
                principalTable: "course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enrollment_course_CourseId",
                table: "enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_enrollment",
                table: "enrollment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f1ffbef-3dd9-4b6a-b33d-d8b06b64bb3d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3994a65-92e1-4937-ac5b-906ec1895109");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf82b172-13ed-45f4-93e0-44914beeaea2");

            migrationBuilder.RenameTable(
                name: "enrollment",
                newName: "Enrollments");

            migrationBuilder.RenameIndex(
                name: "IX_enrollment_CourseId",
                table: "Enrollments",
                newName: "IX_Enrollments_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enrollments",
                table: "Enrollments",
                column: "EnrollmentId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6ccc1d95-270c-4e0b-aff8-a0aa267b88fd", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a5ef5d90-30cc-48a2-b36f-2ff66e46ca2e", "3", "Guest", "Guest" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bb41c873-3cfb-4884-8ac0-e16a72c72e1c", "2", "Member", "Member" });

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_course_CourseId",
                table: "Enrollments",
                column: "CourseId",
                principalTable: "course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
