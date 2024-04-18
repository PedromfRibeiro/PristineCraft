using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddForgeinKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DbEventCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbEventCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbGroup_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbEventSubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbEventSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbEventSubCategory_DbEventCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DbEventCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DbEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    Attachments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbEvent_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DbEvent_DbEventCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DbEventCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DbEvent_DbEventSubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "DbEventSubCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DbEvent_DbGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DbGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DbEvent_CategoryId",
                table: "DbEvent",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DbEvent_GroupId",
                table: "DbEvent",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DbEvent_OwnerId",
                table: "DbEvent",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DbEvent_SubCategoryId",
                table: "DbEvent",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DbEventSubCategory_CategoryId",
                table: "DbEventSubCategory",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DbGroup_OwnerId",
                table: "DbGroup",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_DbGroup_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "DbGroup",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_DbGroup_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DbEvent");

            migrationBuilder.DropTable(
                name: "DbEventSubCategory");

            migrationBuilder.DropTable(
                name: "DbGroup");

            migrationBuilder.DropTable(
                name: "DbEventCategory");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AspNetUsers");
        }
    }
}
