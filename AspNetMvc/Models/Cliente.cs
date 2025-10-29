namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Pedido = new HashSet<Pedido>();
        }

        [Key]
        [StringLength(5)]
        public string cod_cliente { get; set; }

        [Required]
        [StringLength(8)]
        public string dni { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [Required]
        [StringLength(75)]
        public string nombre { get; set; }

        [StringLength(255)]
        public string direccion { get; set; }

        [StringLength(12)]
        public string celular { get; set; }

        [StringLength(75)]
        public string email { get; set; }

        [Required]
        [StringLength(6)]
        public string cod_ubigeo { get; set; }

        public virtual Ubigeo Ubigeo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
