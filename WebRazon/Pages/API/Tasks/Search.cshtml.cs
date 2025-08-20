using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;
using WebRazon.Services;

namespace WebRazon.Pages.API.Tasks
{
    public class SearchModel : PageModel
    {
        private readonly TaskService _taskService;
        
        public List<TaskItem> Results { get; set; }
        
        public SearchModel(TaskService taskService)
        {
            _taskService = taskService;
            Results = new List<TaskItem>();
        }
        
        public void OnGet(string term)
        {
            if (string.IsNullOrEmpty(term) || term.Length < 2)
            {
                // Retornar lista vac�a si el t�rmino de b�squeda es muy corto
                return;
            }
            
            // Buscar tareas que coincidan con el t�rmino en el nombre o descripci�n
            Results = _taskService.GetAllTasks()
                .Where(t => (t.Nombre != null && t.Nombre.Contains(term, StringComparison.OrdinalIgnoreCase)) || 
                            (t.Descripcion != null && t.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase)))
                .Take(10)
                .ToList();
        }
    }
}