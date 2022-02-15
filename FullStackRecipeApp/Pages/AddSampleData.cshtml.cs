using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FullStackRecipeApp.Pages
{
    public class AddSampleDataModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl AccessControl;

        public AddSampleDataModel(RecipeDbContext database, AccessControl accessControl)
        {
            this.database = database;
            this.AccessControl = accessControl;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            // Empty ingredient table
            var ingredients = await database.Ingredient.ToListAsync();
            database.Ingredient.RemoveRange(ingredients);
            
            // Load ingredient csv
            var ingredientsToAdd = await LoadIngredientsCSVAsync(@"Data\Ingredients.csv");
            
            // Add ingredients to database
            await database.Ingredient.AddRangeAsync(ingredientsToAdd);
            await database.SaveChangesAsync();

            // Empty recipe table
            var recipes = await database.Recipe.ToListAsync();
            database.Recipe.RemoveRange(recipes);
            // Load recipe csv
            var recipesToAdd = await LoadRecipesCSVAsync(@"Data\Recipes.csv");
            // Add Recipes
            await database.Recipe.AddRangeAsync(recipesToAdd);
            await database.SaveChangesAsync();

            // Empty unit table
            var units = await database.Unit.ToListAsync();
            database.Unit.RemoveRange(units);
            // Load unit cscv
            var unitsToAdd = await LoadUnitsCSVAsync(@"Data\Units.csv");
            await database.Unit.AddRangeAsync(unitsToAdd);
            await database.SaveChangesAsync();

            // Empty quantity table
            var quantities = await database.Quantity.ToListAsync();
            database.Quantity.RemoveRange(quantities);

            string[] quantityLines = await LoadQuantityCSVAsync(@"Data\Quantities.csv");

            var culture = new CultureInfo("en");

            // Parse csv contents
            foreach (var line in quantityLines)
            {
                string[] columns = line.Split(';');
                string recipeName = columns[0];
                string ingredientName = columns[1];
                string unitName = columns[2];
                double amount = Double.Parse(columns[3], culture);

                // Get objects by name because id is autogenerated.

                var recipe = await database.Recipe.Where(r => r.Name == recipeName).SingleAsync();
                var ingredient = await database.Ingredient.Where(i => i.Name == ingredientName).SingleAsync();
                var unit = await database.Unit.Where(u => u.Name == unitName).SingleAsync();

                var quantity = new Quantity
                {
                    Recipe = recipe,
                    Ingredient = ingredient,
                    Measurement = unit,
                    Amount = amount
                };
                await database.Quantity.AddAsync(quantity);
            }
            await database.SaveChangesAsync();

            // Empty mealplan table
            var mealPlans = await database.MealPlan.ToListAsync();
            database.MealPlan.RemoveRange(mealPlans);
            
            
            await database.MealPlan.AddRangeAsync(await GenerateMealPlan());
            await database.SaveChangesAsync();
            

            return RedirectToPage("./Recipes/Index");
        }


        public async Task<List<MealPlan>> GenerateMealPlan()
        {
            var mealplans = new List<MealPlan>();
            var rnd = new Random();
            
            for (int i = 0; i < rnd.Next(5, 16); i++)
            {
                // WeekNumber random int from 1 to 52
                int weeknumber = rnd.Next(1, 53);
                // make mealplan object with generic name and insert into db            
                var mealplan = new MealPlan
                {
                    Name = $"Exempelplan nummer {i + 1}",
                    WeekNumber = weeknumber,
                    Meals = new List<RecipeMealPlan>(),
                    UserID = AccessControl.LoggedInUserID
                };
                // grab 7 random recipes from db
                for (int j = 0; j < rnd.Next(5, 11); j++)
                {
                    int toSkip = rnd.Next(1, database.Recipe.Count());
                    var recipe = await database.Recipe.Skip(toSkip).FirstAsync();
                    // Weekday random int from 1 to 7
                    int weekday = rnd.Next(0, 7);
                    var mealPlanRecipe = new RecipeMealPlan
                    {
                        Recipe = recipe,
                        MealPlan = mealplan,
                        WeekDay = (WeekDay)weekday
                    };
                    
                    mealplan.Meals.Add(mealPlanRecipe);
                }
                
                mealplans.Add(mealplan);
            }

            return mealplans;
        }

        public async Task<string[]> LoadQuantityCSVAsync(string path)
        {
            return await System.IO.File.ReadAllLinesAsync(path);
        }

        public async Task<List<Ingredient>> LoadIngredientsCSVAsync(string path)
        {
            string[] lines = await System.IO.File.ReadAllLinesAsync(path);


            var ingredientsToAdd = new List<Ingredient>();


            foreach (var line in lines)
            {
                var columns = line.Split(';');
                string name = columns[0].Trim();
                string foodCategory = columns[1].Trim();
                var dietCategory = (DietCategory)Enum.Parse(typeof(DietCategory), columns[2]);

                var ingredient = new Ingredient
                {
                    Name = name,
                    FoodCategory = foodCategory,
                    DietCategory = dietCategory,
                    UserID = AccessControl.LoggedInUserID
                };
                ingredientsToAdd.Add(ingredient);
            }
            return ingredientsToAdd;
        }

        public async Task<List<Recipe>> LoadRecipesCSVAsync(string path)
        {            
            string[] lines = await System.IO.File.ReadAllLinesAsync(path);

            var rnd = new Random();

            var recipesToAdd = new List<Recipe>();


            foreach (var line in lines)
            {
                var columns = line.Split(';');
                string name = columns[0].Trim();
                string description = columns[1].Trim();
                string instructions = columns[2].Trim();
                var mealCategory = (MealCategory)Enum.Parse(typeof(MealCategory), columns[3]);
                var courseCategory = (CourseCategory)int.Parse(columns[4].Trim());
                var recipe = new Recipe
                {
                    Name = name,
                    Description = description,
                    Instructions = InsertNewLines(instructions),
                    MealCategory = mealCategory,
                    CourseCategory = courseCategory,
                    Difficulty = rnd.Next(1,6),
                    UserID = AccessControl.LoggedInUserID
                };
                recipesToAdd.Add(recipe);
            }
            return recipesToAdd;
        }

        public async Task<List<Unit>> LoadUnitsCSVAsync(string path)
        {
            var lines = await System.IO.File.ReadAllLinesAsync(path);
            var unitsToAdd = new List<Unit>();
            foreach (var line in lines)
            {                
                var unit = new Unit {
                    Name = line.Trim(),
                    UserID = AccessControl.LoggedInUserID
                };
                unitsToAdd.Add(unit);
            }
            return unitsToAdd;
        }


        public string InsertNewLines(string instructions)
        {
            return Regex.Replace(instructions, @"(\d\.)", "\n$1");
        }
    }
}