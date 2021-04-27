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
            //DetallePedido - Pedido
            modelBuilder.Entity<DetallePedido>()
                .HasOne<Pedido>(r => r.Pedido)
                .WithMany(s => s.DetallesPedido)
                .HasForeignKey(t => t.IdPedido)
                .OnDelete(DeleteBehavior.Cascade);

            //DetalleFactura - Factura
            modelBuilder.Entity<DetalleFactura>()
                .HasOne<Factura>(r => r.Factura)
                .WithMany(s => s.DetallesFactura)
                .HasForeignKey(t => t.IdFactura)
                .OnDelete(DeleteBehavior.Cascade);

            //ArticuloManufacturadoDetalle - ArticuloManufacturado
            modelBuilder.Entity<ArticuloManufacturadoDetalle>()
                .HasOne<ArticuloManufacturado>(r => r.ArticuloManufacturado)
                .WithMany(s => s.ArticuloManufacturadoDetalles)
                .HasForeignKey(t => t.IdArticuloManufacturado)
                .OnDelete(DeleteBehavior.Cascade);

            
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
