using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("metodo_pago")]
    public class MetodoPago
    {
        [Key]
        [Column("id_metodo")]
        public int IdMetodo { get; set; }

        [Required]
        [Column("nombre_metodo")]
        [MaxLength(50)]
        public string NombreMetodo { get; set; } = string.Empty;
    }
}
