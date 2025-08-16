using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly TaskService _taskService;

        [BindProperty]
        public TodoTask Task { get; set; }

        public DeleteModel(TaskService taskService)
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
            _taskService.DeleteTask(Task.Id);
            return RedirectToPage("./Index");
        }
    }
}