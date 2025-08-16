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
        }

        public void OnGet()
        {
            Task = new TodoTask();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _taskService.AddTask(Task);
            return RedirectToPage("./Index");
        }
    }
}