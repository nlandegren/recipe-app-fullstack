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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


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
        [Display(Name = "Vegan")]
        Vegan,
        [Display(Name = "Karnivor")]
        Carnivore
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

        public IList<MealPlan> MealPlans { get;set; }

        [FromQuery]
        public SortingKey SortingKey { get; set; }
        [FromQuery]
        public FilterKey FilterKey { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsLoggedIn = accessControl.IsLoggedIn();

            MealPlans = await database.MealPlan.Include(m => m.Meals).ToListAsync();
            return Page();
        }
    }
}
