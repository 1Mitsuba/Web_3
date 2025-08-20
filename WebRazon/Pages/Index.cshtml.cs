using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using WebRazon.Models;
using WebRazon.Services;

namespace WebRazon.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public readonly TaskService _taskService;

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        
        [BindProperty]
        public string TaskName { get; set; }
        
        [BindProperty]
        public string Description { get; set; }
        
        [BindProperty]
        public DateTime? DueDate { get; set; }
        
        public string CurrentFilter { get; set; } = "all"; // all, active, completed, canceled, today, week, month, important
        public string CurrentCategory { get; set; } = "pending"; // pending, completed, canceled, important
        
        public string ActiveFilterClass(string tabName)
        {
            return CurrentFilter == tabName ? "active" : "";
        }
        
        public string ActiveCategoryClass(string categoryName)
        {
            return CurrentCategory == categoryName ? "active" : "";
        }

        public IndexModel(ILogger<IndexModel> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public void OnGet(string filter = "all", string category = "pending")
        {
            CurrentFilter = filter;
            CurrentCategory = category;
            LoadTasks();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(TaskName))
            {
                ModelState.AddModelError("TaskName", "El nombre de la tarea es obligatorio");
                LoadTasks();
                return Page();
            }

            var newTask = new TaskItem
            {
                Nombre = TaskName,
                Descripcion = Description,
                FechaVencimiento = DueDate
            };
            
            _taskService.AddTask(newTask);
            _logger.LogInformation("Nueva tarea creada: {TaskName}", TaskName);
            
            return RedirectToPage(new { filter = CurrentFilter, category = CurrentCategory });
        }

        public IActionResult OnGetComplete(int id, string returnFilter = "all", string returnCategory = "pending")
        {
            _taskService.MarkAsCompleted(id);
            _logger.LogInformation("Tarea marcada como completada: {TaskId}", id);
            return RedirectToPage(new { filter = returnFilter, category = returnCategory });
        }

        public IActionResult OnGetCancel(int id, string returnFilter = "all", string returnCategory = "pending")
        {
            _taskService.CancelTask(id);
            _logger.LogInformation("Tarea cancelada: {TaskId}", id);
            return RedirectToPage(new { filter = returnFilter, category = returnCategory });
        }

        public IActionResult OnGetDelete(int id, string returnFilter = "all", string returnCategory = "pending")
        {
            _taskService.DeleteTask(id);
            _logger.LogInformation("Tarea eliminada: {TaskId}", id);
            return RedirectToPage(new { filter = returnFilter, category = returnCategory });
        }

        private void LoadTasks()
        {
            switch (CurrentCategory)
            {
                case "completed":
                    Tasks = _taskService.GetCompletedTasks();
                    break;
                case "canceled":
                    Tasks = _taskService.GetCanceledTasks();
                    break;
                case "important":
                    Tasks = _taskService.GetImportantTasks();
                    break;
                default: // "pending"
                    switch (CurrentFilter)
                    {
                        case "today":
                            Tasks = _taskService.GetTasksForToday();
                            break;
                        case "week":
                            Tasks = _taskService.GetTasksForThisWeek();
                            break;
                        case "month":
                            Tasks = _taskService.GetTasksForThisMonth();
                            break;
                        default:
                            Tasks = _taskService.GetActiveTasks();
                            break;
                    }
                    break;
            }
        }
    }
}
