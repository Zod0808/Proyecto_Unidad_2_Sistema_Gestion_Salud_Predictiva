namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Detalle_Pedido
    {
        [Key]
        public int id_pedido { get; set; }

        [Required]
        [StringLength(5)]
        public string cod_pedido { get; set; }

        [Required]
        [StringLength(3)]
        public string cod_helado { get; set; }

        public int cantidad { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal precioventa { get; set; }

        public virtual Helado Helado { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}
