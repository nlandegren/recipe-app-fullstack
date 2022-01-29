using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Recipes
{
    public class DeleteModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl AccessControl;

        public DeleteModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            
            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await database.Recipe.FirstOrDefaultAsync(m => m.ID == id);

            if (!AccessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await database.Recipe.FindAsync(id);

            if (!AccessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            if (Recipe != null)
            {
                database.Recipe.Remove(Recipe);
                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
