using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RefreshExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "0E63C20429D349EEED0C689DB2E47F1661927CFFFB8124DA456276854575367D0DED966F2C00D9D3A162B98DB8915371A880FB079C86E2AF9C04CC751A9BB9174FF0913A71ABD5CBE112EEAEC812709DB749234CF9ADDECE2937F2A1FC0BF2554E3EA3655EBFB712E0598B45C05FBF893084C7AA1ABF1F990603DD1D9B71400A");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEK4hVsHx9G6FTUDDlJaY/l1aRXqpoUZU9nkEkvECUI2uQ+FHoFYHjlJpmP3KOss/qg==");
        }
    }
}
