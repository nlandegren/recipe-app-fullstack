using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Ingredients
{
    public class DeleteModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public DeleteModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        public bool IsLoggedIn { get; set; }
        [BindProperty]
        public Ingredient Ingredient { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Ingredient))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            Ingredient = await database.Ingredient.FindAsync(id);

            if (Ingredient != null)
            {
                database.Ingredient.Remove(Ingredient);
                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
