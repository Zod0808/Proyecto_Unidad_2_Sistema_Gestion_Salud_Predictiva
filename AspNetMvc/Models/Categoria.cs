using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Proyecto_Unidad_2_MVC_Sistema_Gestion_Salud_Predictiva.Models
{
    [Table("Categoria")]
    public partial class Categoria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categoria()
        {
            Helado = new HashSet<Helado>();
        }

        [Key]
        public int id_categ { get; set; }

        [Required]
        [StringLength(20)]
        public string nombre { get; set; }

        [StringLength(70)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(1)]
        public string estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Helado> Helado { get; set; }
    }
}
