namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pedido")]
    public partial class Pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedido()
        {
            Detalle_Pedido = new HashSet<Detalle_Pedido>();
        }

        [Key]
        [StringLength(5)]
        public string cod_pedido { get; set; }

        [Required]
        [StringLength(5)]
        public string cod_cliente { get; set; }

        public int id_repartidor { get; set; }

        [Required]
        [StringLength(3)]
        public string cod_horario { get; set; }

        public DateTime? fec_pedido { get; set; }

        public DateTime? fec_expedicion { get; set; }

        [StringLength(1)]
        public string estado { get; set; }

        public virtual Cliente Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_Pedido> Detalle_Pedido { get; set; }

        public virtual Horario Horario { get; set; }

        public virtual Repartidor Repartidor { get; set; }
    }
}
