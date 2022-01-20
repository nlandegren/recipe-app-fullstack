using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FullStackRecipeApp.Pages.Ingredients
{
    public enum SortingKey
    {
        [Display(Name = "Namn")]
        Name,
        [Display(Name = "Popularitet")]
        Popularity
    }

    public enum FilterKey
    {
        [Display(Name = "Alla")]
        All,
        [Display(Name = "Lakto-ovo")]
        LactoOvo,
        [Display(Name = "Vegetarisk")]
        Vegetarisk,
        [Display(Name = "Karnivor")]
        Karnivor,
        [Display(Name = "Pesceterian")]
        Pesceterian
    }
    public class IndexModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl accessControl;

        public IndexModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.accessControl = accessControl;
        }

        public bool IsLoggedIn { get; set; }

        public IList<Ingredient> Ingredients { get;set; }
        
        [FromQuery]
        public SortingKey SortingKey { get; set; }
        [FromQuery]
        public FilterKey FilterKey { get; set; }

        public async Task OnGetAsync()
        {
            IsLoggedIn = accessControl.IsLoggedIn();

            Ingredients = await database.Ingredient.Include(i => i.Quantities).ToListAsync();

            var query = database.Ingredient.AsNoTracking();

            if (FilterKey == FilterKey.Karnivor)
            {
                query = query.Where(i => i.DietCategory == DietCategory.Karnivor);
            }
            else if (FilterKey == FilterKey.LactoOvo)
            {
                query = query.Where(i => i.DietCategory == DietCategory.LactoOvo);
            }
            else if (FilterKey == FilterKey.Vegetarisk)
            {
                query = query.Where(i => i.DietCategory == DietCategory.Vegetarisk);
            }

            if (SortingKey == SortingKey.Name)
            {
                query = query.OrderBy(i => i.Name);
            }
            else if (SortingKey == SortingKey.Popularity)
            {
                query = query.OrderByDescending(i => i.Quantities.Count);
            }
           
            Ingredients = await query.ToListAsync();
        }
    }
}
