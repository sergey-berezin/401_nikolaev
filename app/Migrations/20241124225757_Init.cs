using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeneticApplication.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllPopulations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    num1 = table.Column<int>(type: "INTEGER", nullable: false),
                    num2 = table.Column<int>(type: "INTEGER", nullable: false),
                    num3 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllPopulations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllIndividuals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PopulationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllIndividuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllIndividuals_AllPopulations_PopulationId",
                        column: x => x.PopulationId,
                        principalTable: "AllPopulations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AllGens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndividualId = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Height = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllGens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllGens_AllIndividuals_IndividualId",
                        column: x => x.IndividualId,
                        principalTable: "AllIndividuals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllGens_IndividualId",
                table: "AllGens",
                column: "IndividualId");

            migrationBuilder.CreateIndex(
                name: "IX_AllIndividuals_PopulationId",
                table: "AllIndividuals",
                column: "PopulationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllGens");

            migrationBuilder.DropTable(
                name: "AllIndividuals");

            migrationBuilder.DropTable(
                name: "AllPopulations");
        }
    }
}
