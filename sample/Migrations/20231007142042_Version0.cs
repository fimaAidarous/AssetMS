using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sample.Migrations
{
    /// <inheritdoc />
    public partial class Version0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assetMaintenances_Assets_AssetId",
                table: "assetMaintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assetMaintenances",
                table: "assetMaintenances");

            migrationBuilder.RenameTable(
                name: "assetMaintenances",
                newName: "AssetMaintenances");

            migrationBuilder.RenameIndex(
                name: "IX_assetMaintenances_AssetId",
                table: "AssetMaintenances",
                newName: "IX_AssetMaintenances_AssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssetMaintenances",
                table: "AssetMaintenances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetMaintenances_Assets_AssetId",
                table: "AssetMaintenances",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetMaintenances_Assets_AssetId",
                table: "AssetMaintenances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssetMaintenances",
                table: "AssetMaintenances");

            migrationBuilder.RenameTable(
                name: "AssetMaintenances",
                newName: "assetMaintenances");

            migrationBuilder.RenameIndex(
                name: "IX_AssetMaintenances_AssetId",
                table: "assetMaintenances",
                newName: "IX_assetMaintenances_AssetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assetMaintenances",
                table: "assetMaintenances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_assetMaintenances_Assets_AssetId",
                table: "assetMaintenances",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
