using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.MealPlans
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
        public IList<int> SelectedWeekDayIDs { get; set; }

        [BindProperty]
        public IList<int> SelectedRecipeIDs { get; set; }
        
        [BindProperty]
        public int NewWeekDayID { get; set; }
        [BindProperty]
        public int NewRecipeID { get; set; }


        [BindProperty]
        public MealPlan MealPlan { get; set; }

        [BindProperty]
        public int RecipeID { get; set; }
        [BindProperty]
        public WeekDay WeekDay { get; set; }

        public List<RecipeMealPlan> PlannedMeals { get; set; }


        public RecipeMealPlan RecipeMealPlan { get; set; }

        public List<SelectListItem> RecipeOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            MealPlan = await database.MealPlan
                .Include(m => m.Meals)
                .ThenInclude(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (MealPlan == null)
            {
                return NotFound();
            }
            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(MealPlan))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            PlannedMeals = MealPlan.Meals.ToList();



            RecipeOptions = await database.Recipe.Select(r =>
                  new SelectListItem
                  {
                      Value = r.ID.ToString(),
                      Text = r.Name
                  }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(MealPlan mealPlan)
        {
            MealPlan = await database.MealPlan
                .Include(m => m.Meals)
                .ThenInclude(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.ID == mealPlan.ID);

            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(MealPlan))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();

            }

            await SaveMealPlan(mealPlan);

            return RedirectToPage("./Edit", new { id = MealPlan.ID });
        }


        private async Task<IActionResult> SaveMealPlan(MealPlan mealPlan)
        {

            MealPlan = await database.MealPlan
                .Include(m => m.Meals)
                .ThenInclude(m => m.Recipe)
                .FirstOrDefaultAsync(m => m.ID == mealPlan.ID);

            PlannedMeals = MealPlan.Meals.ToList();

            MealPlan.Name = mealPlan.Name;
            MealPlan.WeekNumber = mealPlan.WeekNumber;
            
            await database.SaveChangesAsync();

            // Save edited planned recipes

            for (int i = 0; i < SelectedWeekDayIDs.Count; i++)
            {
                // Get weekday
                int weekdayID = SelectedWeekDayIDs[i];
                var weekday = (WeekDay)weekdayID;

                // Get recipe
                var recipeID = SelectedRecipeIDs[i];
                var recipe = await database.Recipe
                    .Where(r => r.ID == recipeID)
                    .SingleAsync();

                // Get recipe in join table RecipeMealPlan
                RecipeMealPlan = PlannedMeals[i];

                // Update values in case they have changed
                RecipeMealPlan.Recipe = recipe;
                RecipeMealPlan.WeekDay = weekday;
                RecipeMealPlan.MealPlan = MealPlan;

                await database.SaveChangesAsync();
            }

            // If place holder fields are posted then stop.
            if (NewWeekDayID == -1 || NewRecipeID == -1)
            {
                return RedirectToPage("./Edit", new { id = MealPlan.ID });
            }
            else
            {
                // Get weekday
                var weekday = (WeekDay)NewWeekDayID;

                // Get recipe
                var recipeID = NewRecipeID;
                var recipe = await database.Recipe
                    .Where(r => r.ID == recipeID)
                    .SingleAsync();

                RecipeMealPlan = new RecipeMealPlan
                {
                    Recipe = recipe,
                    WeekDay = weekday,
                    MealPlan = MealPlan
                };

                database.RecipeMealPlan.Add(RecipeMealPlan);

                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Edit", new { id = MealPlan.ID });
        }

        public async Task<IActionResult> OnPostDelete(int recipeMealPlanID, MealPlan mealPlan)
        {

            await SaveMealPlan(mealPlan);

            var recipeMealPlan = await database.RecipeMealPlan
                .Where(m => m.ID == recipeMealPlanID)
                .SingleOrDefaultAsync();

            database.RecipeMealPlan.Remove(recipeMealPlan);
            await database.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = MealPlan.ID });
        }

    }
}
