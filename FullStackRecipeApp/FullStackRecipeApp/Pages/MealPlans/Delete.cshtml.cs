using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.MealPlans
{
    public class DeleteModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl accessControl;

        public DeleteModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.accessControl = accessControl;
        }
        public bool IsLoggedIn { get; set; }

        [BindProperty]
        public MealPlan MealPlan { get; set; }

        public List<RecipeMealPlan> PlannedMeals { get; set; }

        public List<WeekDay> WeekDays { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsLoggedIn = accessControl.IsLoggedIn();
            if (!IsLoggedIn)
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            MealPlan = await database.MealPlan
                .Include(m => m.Meals)
                .ThenInclude(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.ID == id);


            if (MealPlan == null)
            {
                return NotFound();
            }

            PlannedMeals = MealPlan.Meals.ToList();

            WeekDays = PlannedMeals
                .Select(m => m.WeekDay)
                .Distinct()
                .OrderBy(w => w)
                .ToList();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            IsLoggedIn = accessControl.IsLoggedIn();
            if (!IsLoggedIn)
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            MealPlan = await database.MealPlan.FindAsync(id);

            if (MealPlan != null)
            {
                database.MealPlan.Remove(MealPlan);
                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
