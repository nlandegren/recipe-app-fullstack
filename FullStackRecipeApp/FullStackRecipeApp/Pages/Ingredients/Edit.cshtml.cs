using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Ingredients
{
    public class EditModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public EditModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        [BindProperty]
        public Ingredient Ingredient { get; set; }

        public DietCategory Diet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Ingredient = await database.Ingredient.FirstOrDefaultAsync(m => m.ID == id);
            
            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Ingredient))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            

            if (Ingredient == null)
            {
                return NotFound();
            }
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Ingredient))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            database.Attach(Ingredient).State = EntityState.Modified;
            await database.SaveChangesAsync();


            return RedirectToPage("./Index");
        }

    }
}
