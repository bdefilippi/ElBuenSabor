using Microsoft.EntityFrameworkCore.Migrations;

namespace ElBuenSabor.Migrations
{
    public partial class FacturaPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_PedidoId",
                table: "Facturas",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Pedidos_PedidoId",
                table: "Facturas",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Pedidos_PedidoId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_PedidoId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Facturas");
        }
    }
}
