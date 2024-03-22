using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagement.Migrations
{
    public partial class UpdateEnrollmentsFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "enrollment",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "enrollment",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8bb960a2-21f3-4022-9046-0e8dc6bafebd", "3", "Guest", "Guest" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "acf9aa15-a8ce-4725-be74-81d564e20087", "1", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cc4c2589-355a-40a8-b97e-d2bd7671f1f3", "2", "Member", "Member" });

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_UserId",
                table: "enrollment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_enrollment_AspNetUsers_UserId",
                table: "enrollment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enrollment_AspNetUsers_UserId",
                table: "enrollment");

            migrationBuilder.DropIndex(
                name: "IX_enrollment_UserId",
                table: "enrollment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bb960a2-21f3-4022-9046-0e8dc6bafebd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acf9aa15-a8ce-4725-be74-81d564e20087");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc4c2589-355a-40a8-b97e-d2bd7671f1f3");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "enrollment",
                newName: "EnrollmentId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "enrollment",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
        }
    }
}
