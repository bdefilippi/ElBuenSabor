using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElBuenSabor.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuraciones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadCocineros = table.Column<int>(type: "int", nullable: false),
                    EmailEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenMercadoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuraciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Domicilios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domicilios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoDescuento = table.Column<double>(type: "float", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NroTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalVenta = table.Column<double>(type: "float", nullable: false),
                    TotalCosto = table.Column<double>(type: "float", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MercadoPagoDatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificadorPago = table.Column<long>(type: "bigint", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetodoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NroTarjeta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MercadoPagoDatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RubrosArticulos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdRubroPadre = table.Column<long>(type: "bigint", nullable: false),
                    RubroPadreId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubrosArticulos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RubrosArticulos_RubrosArticulos_RubroPadreId",
                        column: x => x.RubroPadreId,
                        principalTable: "RubrosArticulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RubrosGenerales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubrosGenerales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticulosInsumo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioCompra = table.Column<double>(type: "float", nullable: false),
                    PrecioVenta = table.Column<double>(type: "float", nullable: false),
                    StockActual = table.Column<double>(type: "float", nullable: false),
                    StockMinimo = table.Column<double>(type: "float", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EsInsumo = table.Column<bool>(type: "bit", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdRubroArticulo = table.Column<long>(type: "bigint", nullable: false),
                    RubroArticuloId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosInsumo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosInsumo_RubrosArticulos_RubroArticuloId",
                        column: x => x.RubroArticuloId,
                        principalTable: "RubrosArticulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticulosManufacturados",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiempoEstimadoCocina = table.Column<int>(type: "int", nullable: false),
                    Denominacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioVenta = table.Column<double>(type: "float", nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdRubroGeneral = table.Column<long>(type: "bigint", nullable: false),
                    RubroGeneralId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosManufacturados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturados_RubrosGenerales_RubroGeneralId",
                        column: x => x.RubroGeneralId,
                        principalTable: "RubrosGenerales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: true),
                    IdDomicilio = table.Column<long>(type: "bigint", nullable: false),
                    DomicilioId = table.Column<long>(type: "bigint", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Domicilios_DomicilioId",
                        column: x => x.DomicilioId,
                        principalTable: "Domicilios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticulosManufacturadosDetalles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<double>(type: "float", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdArticuloManufacturado = table.Column<long>(type: "bigint", nullable: false),
                    IdArticuloInsumo = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosManufacturadosDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturadosDetalles_ArticulosInsumo_ArticuloInsumoId",
                        column: x => x.ArticuloInsumoId,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturadosDetalles_ArticulosManufacturados_IdArticuloManufacturado",
                        column: x => x.IdArticuloManufacturado,
                        principalTable: "ArticulosManufacturados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetallesFacturas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdFactura = table.Column<long>(type: "bigint", nullable: false),
                    IdArticuloManufacturado = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloManufacturadoId = table.Column<long>(type: "bigint", nullable: true),
                    IdArticuloInsumo = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_ArticulosInsumo_ArticuloInsumoId",
                        column: x => x.ArticuloInsumoId,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_ArticulosManufacturados_ArticuloManufacturadoId",
                        column: x => x.ArticuloManufacturadoId,
                        principalTable: "ArticulosManufacturados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_Facturas_IdFactura",
                        column: x => x.IdFactura,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    HoraEstimadaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoEnvio = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    IdCliente = table.Column<long>(type: "bigint", nullable: false),
                    ClienteId = table.Column<long>(type: "bigint", nullable: true),
                    IdDomicilio = table.Column<long>(type: "bigint", nullable: false),
                    DomicilioId = table.Column<long>(type: "bigint", nullable: true),
                    IdMercadoPagoDatos = table.Column<long>(type: "bigint", nullable: false),
                    MercadoPagoDatosId = table.Column<long>(type: "bigint", nullable: true),
                    IdFactura = table.Column<long>(type: "bigint", nullable: false),
                    FacturaId = table.Column<long>(type: "bigint", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Domicilios_DomicilioId",
                        column: x => x.DomicilioId,
                        principalTable: "Domicilios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_MercadoPagoDatos_MercadoPagoDatosId",
                        column: x => x.MercadoPagoDatosId,
                        principalTable: "MercadoPagoDatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetallesPedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    IdPedido = table.Column<long>(type: "bigint", nullable: false),
                    IdArticuloManufacturado = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloManufacturadoId = table.Column<long>(type: "bigint", nullable: true),
                    IdArticuloInsumo = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_ArticulosInsumo_ArticuloInsumoId",
                        column: x => x.ArticuloInsumoId,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_ArticulosManufacturados_ArticuloManufacturadoId",
                        column: x => x.ArticuloManufacturadoId,
                        principalTable: "ArticulosManufacturados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosInsumo_RubroArticuloId",
                table: "ArticulosInsumo",
                column: "RubroArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturados_RubroGeneralId",
                table: "ArticulosManufacturados",
                column: "RubroGeneralId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturadosDetalles_ArticuloInsumoId",
                table: "ArticulosManufacturadosDetalles",
                column: "ArticuloInsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturadosDetalles_IdArticuloManufacturado",
                table: "ArticulosManufacturadosDetalles",
                column: "IdArticuloManufacturado");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_DomicilioId",
                table: "Clientes",
                column: "DomicilioId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Clientes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_ArticuloInsumoId",
                table: "DetallesFacturas",
                column: "ArticuloInsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_ArticuloManufacturadoId",
                table: "DetallesFacturas",
                column: "ArticuloManufacturadoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_IdFactura",
                table: "DetallesFacturas",
                column: "IdFactura");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_ArticuloInsumoId",
                table: "DetallesPedidos",
                column: "ArticuloInsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_ArticuloManufacturadoId",
                table: "DetallesPedidos",
                column: "ArticuloManufacturadoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_IdPedido",
                table: "DetallesPedidos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DomicilioId",
                table: "Pedidos",
                column: "DomicilioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_FacturaId",
                table: "Pedidos",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_MercadoPagoDatosId",
                table: "Pedidos",
                column: "MercadoPagoDatosId");

            migrationBuilder.CreateIndex(
                name: "IX_RubrosArticulos_RubroPadreId",
                table: "RubrosArticulos",
                column: "RubroPadreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticulosManufacturadosDetalles");

            migrationBuilder.DropTable(
                name: "Configuraciones");

            migrationBuilder.DropTable(
                name: "DetallesFacturas");

            migrationBuilder.DropTable(
                name: "DetallesPedidos");

            migrationBuilder.DropTable(
                name: "ArticulosInsumo");

            migrationBuilder.DropTable(
                name: "ArticulosManufacturados");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "RubrosArticulos");

            migrationBuilder.DropTable(
                name: "RubrosGenerales");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "MercadoPagoDatos");

            migrationBuilder.DropTable(
                name: "Domicilios");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
