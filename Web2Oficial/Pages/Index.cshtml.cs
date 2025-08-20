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
        
        // Opciones de tama�o de p�gina para el selector
        public List<int> OpcionesTamanoPagina { get; } = new List<int> { 5, 10, 15, 25, 50 };
        
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int pagina = 1, int tamanoPagina = 5)
        {
            // Asegurarse de que el tama�o de p�gina sea v�lido
            if (!OpcionesTamanoPagina.Contains(tamanoPagina))
            {
                tamanoPagina = 5; // Valor predeterminado si el par�metro no es v�lido
            }
            
            TamanoPagina = tamanoPagina;
            
            // Ruta al archivo JSON
            string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tareas.json");

            // Leer el JSON y deserializarlo
            var jsonContent = System.IO.File.ReadAllText(jsonFilePath);
            var todasLasTareas = JsonSerializer.Deserialize<List<Tarea>>(jsonContent);
            
            // Contar las tareas completadas para las estad�sticas
            TareasCompletadas = todasLasTareas.Count(t => t.estado == "Finalizado");
            
            // Filtrar para excluir las tareas finalizadas
            var tareasActivas = todasLasTareas.Where(t => t.estado != "Finalizado").ToList();
            TareasActivas = tareasActivas.Count;

            // L�gica de paginaci�n
            PaginaActual = pagina < 1 ? 1 : pagina;
            TotalPaginas = (int)Math.Ceiling(tareasActivas.Count / (double)TamanoPagina);
            
            // Asegurarse de que la p�gina actual es v�lida despu�s de cambiar el tama�o de p�gina
            if (PaginaActual > TotalPaginas && TotalPaginas > 0)
            {
                PaginaActual = TotalPaginas;
            }
            
            Tareas = tareasActivas.Skip((PaginaActual - 1) * TamanoPagina).Take(TamanoPagina).ToList();
        }
    }
}
