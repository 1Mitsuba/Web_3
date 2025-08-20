using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly ILogger<ProfileModel> _logger;

        public ProfileModel(UserService userService, TaskService taskService, ILogger<ProfileModel> logger)
        {
            _userService = userService;
            _taskService = taskService;
            _logger = logger;
        }

        [BindProperty]
        public User UserProfile { get; set; }

        [BindProperty]
        public PasswordChangeModel PasswordModel { get; set; }

        // Renamed to avoid shadowing the User property from PageModel
        public User CurrentUser { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int UrgentTasks { get; set; }

        public class PasswordChangeModel
        {
            [Required(ErrorMessage = "La contraseña actual es obligatoria")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña actual")]
            public string CurrentPassword { get; set; }

            [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva contraseña")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar nueva contraseña")]
            [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public IActionResult OnGet()
        {
            LoadUserData();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadUserData();
                return Page();
            }

            try
            {
                _userService.UpdateUser(UserProfile);
                TempData["SuccessMessage"] = "Perfil actualizado correctamente";
                return RedirectToPage();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                LoadUserData();
                return Page();
            }
        }

        public IActionResult OnPostChangePassword()
        {
            if (!ModelState.IsValid)
            {
                LoadUserData();
                return Page();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userService.GetUserById(userId);

            var isValid = _userService.ValidateUser(user.Username, PasswordModel.CurrentPassword);
            if (!isValid)
            {
                ModelState.AddModelError("PasswordModel.CurrentPassword", "La contraseña actual es incorrecta");
                LoadUserData();
                return Page();
            }

            try
            {
                _userService.ChangePassword(userId, PasswordModel.NewPassword);
                TempData["SuccessMessage"] = "Contraseña cambiada correctamente";
                return RedirectToPage();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                LoadUserData();
                return Page();
            }
        }

        private void LoadUserData()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            CurrentUser = _userService.GetUserById(userId);
            
            if (UserProfile == null)
            {
                UserProfile = new User
                {
                    Id = CurrentUser.Id,
                    Username = CurrentUser.Username,
                    Email = CurrentUser.Email,
                    FirstName = CurrentUser.FirstName,
                    LastName = CurrentUser.LastName
                };
            }

            // For simplicity, we're just showing all tasks - in a real app, you'd filter by user
            TotalTasks = _taskService.GetAllTasks().Count;
            CompletedTasks = _taskService.GetCompletedTasks().Count;
            PendingTasks = _taskService.GetPendingTasks().Count;
            UrgentTasks = _taskService.GetUrgentTasks().Count;
        }
    }
}