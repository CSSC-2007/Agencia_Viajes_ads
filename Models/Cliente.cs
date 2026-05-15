using Agencia_Viajes_ADS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("cliente")]
public class Cliente
{
    [Key]
    [Column("id_cliente")]
    public long IdCliente { get; set; }   // Cambiado de int a long

    [Required]
    [Column("nombre")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Column("direccion")]
    [MaxLength(200)]
    public string? Direccion { get; set; }

    [Column("telefono")]
    [MaxLength(15)]
    public string? Telefono { get; set; }

    [Column("ocupacion")]
    [MaxLength(50)]
    public string? Ocupacion { get; set; }

    [Column("estado_cliente")]
    public bool EstadoCliente { get; set; } = true;

    // Navegación inversa
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
