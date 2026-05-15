using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("pago")]
    public class Pago
    {
        [Key]
        [Column("id_pago")]
        public int IdPago { get; set; }

        [Column("monto_total")]
        public decimal MontoTotal { get; set; }

        [Column("metodo_pago")]
        public string MetodoPago { get; set; } = string.Empty;

        [Column("cantidad_cuotas")]
        public int CantidadCuotas { get; set; }

        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; }

        [Column("factura")]
        public string Factura { get; set; } = string.Empty;

        public ICollection<Inscripcion> Inscripciones { get; set; }
            = new List<Inscripcion>();
    }
}