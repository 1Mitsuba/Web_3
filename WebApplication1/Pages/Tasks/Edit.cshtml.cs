using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TodoTask Task { get; set; }

        public EditModel(TaskService taskService)
        {
            _taskService = taskService;
        }

        public IActionResult OnGet(int id)
        {
            Task = _taskService.GetTaskById(id);

            if (Task == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Actualizar fechas basado en el estado
            var originalTask = _taskService.GetTaskById(Task.Id);
            
            // Si cambió de estado, actualizar las fechas correspondientes
            if (originalTask.Status != Task.Status)
            {
                if (Task.Status == TodoTaskStatus.Completed)
                {
                    Task.CompletedDate = System.DateTime.Now;
                    Task.CancelledDate = null;
                }
                else if (Task.Status == TodoTaskStatus.Cancelled)
                {
                    Task.CancelledDate = System.DateTime.Now;
                    Task.CompletedDate = null;
                }
                else // Pending
                {
                    Task.CompletedDate = null;
                    Task.CancelledDate = null;
                }
            }
            else
            {
                // Mantener las fechas originales si no cambió el estado
                Task.CompletedDate = originalTask.CompletedDate;
                Task.CancelledDate = originalTask.CancelledDate;
            }

            _taskService.UpdateTask(Task);

            return RedirectToPage("./Index");
        }
    }
}