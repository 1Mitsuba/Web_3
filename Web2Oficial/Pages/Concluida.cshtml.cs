using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Web2Oficial.Models;

namespace Web2Oficial.Pages
{
    public class ConcluidaModel : PageModel
    {
        public List<Tarea> TareasFinalizadas { get; set; } = new List<Tarea>();
        public int TotalTareasFinalizadas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; }
        
        public List<int> OpcionesTamanoPagina { get; } = new List<int> { 5, 10, 15, 25, 50 };

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

            var soloFinalizadas = todasLasTareas.Where(t => t.estado == "Finalizado").ToList();
            TotalTareasFinalizadas = soloFinalizadas.Count;

            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(soloFinalizadas.Count / (double)TamanoPagina);
            
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }
            
            TareasFinalizadas = soloFinalizadas.Skip((PaginaActual - 1) * TamanoPagina).Take(TamanoPagina).ToList();
        }
    }
}