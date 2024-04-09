using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingFPTCo.Migrations
{
    /// <inheritdoc />
    public partial class Topics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    NameTopic = table.Column<string>(type: "Varchar(200)", nullable: false),
                    Description = table.Column<string>(type: "Varchar(200)", nullable: true),
                    Status = table.Column<string>(type: "Varchar(200)", nullable: false),
                    Documents = table.Column<string>(type: "Varchar(MAX)", nullable: false),
                    AttachFile = table.Column<string>(type: "Varchar(MAX)", nullable: false),
                    PoterTopic = table.Column<string>(type: "Varchar(50)", nullable: false),
                    TypeDocument = table.Column<string>(type: "Varchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
