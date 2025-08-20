using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TodoTask Task { get; set; }

        public CreateModel(TaskService taskService)
        {
            _taskService = taskService;
            Task = new TodoTask();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Configurar estado inicial de la tarea
            if (Task.Status == TodoTaskStatus.Completed)
            {
                Task.CompletedDate = System.DateTime.Now;
            }
            else if (Task.Status == TodoTaskStatus.Cancelled)
            {
                Task.CancelledDate = System.DateTime.Now;
            }

            _taskService.AddTask(Task);

            return RedirectToPage("./Index");
        }
    }
}