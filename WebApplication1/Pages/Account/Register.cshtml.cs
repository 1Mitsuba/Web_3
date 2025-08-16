using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;

namespace WebApplication1.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserService _userService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(UserService userService, ILogger<RegisterModel> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El nombre es obligatorio")]
            [Display(Name = "Nombre")]
            public string FirstName { get; set; }
            
            [Required(ErrorMessage = "El apellido es obligatorio")]
            [Display(Name = "Apellido")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
            [Display(Name = "Nombre de usuario")]
            [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} y como m�ximo {1} caracteres.", MinimumLength = 3)]
            public string Username { get; set; }

            [Required(ErrorMessage = "El correo electr�nico es obligatorio")]
            [EmailAddress(ErrorMessage = "El correo electr�nico no tiene un formato v�lido")]
            [Display(Name = "Correo electr�nico")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La contrase�a es obligatoria")]
            [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contrase�a")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contrase�a")]
            [Compare("Password", ErrorMessage = "La contrase�a y la confirmaci�n no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Account/Login");

            if (ModelState.IsValid)
            {
                try
                {
                    // Create the user
                    var user = new User
                    {
                        Username = Input.Username,
                        Email = Input.Email,
                        PasswordHash = UserService.HashPassword(Input.Password),
                        FirstName = Input.FirstName,
                        LastName = Input.LastName
                    };

                    _userService.AddUser(user);

                    _logger.LogInformation("Usuario creado con �xito: {Username}", user.Username);
                    
                    TempData["SuccessMessage"] = "Registro exitoso. Ahora puedes iniciar sesi�n.";
                    return RedirectToPage("Login");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}