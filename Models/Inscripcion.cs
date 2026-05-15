using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("inscripcion")]
    public class Inscripcion
    {
        [Key]
        [Column("id_inscripcion")]
        public long IdInscripcion { get; set; }

        [Column("id_cliente")]
        public long IdCliente { get; set; }

        [Column("id_tour")]
        public long IdTour { get; set; }

        [Column("id_pago")]
        public long IdPago { get; set; }

        [Column("fecha_inscripcion")]
        public DateTime FechaInscripcion { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = "Activa";

        // ✅ Navegación - SIN columna propia, solo referencia
        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [ForeignKey("IdTour")]
        public Tour? Tour { get; set; }

        [ForeignKey("IdPago")]
        public Pago? Pago { get; set; }
    }
}