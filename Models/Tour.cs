using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("tour")]
    public class Tour
    {
        [Key]
        [Column("id_tour")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "El ID del Tour es obligatorio.")]
        public int IdTour { get; set; }

        [Column("nombre_tour")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El nombre del tour es obligatorio.")]
        public string NombreTour { get; set; } = string.Empty;

        [Column("descripcion_tour")]
        [MaxLength(255)]
        public string? DescripcionTour { get; set; }

        [Column("fecha_salida")]
        [Required(ErrorMessage = "La fecha de salida es obligatoria.")]
        public DateTime FechaSalida { get; set; }

        [Column("fecha_llegada")]
        [Required(ErrorMessage = "La fecha de llegada es obligatoria.")]
        public DateTime FechaLlegada { get; set; }

        [Column("cantidad_plazas")]
        [Required(ErrorMessage = "La cantidad de plazas es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de plazas debe ser al menos 1.")]
        public int CantidadPlazas { get; set; }

        [Column("plazas_ocupadas")]
        [Required(ErrorMessage = "Las plazas ocupadas son obligatorias.")]
        [Range(0, int.MaxValue, ErrorMessage = "Las plazas ocupadas no pueden ser negativas.")]
        public int PlazasOcupadas { get; set; }

        [Column("precio")]
        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        public ICollection<Escala> Escalas { get; set; } = new List<Escala>();
    }
}