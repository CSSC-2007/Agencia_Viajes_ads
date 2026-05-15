using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("inscripcion")]
    public class Inscripcion
    {
        [Key]
        [Column("id_inscripcion")]
        public int IdInscripcion { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("id_tour")]
        public int IdTour { get; set; }

        [Column("id_pago")]
        public int IdPago { get; set; }

        [Column("fecha_inscripcion")]
        public DateTime FechaInscripcion { get; set; }

        [Column("estado")]
        public string Estado { get; set; } = string.Empty;

        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; } = null!;

        [ForeignKey("IdTour")]
        public Tour Tour { get; set; } = null!;

        [ForeignKey("IdPago")]
        public Pago Pago { get; set; } = null!;
    }
}