using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PayeeAndMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_db_UserMetaData_AspNetUsers_UserId",
                table: "db_UserMetaData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_db_UserMetaData",
                table: "db_UserMetaData");

            migrationBuilder.RenameTable(
                name: "db_UserMetaData",
                newName: "DbUserMetaData");

            migrationBuilder.RenameIndex(
                name: "IX_db_UserMetaData_UserId",
                table: "DbUserMetaData",
                newName: "IX_DbUserMetaData_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbUserMetaData",
                table: "DbUserMetaData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DbMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DbMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DbMessages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DbPayeeCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbPayeeCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbPayee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbPayee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbPayee_DbPayeeCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DbPayeeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbMessages_ReceiverId",
                table: "DbMessages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_DbMessages_SenderId",
                table: "DbMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DbMessages_UserId",
                table: "DbMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DbPayee_CategoryId",
                table: "DbPayee",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbUserMetaData_AspNetUsers_UserId",
                table: "DbUserMetaData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbUserMetaData_AspNetUsers_UserId",
                table: "DbUserMetaData");

            migrationBuilder.DropTable(
                name: "DbMessages");

            migrationBuilder.DropTable(
                name: "DbPayee");

            migrationBuilder.DropTable(
                name: "DbPayeeCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbUserMetaData",
                table: "DbUserMetaData");

            migrationBuilder.RenameTable(
                name: "DbUserMetaData",
                newName: "db_UserMetaData");

            migrationBuilder.RenameIndex(
                name: "IX_DbUserMetaData_UserId",
                table: "db_UserMetaData",
                newName: "IX_db_UserMetaData_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_db_UserMetaData",
                table: "db_UserMetaData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_db_UserMetaData_AspNetUsers_UserId",
                table: "db_UserMetaData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
