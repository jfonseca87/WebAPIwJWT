namespace WebAPIwJWT.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Socio")]
    public partial class Socio
    {
        [Key]
        public int IdSocio { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        public DateTime? FechaIngreso { get; set; }
    }
}
