using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("escala")]
    public class Escala
    {
        [Key]
        [Column("id_escala")]
        public long IdEscala { get; set; }

        [Column("lugar_escala")]
        public string LugarEscala { get; set; }

        [Column("orden")]
        public int Orden { get; set; }
    }
}