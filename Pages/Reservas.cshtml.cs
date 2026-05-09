using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Agencia_Viajes_ADS.Pages
{
        public class ReservasModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ReservasModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Cliente> Clientes { get; set; } = new();

        public List<Tour> Tours { get; set; } = new();

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        public int IdTour { get; set; }

        [BindProperty]
        public decimal Monto { get; set; }

        [BindProperty]
        public string MetodoPago { get; set; }

        [BindProperty]
        public int Cuotas { get; set; }

        [BindProperty]
        public string Factura { get; set; }

        public void OnGet()
        {
            CargarClientes();
            CargarTours();
        }

        public void CargarClientes()
        {

        }

        public void CargarTours()
        {

        }
    }

    public class Cliente
    {
        public int IdCliente { get; set; }

        public string Nombre { get; set; }
    }

    public class Tour
    {
        public int IdTour { get; set; }
    }
}
