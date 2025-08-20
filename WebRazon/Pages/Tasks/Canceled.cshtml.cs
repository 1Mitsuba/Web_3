using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using WebRazon.Models;
using WebRazon.Services;

namespace WebRazon.Pages.Tasks
{
    public class CanceledModel : PageModel
    {
        private readonly ILogger<CanceledModel> _logger;
        private readonly TaskService _taskService;

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public CanceledModel(ILogger<CanceledModel> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public void OnGet()
        {
            LoadTasks();
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

        private void LoadTasks()
        {
            Tasks = _taskService.GetCanceledTasks();
        }
    }
}