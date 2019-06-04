namespace WebAPIwJWT.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int IdUser { get; set; }

        [Column("UserName")]
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }

        public bool Active { get; set; }

        public DateTime? LastAccess { get; set; }
    }
}
