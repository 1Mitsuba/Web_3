using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Web2Oficial.Models;

namespace Web2Oficial.Pages
{
    public class IndexModel : PageModel
    {
        public List<Tarea> Tareas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; }
        public int TareasActivas { get; set; }
        public int TareasCompletadas { get; set; }
        
        public List<int> OpcionesTamanoPagina { get; } = new List<int> { 5, 10, 15, 25, 50 };
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int pagina = 1, int tamanoPagina = 5)
        {
            if (!OpcionesTamanoPagina.Contains(tamanoPagina))
            {
                tamanoPagina = 5;
            }
            
            TamanoPagina = tamanoPagina;
            
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tareas.json");

            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var todasLasTareas = JsonSerializer.Deserialize<List<Tarea>>(jsonContent);
            
            TareasCompletadas = todasLasTareas.Count(t => t.estado == "Finalizado");
            
            var tareasActivas = todasLasTareas.Where(t => t.estado != "Finalizado").ToList();
            TareasActivas = tareasActivas.Count;

            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(tareasActivas.Count / (double)TamanoPagina);
            
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }
            
            Tareas = tareasActivas.Skip((PaginaActual - 1) * TamanoPagina).Take(TamanoPagina).ToList();
        }
    }
}
