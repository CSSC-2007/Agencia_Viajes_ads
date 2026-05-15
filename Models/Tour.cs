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

        [Column("id_escala")]
        public int IdEscala { get; set; }

        [Required]
        [Column("nombre_tour")]
        [MaxLength(100)]
        public string NombreTour { get; set; } = string.Empty;

        [Column("descripcion_tour")]
        [MaxLength(255)]
        public string? DescripcionTour { get; set; }

        [Column("fecha_salida")]
        public DateTime FechaSalida { get; set; }

        [Column("fecha_llegada")]
        public DateTime FechaLlegada { get; set; }

        [Column("cantidad_plazas")]
        public int CantidadPlazas { get; set; }

        [Column("plazas_ocupadas")]
        public int PlazasOcupadas { get; set; }

        [ForeignKey("IdEscala")]
        public Escala? Escala { get; set; }
    }
}