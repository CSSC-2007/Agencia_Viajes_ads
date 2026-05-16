using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("tour")]
    public class Tour
    {
        [Key]
        [Column("id_tour")]
        public long IdTour { get; set; }

        [Column("id_escala")]
        public long IdEscala { get; set; }

        [Column("nombre_tour")]
        public string NombreTour { get; set; }

        [Column("descripcion_tour")]
        public string DescripcionTour { get; set; }

        [Column("fecha_salida")]
        public DateTime FechaSalida { get; set; }

        [Column("fecha_llegada")]
        public DateTime FechaLlegada { get; set; }

        [Column("cantidad_plazas")]
        public int CantidadPlazas { get; set; }

        [Column("plazas_ocupadas")]
        public int PlazasOcupadas { get; set; }
    }
}