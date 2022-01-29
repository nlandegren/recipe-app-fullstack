using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FullStackRecipeApp.Pages.Recipes
{

    public enum SortingKey
    {
        [Display(Name = "Ingen sortering")]
        NoSorting,
        [Display(Name = "Namn")]
        Name,
        [Display(Name = "Antal ingredienser")]
        IngredientCount
    }

    public enum FilterKey
    {
        [Display(Name = "Alla")]
        All,
        [Display(Name = "Frukost")]
        Breakfast,
        [Display(Name = "Brunch")]
        Brunch,
        [Display(Name = "Lunch")]
        Lunch,
        [Display(Name = "Middag")]
        Dinner,
        [Display(Name = "Annan")]
        Other
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

        public IList<Recipe> Recipes { get;set; }

        [FromQuery]
        public SortingKey SortingKey { get; set; }
        [FromQuery]
        public FilterKey FilterKey { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {

            var query = database.Recipe.AsNoTracking();

            if (SearchTerm != null)
            {
                query = query.Where(r =>
                    r.Name.ToLower().Contains(SearchTerm.ToLower()) ||
                    r.Description.ToLower().Contains(SearchTerm.ToLower())
                );
            }


            if (FilterKey == FilterKey.Breakfast)
            {
                query = query.Where(r => r.MealCategory == Models.MealCategory.Frukost);
            }
            else if (FilterKey == FilterKey.Brunch)
            {
                query = query.Where(r => r.MealCategory == Models.MealCategory.Brunch);
            }
            else if (FilterKey == FilterKey.Lunch)
            {
                query = query.Where(i => i.MealCategory == Models.MealCategory.Lunch);
            }
            else if (FilterKey == FilterKey.Dinner)
            {
                query = query.Where(i => i.MealCategory == Models.MealCategory.Middag);
            }
            else if (FilterKey == FilterKey.Other)
            {
                query = query.Where(i => i.MealCategory == Models.MealCategory.Annan);
            }


            if (SortingKey == SortingKey.Name)
            {
                query = query.OrderBy(i => i.Name);
            }
            else if (SortingKey == SortingKey.IngredientCount)
            {
                query = query.OrderByDescending(i => i.Quantities.Count);
            }

            Recipes = await query.ToListAsync();
        }
    }
}
