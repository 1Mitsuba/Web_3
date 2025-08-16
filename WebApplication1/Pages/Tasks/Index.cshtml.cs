using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly TaskService _taskService;

        public List<TodoTask> Tasks { get; set; }

        public IndexModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet()
        {
            Tasks = _taskService.GetAllTasks();
        }

        public IActionResult OnPostToggleStatus(int id)
        {
            _taskService.ToggleTaskCompletion(id);
            return new JsonResult(new { success = true });
        }
    }
}