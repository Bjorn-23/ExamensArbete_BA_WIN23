using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamensArbete_BA_WIN23.API.Migrations
{
    /// <inheritdoc />
    public partial class ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                columns: table => new
                {
                    ChangeRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Customer = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    FirstApprover = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondApprover = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    isSignCompleted = table.Column<bool>(type: "bit", nullable: false),
                    DateSentBV = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.ChangeRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangeRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    isDeactivated = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ChangeRequests_ChangeRequestId",
                        column: x => x.ChangeRequestId,
                        principalTable: "ChangeRequests",
                        principalColumn: "ChangeRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ChangeRequestId",
                table: "Notifications",
                column: "ChangeRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ChangeRequests");
        }
    }
}
