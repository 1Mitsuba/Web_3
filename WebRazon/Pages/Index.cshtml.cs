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
        private readonly TaskService _taskService;

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        
        [BindProperty]
        public string TaskName { get; set; }
        
        [BindProperty]
        public string Description { get; set; }
        
        [BindProperty]
        public DateTime? DueDate { get; set; }

        public IndexModel(ILogger<IndexModel> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public void OnGet()
        {
            LoadTasks();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
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
            
            return RedirectToPage();
        }

        public IActionResult OnGetComplete(int id)
        {
            _taskService.MarkAsCompleted(id);
            _logger.LogInformation("Tarea marcada como completada: {TaskId}", id);
            return RedirectToPage();
        }

        public IActionResult OnGetCancel(int id)
        {
            _taskService.CancelTask(id);
            _logger.LogInformation("Tarea cancelada: {TaskId}", id);
            return RedirectToPage();
        }

        public IActionResult OnGetDelete(int id)
        {
            _taskService.DeleteTask(id);
            _logger.LogInformation("Tarea eliminada: {TaskId}", id);
            return RedirectToPage();
        }

        private void LoadTasks()
        {
            Tasks = _taskService.GetAllTasks();
        }
    }
}
