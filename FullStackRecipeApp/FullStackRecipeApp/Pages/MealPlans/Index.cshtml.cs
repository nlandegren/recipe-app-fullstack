using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using System.ComponentModel.DataAnnotations;


namespace FullStackRecipeApp.Pages.MealPlans
{
    public enum SortingKey
    {
        [Display(Name = "Veckonummer")]
        WeekNumber,
        [Display(Name = "Antal recept")]
        RecipeCount
    }

    public enum FilterKey
    {
        [Display(Name = "Alla")]
        All,
        [Display(Name = "Lakto-ovo")]
        LactoOvo,
        Vegetarisk,
        Karnivor,
        Pesceterian
    }
    public class IndexModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public IndexModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        public IList<MealPlan> MealPlans { get;set; }

        [FromQuery]
        public SortingKey SortingKey { get; set; }
        [FromQuery]
        public DietCategory FilterKey { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {            

            var query = database.MealPlan
                    .Include(p => p.Meals)
                    .AsNoTracking();


            // Filter by diet
            if (FilterKey != DietCategory.Karnivor)
            {
                if (FilterKey == DietCategory.Pesceterian)
                {
                    query = query
                    .Where(p => !p.Meals
                    .Any(m => m.Recipe.Quantities
                    .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor)));
                }
                else if (FilterKey == DietCategory.LactoOvo)
                {
                    query = query
                    .Where(p => !p.Meals
                    .Any(m => m.Recipe.Quantities
                    .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor ||
                              q.Ingredient.DietCategory == DietCategory.Pesceterian)));
                }
                else if (FilterKey == DietCategory.Vegetarisk)
                {
                    query = query
                    .Where(p => !p.Meals
                    .Any(m => m.Recipe.Quantities
                    .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor ||
                              q.Ingredient.DietCategory == DietCategory.Pesceterian ||
                              q.Ingredient.DietCategory == DietCategory.LactoOvo)));
                }
            }


            if (SearchTerm != null)
            {
                query = query.Where(m =>
                    m.Name.ToLower().Contains(SearchTerm.ToLower())
                );
            }

            // Sort by week number
            if (SortingKey == SortingKey.WeekNumber)
            {
                query = query.OrderByDescending(m => m.WeekNumber);
            }

            // Sort by number of recipes
            if (SortingKey == SortingKey.RecipeCount)
            {
                query = query.OrderByDescending(m => m.Meals.Count);
            }


            MealPlans = await query.ToListAsync();



            return Page();
        }
    }
}
