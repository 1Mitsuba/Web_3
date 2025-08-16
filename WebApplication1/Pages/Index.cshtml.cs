using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TaskService _taskService;

        public IndexModel(ILogger<IndexModel> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public int TotalTasks { get; private set; }
        public int CompletedTasks { get; private set; }
        public int PendingTasks { get; private set; }

        public void OnGet()
        {
            LoadTaskStats();
        }

        private void LoadTaskStats()
        {
            TotalTasks = _taskService.GetAllTasks().Count;
            CompletedTasks = _taskService.GetCompletedTasks().Count;
            PendingTasks = _taskService.GetPendingTasks().Count;
        }

        public JsonResult OnGetTaskStats()
        {
            LoadTaskStats();
            return new JsonResult(new { 
                totalTasks = TotalTasks,
                completedTasks = CompletedTasks,
                pendingTasks = PendingTasks
            });
        }
    }
}
