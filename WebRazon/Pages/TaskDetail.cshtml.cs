using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebRazon.Models;
using WebRazon.Services;

namespace WebRazon.Pages
{
    public class TaskDetailModel : PageModel
    {
        private readonly TaskService _taskService;
        private readonly ILogger<TaskDetailModel> _logger;

        [BindProperty]
        public TaskItem Task { get; set; } = new TaskItem();
        
        [BindProperty(SupportsGet = true)]
        public string ReturnPage { get; set; } = "/Index";
        
        public List<string> SuggestedTaskNames { get; set; } = new List<string>();

        public TaskDetailModel(TaskService taskService, ILogger<TaskDetailModel> logger)
        {
            _taskService = taskService;
            _logger = logger;
            
            // Inicializar sugerencias comunes
            SuggestedTaskNames = new List<string>
            {
                "Reunión de equipo",
                "Revisión de código",
                "Actualizar documentación",
                "Preparar presentación",
                "Llamar al cliente",
                "Enviar correo electrónico",
                "Crear informe mensual",
                "Revisar presupuesto",
                "Planificar sprint"
            };
        }

        public IActionResult OnGet(int? id, string returnPage = "/Index")
        {
            ReturnPage = returnPage;
            
            if (id.HasValue && id.Value > 0)
            {
                var task = _taskService.GetTaskById(id.Value);
                
                if (task == null)
                {
                    return NotFound();
                }

                Task = task;
            }
            
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Task.Id > 0)
            {
                _taskService.UpdateTask(Task);
                _logger.LogInformation("Tarea actualizada: {TaskId}", Task.Id);
            }
            else
            {
                _taskService.AddTask(Task);
                _logger.LogInformation("Tarea creada: {TaskName}", Task.Nombre);
            }

            return RedirectToPage(ReturnPage);
        }
        
        [HttpGet]
        public JsonResult OnGetSuggestions(string term)
        {
            // Filtrar sugerencias basadas en el término de búsqueda
            var filteredSuggestions = SuggestedTaskNames
                .Where(s => s.Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList();
                
            // Agregar nombres de tareas existentes como sugerencias
            var existingTaskNames = _taskService.GetAllTasks()
                .Where(t => t.Nombre.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Select(t => t.Nombre)
                .Distinct()
                .ToList();
                
            // Combinar y eliminar duplicados
            return new JsonResult(filteredSuggestions.Union(existingTaskNames).Take(10).ToList());
        }
    }
}