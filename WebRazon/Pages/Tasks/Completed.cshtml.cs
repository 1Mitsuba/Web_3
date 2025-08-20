using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using WebRazon.Models;
using WebRazon.Services;
using X.PagedList;

namespace WebRazon.Pages.Tasks
{
    public class CompletedModel : PageModel
    {
        private readonly ILogger<CompletedModel> _logger;
        private readonly TaskService _taskService;

        public IPagedList<TaskItem> PagedTasks { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "date";
        
        [BindProperty(SupportsGet = true)]
        public string SortDirection { get; set; } = "desc";

        public CompletedModel(ILogger<CompletedModel> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public void OnGet()
        {
            LoadTasks();
        }
        
        public void OnGetRecent()
        {
            Tasks = _taskService.GetCompletedTasks()
                .OrderByDescending(t => t.FechaVencimiento)
                .ToList();
                
            ApplyPagination();
        }
        
        public void OnGetOld()
        {
            Tasks = _taskService.GetCompletedTasks()
                .OrderBy(t => t.FechaVencimiento)
                .ToList();
                
            ApplyPagination();
        }

        public IActionResult OnGetReactivate(int id)
        {
            _taskService.MarkAsActive(id);
            _logger.LogInformation("Tarea reactivada: {TaskId}", id);
            return RedirectToPage();
        }

        public IActionResult OnGetDelete(int id)
        {
            _taskService.DeleteTask(id);
            _logger.LogInformation("Tarea eliminada: {TaskId}", id);
            return RedirectToPage();
        }
        
        public IActionResult OnGetSort(string sortBy, string sortDirection)
        {
            SortBy = sortBy;
            SortDirection = sortDirection;
            LoadTasks();
            return Page();
        }

        private void LoadTasks()
        {
            var completedTasks = _taskService.GetCompletedTasks();
            
            // Aplicar orden
            switch(SortBy?.ToLower())
            {
                case "name":
                    Tasks = SortDirection?.ToLower() == "asc" 
                        ? completedTasks.OrderBy(t => t.Nombre).ToList()
                        : completedTasks.OrderByDescending(t => t.Nombre).ToList();
                    break;
                case "date":
                default:
                    Tasks = SortDirection?.ToLower() == "asc" 
                        ? completedTasks.OrderBy(t => t.FechaVencimiento).ToList()
                        : completedTasks.OrderByDescending(t => t.FechaVencimiento).ToList();
                    break;
            }
            
            ApplyPagination();
        }
        
        private void ApplyPagination()
        {
            // Asegurarse que el número de página es válido
            if (PageNumber < 1)
                PageNumber = 1;
                
            // Crear página de resultados
            PagedTasks = new PagedList<TaskItem>(Tasks.AsQueryable(), PageNumber, PageSize);
        }
    }
}