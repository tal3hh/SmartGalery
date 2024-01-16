using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class addContextShippingAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAsdress_AspNetUsers_AppUserId",
                table: "ShippingAsdress");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAsdress_Orders_OrderId",
                table: "ShippingAsdress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingAsdress",
                table: "ShippingAsdress");

            migrationBuilder.RenameTable(
                name: "ShippingAsdress",
                newName: "ShippingAsdresses");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAsdress_OrderId",
                table: "ShippingAsdresses",
                newName: "IX_ShippingAsdresses_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAsdress_AppUserId",
                table: "ShippingAsdresses",
                newName: "IX_ShippingAsdresses_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingAsdresses",
                table: "ShippingAsdresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAsdresses_AspNetUsers_AppUserId",
                table: "ShippingAsdresses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAsdresses_Orders_OrderId",
                table: "ShippingAsdresses",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAsdresses_AspNetUsers_AppUserId",
                table: "ShippingAsdresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAsdresses_Orders_OrderId",
                table: "ShippingAsdresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingAsdresses",
                table: "ShippingAsdresses");

            migrationBuilder.RenameTable(
                name: "ShippingAsdresses",
                newName: "ShippingAsdress");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAsdresses_OrderId",
                table: "ShippingAsdress",
                newName: "IX_ShippingAsdress_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingAsdresses_AppUserId",
                table: "ShippingAsdress",
                newName: "IX_ShippingAsdress_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingAsdress",
                table: "ShippingAsdress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAsdress_AspNetUsers_AppUserId",
                table: "ShippingAsdress",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAsdress_Orders_OrderId",
                table: "ShippingAsdress",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
