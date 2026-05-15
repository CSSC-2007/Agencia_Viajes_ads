using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("username")]
        [MaxLength(30)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("id_rol")]
        public int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public Rol Rol { get; set; } = null!;

        [Column("activo")]
        public bool Activo { get; set; } = true;

        [Column("id_cliente")]
        public long? IdCliente { get; set; }   // FK hacia Cliente

        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }
    }
}
