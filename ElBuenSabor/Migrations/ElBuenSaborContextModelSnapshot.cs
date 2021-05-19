﻿// <auto-generated />
using System;
using ElBuenSabor.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElBuenSabor.Migrations
{
    [DbContext(typeof(ElBuenSaborContext))]
    partial class ElBuenSaborContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ElBuenSabor.Models.Articulo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ALaVenta")
                        .HasColumnType("bit");

                    b.Property<string>("Denominacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RubroArticuloID")
                        .HasColumnType("bigint");

                    b.Property<double>("StockMinimo")
                        .HasColumnType("float");

                    b.Property<int>("TiempoEstimadoCocina")
                        .HasColumnType("int");

                    b.Property<string>("UnidadMedida")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RubroArticuloID");

                    b.ToTable("Articulos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Cliente", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Apellido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Telefono")
                        .HasColumnType("bigint");

                    b.Property<long>("UsuarioID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioID");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Configuracion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadCocineros")
                        .HasColumnType("int");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("EmailEmpresa")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TokenMercadoPago")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Configuraciones");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetalleFactura", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<long>("FacturaID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.HasIndex("FacturaID");

                    b.ToTable("DetallesFacturas");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetallePedido", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<long>("PedidoID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.HasIndex("PedidoID");

                    b.ToTable("DetallesPedidos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetalleReceta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<double>("Cantidad")
                        .HasColumnType("float");

                    b.Property<long>("RecetaID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.HasIndex("RecetaID");

                    b.ToTable("DetallesRecetas");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Domicilio", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Calle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ClienteID")
                        .HasColumnType("bigint");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Localidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteID");

                    b.ToTable("Domicilios");
                });

            modelBuilder.Entity("ElBuenSabor.Models.EgresoArticulo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CantidadEgresada")
                        .HasColumnType("int");

                    b.Property<long>("DetalleFacturaId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<long>("StockID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DetalleFacturaId");

                    b.HasIndex("StockID");

                    b.ToTable("EgresosArticulos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Factura", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("FormaPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MontoDescuento")
                        .HasColumnType("float");

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("ElBuenSabor.Models.MercadoPagoDatos", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaAprobacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("FormaPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("IdentificadorPago")
                        .HasColumnType("bigint");

                    b.Property<string>("MetodoPago")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NroTarjeta")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MercadoPagoDatos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Pedido", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ClienteID")
                        .HasColumnType("bigint");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<long>("DomicilioID")
                        .HasColumnType("bigint");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<long>("FacturaID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("HoraEstimadaFin")
                        .HasColumnType("datetime2");

                    b.Property<long>("MercadoPagoDatosID")
                        .HasColumnType("bigint");

                    b.Property<long>("Numero")
                        .HasColumnType("bigint");

                    b.Property<int>("TipoEnvio")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteID");

                    b.HasIndex("DomicilioID");

                    b.HasIndex("FacturaID");

                    b.HasIndex("MercadoPagoDatosID");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.PrecioVentaArticulo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("PrecioVenta")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.ToTable("PreciosVentaArticulos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Receta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<int>("TiempoEstimadoCocina")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.ToTable("Recetas");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Rol", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ElBuenSabor.Models.RubroArticulo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Denominacion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("RubrosArticulos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Stock", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ArticuloID")
                        .HasColumnType("bigint");

                    b.Property<int>("CantidadCompradorProveedor")
                        .HasColumnType("int");

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCompra")
                        .HasColumnType("datetime2");

                    b.Property<double>("PrecioCompra")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ArticuloID");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Usuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Clave")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("NombreUsuario")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RolId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("RolId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Articulo", b =>
                {
                    b.HasOne("ElBuenSabor.Models.RubroArticulo", "RubroArticulo")
                        .WithMany("Articulos")
                        .HasForeignKey("RubroArticuloID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("RubroArticulo");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Cliente", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetalleFactura", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("DetallesFacturas")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Factura", "Factura")
                        .WithMany("DetallesFactura")
                        .HasForeignKey("FacturaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");

                    b.Navigation("Factura");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetallePedido", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("DetallesPedidos")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Pedido", "Pedido")
                        .WithMany("DetallesPedido")
                        .HasForeignKey("PedidoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("ElBuenSabor.Models.DetalleReceta", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("DetallesRecetas")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Receta", "Receta")
                        .WithMany("DetallesRecetas")
                        .HasForeignKey("RecetaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");

                    b.Navigation("Receta");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Domicilio", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Cliente", "Cliente")
                        .WithMany("Domicilios")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("ElBuenSabor.Models.EgresoArticulo", b =>
                {
                    b.HasOne("ElBuenSabor.Models.DetalleFactura", "DetalleFactura")
                        .WithMany()
                        .HasForeignKey("DetalleFacturaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Stock", "Stock")
                        .WithMany("EgresosArticulos")
                        .HasForeignKey("StockID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DetalleFactura");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Pedido", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Cliente", "Cliente")
                        .WithMany("Pedidos")
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Domicilio", "Domicilio")
                        .WithMany("Pedidos")
                        .HasForeignKey("DomicilioID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.Factura", "Factura")
                        .WithMany()
                        .HasForeignKey("FacturaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ElBuenSabor.Models.MercadoPagoDatos", "MercadoPagoDatos")
                        .WithMany()
                        .HasForeignKey("MercadoPagoDatosID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Domicilio");

                    b.Navigation("Factura");

                    b.Navigation("MercadoPagoDatos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.PrecioVentaArticulo", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("PreciosVentaArticulos")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Receta", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("Recetas")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Stock", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Articulo", "Articulo")
                        .WithMany("Stocks")
                        .HasForeignKey("ArticuloID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articulo");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Usuario", b =>
                {
                    b.HasOne("ElBuenSabor.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Articulo", b =>
                {
                    b.Navigation("DetallesFacturas");

                    b.Navigation("DetallesPedidos");

                    b.Navigation("DetallesRecetas");

                    b.Navigation("PreciosVentaArticulos");

                    b.Navigation("Recetas");

                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Cliente", b =>
                {
                    b.Navigation("Domicilios");

                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Domicilio", b =>
                {
                    b.Navigation("Pedidos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Factura", b =>
                {
                    b.Navigation("DetallesFactura");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Pedido", b =>
                {
                    b.Navigation("DetallesPedido");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Receta", b =>
                {
                    b.Navigation("DetallesRecetas");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("ElBuenSabor.Models.RubroArticulo", b =>
                {
                    b.Navigation("Articulos");
                });

            modelBuilder.Entity("ElBuenSabor.Models.Stock", b =>
                {
                    b.Navigation("EgresosArticulos");
                });
#pragma warning restore 612, 618
        }
    }
}
