using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.UnitsOfMeasurement
{
    public class CreateModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

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
        public Unit Unit { get; set; }

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

            Unit.UserID = AccessControl.LoggedInUserID;

            database.Unit.Add(Unit);
            await database.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
