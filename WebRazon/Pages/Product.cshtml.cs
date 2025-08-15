using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazon.Pages
{
    public class ProductModel : PageModel
    {
        public void OnGet()
        {
            // In a real application, you would load product data from a database
            // based on the product ID passed in the URL
        }
    }
}