namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ubigeo")]
    public partial class Ubigeo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ubigeo()
        {
            Cliente = new HashSet<Cliente>();
            Repartidor = new HashSet<Repartidor>();
        }

        [Key]
        [StringLength(6)]
        public string cod_ubigeo { get; set; }

        [StringLength(70)]
        public string descripcion { get; set; }

        [StringLength(6)]
        public string cod_ubigeo_sup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cliente> Cliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repartidor> Repartidor { get; set; }
    }
}
