using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElBuenSabor.Migrations
{
    public partial class InitialCreation : Migration
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
                    RubroPadreID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RubrosArticulos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RubrosArticulos_RubrosArticulos_RubroPadreID",
                        column: x => x.RubroPadreID,
                        principalTable: "RubrosArticulos",
                        principalColumn: "Id");
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
                    RubroArticuloID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosInsumo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosInsumo_RubrosArticulos_RubroArticuloID",
                        column: x => x.RubroArticuloID,
                        principalTable: "RubrosArticulos",
                        principalColumn: "Id");
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
                    RubroGeneralID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosManufacturados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturados_RubrosGenerales_RubroGeneralID",
                        column: x => x.RubroGeneralID,
                        principalTable: "RubrosGenerales",
                        principalColumn: "Id");
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
                    UsuarioID = table.Column<long>(type: "bigint", nullable: false),
                    DomicilioID = table.Column<long>(type: "bigint", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_Domicilios_DomicilioID",
                        column: x => x.DomicilioID,
                        principalTable: "Domicilios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clientes_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ArticuloManufacturadoID = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticulosManufacturadosDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturadosDetalles_ArticulosInsumo_ArticuloInsumoID",
                        column: x => x.ArticuloInsumoID,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticulosManufacturadosDetalles_ArticulosManufacturados_ArticuloManufacturadoID",
                        column: x => x.ArticuloManufacturadoID,
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
                    FacturaID = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloManufacturadoID = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesFacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_ArticulosInsumo_ArticuloInsumoID",
                        column: x => x.ArticuloInsumoID,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_ArticulosManufacturados_ArticuloManufacturadoID",
                        column: x => x.ArticuloManufacturadoID,
                        principalTable: "ArticulosManufacturados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesFacturas_Facturas_FacturaID",
                        column: x => x.FacturaID,
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
                    ClienteID = table.Column<long>(type: "bigint", nullable: false),
                    DomicilioID = table.Column<long>(type: "bigint", nullable: false),
                    MercadoPagoDatosID = table.Column<long>(type: "bigint", nullable: false),
                    FacturaID = table.Column<long>(type: "bigint", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteID",
                        column: x => x.ClienteID,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Domicilios_DomicilioID",
                        column: x => x.DomicilioID,
                        principalTable: "Domicilios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Facturas_FacturaID",
                        column: x => x.FacturaID,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_MercadoPagoDatos_MercadoPagoDatosID",
                        column: x => x.MercadoPagoDatosID,
                        principalTable: "MercadoPagoDatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    PedidoID = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloManufacturadoID = table.Column<long>(type: "bigint", nullable: false),
                    ArticuloInsumoID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_ArticulosInsumo_ArticuloInsumoID",
                        column: x => x.ArticuloInsumoID,
                        principalTable: "ArticulosInsumo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_ArticulosManufacturados_ArticuloManufacturadoID",
                        column: x => x.ArticuloManufacturadoID,
                        principalTable: "ArticulosManufacturados",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetallesPedidos_Pedidos_PedidoID",
                        column: x => x.PedidoID,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosInsumo_RubroArticuloID",
                table: "ArticulosInsumo",
                column: "RubroArticuloID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturados_RubroGeneralID",
                table: "ArticulosManufacturados",
                column: "RubroGeneralID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturadosDetalles_ArticuloInsumoID",
                table: "ArticulosManufacturadosDetalles",
                column: "ArticuloInsumoID");

            migrationBuilder.CreateIndex(
                name: "IX_ArticulosManufacturadosDetalles_ArticuloManufacturadoID",
                table: "ArticulosManufacturadosDetalles",
                column: "ArticuloManufacturadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_DomicilioID",
                table: "Clientes",
                column: "DomicilioID");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioID",
                table: "Clientes",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_ArticuloInsumoID",
                table: "DetallesFacturas",
                column: "ArticuloInsumoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_ArticuloManufacturadoID",
                table: "DetallesFacturas",
                column: "ArticuloManufacturadoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesFacturas_FacturaID",
                table: "DetallesFacturas",
                column: "FacturaID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_ArticuloInsumoID",
                table: "DetallesPedidos",
                column: "ArticuloInsumoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_ArticuloManufacturadoID",
                table: "DetallesPedidos",
                column: "ArticuloManufacturadoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesPedidos_PedidoID",
                table: "DetallesPedidos",
                column: "PedidoID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteID",
                table: "Pedidos",
                column: "ClienteID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_DomicilioID",
                table: "Pedidos",
                column: "DomicilioID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_FacturaID",
                table: "Pedidos",
                column: "FacturaID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_MercadoPagoDatosID",
                table: "Pedidos",
                column: "MercadoPagoDatosID");

            migrationBuilder.CreateIndex(
                name: "IX_RubrosArticulos_RubroPadreID",
                table: "RubrosArticulos",
                column: "RubroPadreID");
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
