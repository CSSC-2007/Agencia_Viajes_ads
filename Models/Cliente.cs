using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Required]
        [Column("nombre")]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Column("direccion")]
        [MaxLength(200)]
        public string? Direccion { get; set; }

        [Column("telefono")]
        [MaxLength(15)]
        public string? Telefono { get; set; }

        [Column("ocupacion")]
        [MaxLength(50)]
        public string? Ocupacion { get; set; }

        [Column("estado_cliente")]
        public bool EstadoCliente { get; set; } = true;

        // ✅ Sin navegación a Usuario
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}