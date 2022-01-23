using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using System.Text.RegularExpressions;

namespace FullStackRecipeApp.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public DetailsModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }
        public Recipe Recipe { get; set; }
        public IList<Quantity> Quantities { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Recipe = await database.Recipe.FirstOrDefaultAsync(m => m.ID == id);
            Quantities = await database.Quantity
                .Include(q => q.Ingredient)
                .Include(q => q.Measurement)
                .Where(q => q.Recipe == Recipe)
                .ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
