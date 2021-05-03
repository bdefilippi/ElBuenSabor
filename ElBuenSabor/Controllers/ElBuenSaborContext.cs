using ElBuenSabor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElBuenSabor.Controllers
{
    public class ElBuenSaborContext : DbContext
    {
        public ElBuenSaborContext(DbContextOptions<ElBuenSaborContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //DetallePedido - Pedido (composicion)
            modelBuilder.Entity<DetallePedido>()
                .HasOne<Pedido>(r => r.Pedido)
                .WithMany(s => s.DetallesPedido)
                .HasForeignKey(t => t.PedidoID)
                .OnDelete(DeleteBehavior.Cascade);

            //DetalleFactura - Factura (composicion)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne<Factura>(r => r.Factura)
                .WithMany(s => s.DetallesFactura)
                .HasForeignKey(t => t.FacturaID)
                .OnDelete(DeleteBehavior.Cascade);

            //ArticuloManufacturadoDetalle - ArticuloManufacturado (composicion)
            modelBuilder.Entity<ArticuloManufacturadoDetalle>()
                .HasOne<ArticuloManufacturado>(r => r.ArticuloManufacturado)
                .WithMany(s => s.ArticuloManufacturadoDetalles)
                .HasForeignKey(t => t.ArticuloManufacturadoID)
                .OnDelete(DeleteBehavior.Cascade);

            //Pedido - Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne<Cliente>(r => r.Cliente)
                .WithMany(s => s.Pedidos)
                .HasForeignKey(t => t.ClienteID)
                .OnDelete(DeleteBehavior.NoAction);

            //Pedido - Domicilio REVISAR ESTE
            modelBuilder.Entity<Pedido>()
                .HasOne<Domicilio>(r => r.Domicilio)
                .WithMany(s => s.Pedidos)
                .HasForeignKey(t => t.DomicilioID)
                .OnDelete(DeleteBehavior.NoAction);

            //ArticuloManufacturado - RubroGeneral
            modelBuilder.Entity<ArticuloManufacturado>()
                .HasOne<RubroGeneral>(r => r.RubroGeneral)
                .WithMany(s => s.ArticulosManufacturados)
                .HasForeignKey(t => t.RubroGeneralID)
                .OnDelete(DeleteBehavior.NoAction);

            //DetallePedido - ArticuloManufacturado
            modelBuilder.Entity<DetallePedido>()
                .HasOne<ArticuloManufacturado>(r => r.ArticuloManufacturado)
                .WithMany(s => s.DetallesPedidos)
                .HasForeignKey(t => t.ArticuloManufacturadoID)
                .OnDelete(DeleteBehavior.NoAction);

            //DetallePedido - ArticuloInsumo
            modelBuilder.Entity<DetallePedido>()
                .HasOne<ArticuloInsumo>(r => r.ArticuloInsumo)
                .WithMany(s => s.DetallePedidos)
                .HasForeignKey(t => t.ArticuloInsumoID)
                .OnDelete(DeleteBehavior.NoAction);

            //DetalleFactura - ArticuloManufacturado
            modelBuilder.Entity<DetalleFactura>()
                .HasOne<ArticuloManufacturado>(r => r.ArticuloManufacturado)
                .WithMany(s => s.DetallesFacturas)
                .HasForeignKey(t => t.ArticuloManufacturadoID)
                .OnDelete(DeleteBehavior.NoAction);

            //DetalleFactura - ArticuloInsumo
            modelBuilder.Entity<DetalleFactura>()
                .HasOne<ArticuloInsumo>(r => r.ArticuloInsumo)
                .WithMany(s => s.DetalleFacturas)
                .HasForeignKey(t => t.ArticuloInsumoID)
                .OnDelete(DeleteBehavior.NoAction);

            //ArticuloInsumo - RubroArticulo
            modelBuilder.Entity<ArticuloInsumo>()
                .HasOne<RubroArticulo>(r => r.RubroArticulo)
                .WithMany(s => s.ArticulosInsumo)
                .HasForeignKey(t => t.RubroArticuloID)
                .OnDelete(DeleteBehavior.NoAction);

            //RubroArticulo - RubroArticulo
            modelBuilder.Entity<RubroArticulo>()
                .HasOne<RubroArticulo>(r => r.RubroPadre)
                .WithMany(s => s.RubrosHijos)
                .HasForeignKey(t => t.RubroPadreID)
                .OnDelete(DeleteBehavior.NoAction);

        }

        public DbSet<ArticuloInsumo> ArticulosInsumo { get; set; }
        public DbSet<ArticuloManufacturado> ArticulosManufacturados { get; set; }
        public DbSet<ArticuloManufacturadoDetalle> ArticulosManufacturadosDetalles { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<DetalleFactura> DetallesFacturas { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<MercadoPagoDatos> MercadoPagoDatos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<RubroArticulo> RubrosArticulos { get; set; }
        public DbSet<RubroGeneral> RubrosGenerales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}
