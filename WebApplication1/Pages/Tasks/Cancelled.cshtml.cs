using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Pages.Tasks
{
    public class CancelledModel : PageModel
    {
        private readonly TaskService _taskService;

        public List<TodoTask> Tasks { get; set; }

        public CancelledModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet()
        {
            Tasks = _taskService.GetCancelledTasks();
        }
    }
}