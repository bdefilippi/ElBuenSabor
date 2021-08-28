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
        public ElBuenSaborContext(DbContextOptions<ElBuenSaborContext> options): base(options)
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

            //DetalleFactura - DetallePedido (composicion)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(a => a.DetallePedido)
                .WithOne();

            //Receta - Articulo (composicion)
            modelBuilder.Entity<Receta>()
                .HasOne<Articulo>(r => r.Articulo)
                .WithMany(s => s.Recetas)
                .HasForeignKey(t => t.ArticuloID)
                .OnDelete(DeleteBehavior.Cascade);

            //DetalleReceta - Receta (composicion)
            modelBuilder.Entity<DetalleReceta>()
                .HasOne<Receta>(r => r.Receta)
                .WithMany(s => s.DetallesRecetas)
                .HasForeignKey(t => t.RecetaID)
                .OnDelete(DeleteBehavior.Cascade);

            //DetalleReceta - Articulo (CAMBIADO)
            modelBuilder.Entity<DetalleReceta>()
                .HasOne<Articulo>(r => r.Articulo)
                .WithMany(s => s.DetallesRecetas)
                .HasForeignKey(t => t.ArticuloID)
                .OnDelete(DeleteBehavior.NoAction);

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

            //Articulo - RubroArticulo
            modelBuilder.Entity<Articulo>()
                .HasOne<RubroArticulo>(r => r.RubroArticulo)
                .WithMany(s => s.Articulos)
                .HasForeignKey(t => t.RubroArticuloID)
                .OnDelete(DeleteBehavior.NoAction);

            //DetallePedido - Articulo
            modelBuilder.Entity<DetallePedido>()
                .HasOne<Articulo>(r => r.Articulo)
                .WithMany(s => s.DetallesPedidos)
                .HasForeignKey(t => t.ArticuloID)
                .OnDelete(DeleteBehavior.NoAction);




        }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Configuracion> Configuraciones { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<DetalleFactura> DetallesFacturas { get; set; }
        public DbSet<DetalleReceta> DetallesRecetas { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<EgresoArticulo> EgresosArticulos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<MercadoPagoDatos> MercadoPagoDatos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PrecioVentaArticulo> PreciosVentaArticulos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RubroArticulo> RubrosArticulos { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

    }
}
