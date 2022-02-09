using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.MealPlans
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

        [BindProperty]
        public MealPlan MealPlan { get; set; }


        public IActionResult OnGet()
        {

            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(MealPlan mealPlan)
        {
            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MealPlan = mealPlan;
            MealPlan.UserID = AccessControl.LoggedInUserID;

            await database.MealPlan.AddAsync(MealPlan);
            await database.SaveChangesAsync();
            return RedirectToPage("./Edit", new { id = MealPlan.ID });
        }
    }
}
