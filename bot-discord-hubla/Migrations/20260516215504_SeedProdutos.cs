using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bot_discord_hubla.Migrations
{
    /// <inheritdoc />
    public partial class SeedProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "HublaProductId", "RoleName" },
                values: new object[,]
                {
                    { 1, "xwTmekj6quTSGZs3Q77v", "aluno" },     // Currículo
                    { 2, "3HB3Zj6PUGOX8fhRq49C", "aluno" },     // Projetos Fullstack
                    { 3, "PD3CxaDsxJnXhaSUhb30", "aluno" },     // Entrevistas
                    { 4, "8vFmWBIx4mZXRWzBHPb7", "aluno" },     // Linkedin
                    { 5, "Ls0PAQJv0B1txTSwELxf", "aluno" },     // Pack
                    { 6, "HakeQ4Vk06Yc0ucgoB1b", "Mentorado" }  // Mentoria
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 1);
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 2);
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 3);
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 4);
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 5);
            migrationBuilder.DeleteData(table: "Produtos", keyColumn: "Id", keyValue: 6);
        }
    }
}
