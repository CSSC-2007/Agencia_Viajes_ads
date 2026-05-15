using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("tour")]
    public class Tour
    {
        [Key]
        [Column("id_tour")]
        public int IdTour { get; set; }

        [Column("nombre_tour")]
        public string NombreTour { get; set; } = string.Empty;
    }
}