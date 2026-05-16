using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agencia_Viajes_ADS.Models
{
    [Table("pago")]
    public class Pago
    {
        [Key]
        [Column("id_pago")]
        public long IdPago { get; set; }

        [Column("monto_total")]
        public decimal MontoTotal { get; set; }

        [Column("metodo_pago")]
        public string MetodoPago { get; set; }

        [Column("cantidad_cuotas")]
        public int CantidadCuotas { get; set; }

        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; }

        [Column("factura")]
        public string Factura { get; set; }
    }
}