using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly RecipeDbContext database;

        public IndexModel(RecipeDbContext context)
        {
            database = context;
        }

        public IList<Recipe> Recipes { get;set; }

        public async Task OnGetAsync()
        {
            Recipes = await database.Recipe.ToListAsync();
        }
    }
}
