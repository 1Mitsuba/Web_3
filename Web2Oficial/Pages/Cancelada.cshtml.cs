using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Web2Oficial.Models;

namespace Web2Oficial.Pages
{
    public class CanceladaModel : PageModel
    {
        public List<Tarea> TareasCanceladas { get; set; } = new List<Tarea>();
        public int TotalTareasCanceladas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanoPagina { get; set; }
        
        // Opciones de tamaño de página para el selector
        public List<int> OpcionesTamanoPagina { get; } = new List<int> { 5, 10, 15, 25, 50 };

        public void OnGet(int pagina = 1, int tamanoPagina = 5)
        {
            // Asegurarse de que el tamaño de página sea válido
            if (!OpcionesTamanoPagina.Contains(tamanoPagina))
            {
                tamanoPagina = 5; // Valor predeterminado si el parámetro no es válido
            }
            
            TamanoPagina = tamanoPagina;
            
            // Ruta al archivo JSON
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tareas.json");

            // Leer el JSON y deserializarlo
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var todasLasTareas = JsonSerializer.Deserialize<List<Tarea>>(jsonContent);

            // Filtrar solo las tareas canceladas
            var soloCanceladas = todasLasTareas.Where(t => t.estado == "Cancelado").ToList();
            TotalTareasCanceladas = soloCanceladas.Count;

            // Lógica de paginación
            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(soloCanceladas.Count / (double)TamanoPagina);
            
            // Asegurarse de que la página actual es válida después de cambiar el tamaño de página
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }
            
            TareasCanceladas = soloCanceladas.Skip((PaginaActual - 1) * TamanoPagina).Take(TamanoPagina).ToList();
        }
    }
}