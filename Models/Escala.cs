using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("escala")]
    public class Escala
    {
        [Key]
        [Column("id_escala")]
        public int IdEscala { get; set; }

        [Required]
        [Column("lugar_escala")]
        [MaxLength(100)]
        public string LugarEscala { get; set; } = string.Empty;

        [Column("orden")]
        public int Orden { get; set; }
    }
}