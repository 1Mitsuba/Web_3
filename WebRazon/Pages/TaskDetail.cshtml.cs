using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public TaskDetailModel(TaskService taskService, ILogger<TaskDetailModel> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                var task = _taskService.GetTaskById(id);
                
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

            return RedirectToPage("Index");
        }
    }
}