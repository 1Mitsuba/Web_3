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
        public List<TodoTask> UrgentTasks { get; set; }
        public List<TodoTask> DueTodayTasks { get; set; }
        public List<TodoTask> DueSoonTasks { get; set; }
        public List<TodoTask> OverdueTasks { get; set; }
        public Dictionary<string, int> TaskStats { get; set; }
        
        public IndexModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public void OnGet()
        {
            Tasks = _taskService.GetAllTasks();
            UrgentTasks = _taskService.GetUrgentTasks();
            DueTodayTasks = _taskService.GetTasksDueToday();
            DueSoonTasks = _taskService.GetTasksDueSoon();
            OverdueTasks = _taskService.GetOverdueTasks();
            TaskStats = _taskService.GetTaskStatistics();
        }

        public IActionResult OnPostToggleStatus(int id)
        {
            _taskService.ToggleTaskCompletion(id);
            return new JsonResult(new { success = true });
        }
        
        public IActionResult OnPostToggleUrgent(int id)
        {
            _taskService.ToggleTaskUrgent(id);
            return new JsonResult(new { success = true });
        }
        
        public IActionResult OnPostCancelTask(int id)
        {
            _taskService.CancelTask(id);
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostReactivateTask(int id)
        {
            _taskService.ReactivateTask(id);
            return new JsonResult(new { success = true });
        }

        public JsonResult OnGetTaskStats()
        {
            return new JsonResult(_taskService.GetTaskStatistics());
        }
    }
}