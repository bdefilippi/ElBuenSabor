using Microsoft.EntityFrameworkCore.Migrations;

namespace ElBuenSabor.Migrations
{
    public partial class DetalleFacturaDetallePedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "DetallesFacturas");



            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_DetallePedidoID",
                table: "DetallesFacturas",
                column: "DetallePedidoID");


            migrationBuilder.AddForeignKey(
                name: "FK_DetallesFacturas_DetallesPedidos_DetallePedidoID",
                table: "DetallesFacturas",
                column: "DetallePedidoID",
                principalTable: "DetallesPedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallesFacturas_Articulos_ArticuloId",
                table: "DetallesFacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesFacturas_DetallesPedidos_DetallePedidoID",
                table: "DetallesFacturas");

            migrationBuilder.DropIndex(
                name: "IX_DetallesFacturas_DetallePedidoID",
                table: "DetallesFacturas");

            migrationBuilder.DropColumn(
                name: "DetallePedidoID",
                table: "DetallesFacturas");

            migrationBuilder.RenameColumn(
                name: "ArticuloId",
                table: "DetallesFacturas",
                newName: "ArticuloID");

            migrationBuilder.RenameIndex(
                name: "IX_DetallesFacturas_ArticuloId",
                table: "DetallesFacturas",
                newName: "IX_DetallesFacturas_ArticuloID");

            migrationBuilder.AlterColumn<long>(
                name: "ArticuloID",
                table: "DetallesFacturas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "DetallesFacturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesFacturas_Articulos_ArticuloID",
                table: "DetallesFacturas",
                column: "ArticuloID",
                principalTable: "Articulos",
                principalColumn: "Id");
        }
    }
}
