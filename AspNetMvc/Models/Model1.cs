using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Detalle_Pedido> Detalle_Pedido { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<Helado> Helado { get; set; }
        public virtual DbSet<Horario> Horario { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Repartidor> Repartidor { get; set; }
        public virtual DbSet<Ubigeo> Ubigeo { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .Property(e => e.estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.Helado)
                .WithRequired(e => e.Categoria)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.cod_cliente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.dni)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .Property(e => e.cod_ubigeo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cliente>()
                .HasMany(e => e.Pedido)
                .WithRequired(e => e.Cliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Detalle_Pedido>()
                .Property(e => e.cod_pedido)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Detalle_Pedido>()
                .Property(e => e.cod_helado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Detalle_Pedido>()
                .Property(e => e.precioventa)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.cod_empleado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.dni)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.celular)
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .Property(e => e.cod_ubigeo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.Usuario)
                .WithRequired(e => e.Empleado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Helado>()
                .Property(e => e.cod_helado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Helado>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Helado>()
                .Property(e => e.composicion)
                .IsUnicode(false);

            modelBuilder.Entity<Helado>()
                .Property(e => e.precio)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Helado>()
                .HasMany(e => e.Detalle_Pedido)
                .WithRequired(e => e.Helado)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Horario>()
                .Property(e => e.cod_horario)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Horario>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Horario>()
                .HasMany(e => e.Pedido)
                .WithRequired(e => e.Horario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.cod_pedido)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.cod_cliente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.cod_horario)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .Property(e => e.estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Pedido>()
                .HasMany(e => e.Detalle_Pedido)
                .WithRequired(e => e.Pedido)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Repartidor>()
                .Property(e => e.apellido)
                .IsUnicode(false);

            modelBuilder.Entity<Repartidor>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Repartidor>()
                .Property(e => e.direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Repartidor>()
                .Property(e => e.cod_ubigeo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Repartidor>()
                .HasMany(e => e.Pedido)
                .WithRequired(e => e.Repartidor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ubigeo>()
                .Property(e => e.cod_ubigeo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Ubigeo>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Ubigeo>()
                .Property(e => e.cod_ubigeo_sup)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Ubigeo>()
                .HasMany(e => e.Cliente)
                .WithRequired(e => e.Ubigeo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ubigeo>()
                .HasMany(e => e.Repartidor)
                .WithRequired(e => e.Ubigeo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.cod_empleado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.clave)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.nivel)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.estado)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
