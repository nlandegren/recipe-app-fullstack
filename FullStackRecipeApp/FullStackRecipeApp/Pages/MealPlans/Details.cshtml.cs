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

namespace FullStackRecipeApp.Pages.MealPlans
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

        public IDictionary<string, Dictionary<string, double?>> ShoppingList;
        public MealPlan MealPlan { get; set; }
        public List<RecipeMealPlan> PlannedMeals { get; set; }

        public List<WeekDay> WeekDays { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MealPlan = await database.MealPlan
                .Include(m => m.Meals)
                .ThenInclude(m => m.Recipe)
                .ThenInclude(r => r.Quantities)
                .ThenInclude(q => q.Ingredient)                
                .FirstAsync(m => m.ID == id);

            PlannedMeals = MealPlan.Meals.ToList();

            var mealPlanIngredients = await database.RecipeMealPlan
                .Where(r => r.MealPlanID == MealPlan.ID)
                .Include(m => m.Recipe)
                .ThenInclude(r => r.Quantities)
                .ThenInclude(q => q.Measurement)
                .Select(m => m.Recipe)
                .Select(r => r.Quantities)                
                .ToListAsync();
            
            ShoppingList = new Dictionary<string, Dictionary<string, double?>>();

            foreach (var recipeIngredients in mealPlanIngredients)
            {                
                if (recipeIngredients.Count < 1)
                {
                    break;
                }

                foreach (var recipeIngredient in recipeIngredients)
                {
                    var ingredient = recipeIngredient.Ingredient.Name;
                    var measurement = recipeIngredient.Measurement.Name;
                    var amount = recipeIngredient.Amount;
                    if (ShoppingList.ContainsKey(ingredient))
                    {
                        var amountDict = ShoppingList[ingredient];
                        if (amountDict.ContainsKey(measurement))
                        {
                            amountDict[measurement] += amount;
                        }
                        else
                        {
                            amountDict[measurement] = amount;
                        }
                    }
                    else
                    {
                        ShoppingList[ingredient] = new Dictionary<string, double?>
                        {
                            [measurement] = amount
                        };
                    }
                }
            }

            WeekDays = PlannedMeals.Select(m => m.WeekDay).Distinct().OrderBy(w => w).ToList();

            if (MealPlan == null)
            {
                return NotFound();
            }
            return Page();
        }
        
    }
}
