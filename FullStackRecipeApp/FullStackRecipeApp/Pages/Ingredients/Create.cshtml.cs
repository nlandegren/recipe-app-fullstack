using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Ingredients
{
    public class CreateModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl AccessControl;

        public CreateModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        public IActionResult OnGet()
        {
            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            return Page();
        }

        [BindProperty]
        public Ingredient Ingredient { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Ingredient.UserID = AccessControl.LoggedInUserID;
            database.Ingredient.Add(Ingredient);

            await database.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
