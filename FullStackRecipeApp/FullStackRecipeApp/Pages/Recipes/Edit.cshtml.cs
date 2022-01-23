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

namespace FullStackRecipeApp.Pages.Recipes
{
    public class EditModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public EditModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;            
            AccessControl = accessControl;
        }


        [BindProperty]
        public int NewIngredientID { get; set; }

        [BindProperty]
        public int NewMeasurementID { get; set; }
        [BindProperty]
        public int NewAmountID { get; set; }

        [BindProperty]
        public IList<int> SelectedIngredientIDs { get; set; }

        [BindProperty]
        public IList<int> SelectedMeasurementIDs { get; set; }

        [BindProperty]
        public IList<int> SelectedAmounts { get; set; }

        [BindProperty]
        public Recipe Recipe { get; set; }

        [BindProperty]
        public Quantity Quantity { get; set; }

        public IList<Quantity> RecipeIngredients { get; set; }

        public List<SelectListItem> IngredientOptions { get; set; }
        public List<SelectListItem> MeasurementOptions { get; set; }


        private void CreateEmptyRecipe()
        {
            Recipe = new Recipe
            {
                UserID = AccessControl.LoggedInUserID
            };
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            CreateEmptyRecipe();

            if (!AccessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await database.Recipe.FirstOrDefaultAsync(m => m.ID == id);
            
            
            if (!AccessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            RecipeIngredients = await database.Quantity
                .Include(q => q.Ingredient)
                .Include(q => q.Measurement)
                .Where(q => q.RecipeID == id)
                .ToListAsync();
                

            if (Recipe == null)
            {
                return NotFound();
            }

            // Only let user pick ingredients that are not currently in the recipe.
            var allowedIngredients = await database.Ingredient
                .Where(i => !i.Quantities.Any(q => q.RecipeID == Recipe.ID))         
                .ToListAsync();
                
            IngredientOptions = allowedIngredients.Select(i =>
              new SelectListItem
              {
                  Value = i.ID.ToString(),
                  Text = i.Name
              }).ToList();

            MeasurementOptions = await database.Unit.Select(i =>
            new SelectListItem
            {
                Value = i.ID.ToString(),
                Text = i.Name
            }).ToListAsync();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(Recipe recipe)
        {
            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await SaveRecipe(recipe);

            try
            {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(Recipe.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Edit", new { id = Recipe.ID });
        }

        public async Task<IActionResult> OnPostDelete(int ingredientID, int recipeID)
        {

            await SaveRecipe(Recipe);

            var quantity = await database.Quantity
                .Where(q => q.IngredientID == ingredientID && q.RecipeID == recipeID)
                .SingleOrDefaultAsync();

            database.Quantity.Remove(quantity);
            await database.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = recipeID });
        }

        private async Task<IActionResult> SaveRecipe(Recipe recipe)
        {


            Recipe = recipe;

            database.Attach(Recipe).State = EntityState.Modified;

            RecipeIngredients = await database.Quantity
                .Include(q => q.Ingredient)
                .Include(q => q.Measurement)
                .Where(q => q.RecipeID == Recipe.ID)
                .ToListAsync();

            // Save edited ingredient fields

            for (int i = 0; i < SelectedIngredientIDs.Count; i++)
            {
                // Get ingredient
                int ingredientID = SelectedIngredientIDs[i];
                var ingredient = await database.Ingredient
                    .Where(i => i.ID == ingredientID)
                    .SingleAsync();

                // Get unit of measurement
                var unitID = SelectedMeasurementIDs[i];
                var unit = await database.Unit
                    .Where(m => m.ID == unitID)
                    .SingleAsync();

                // Get amount
                double amount = SelectedAmounts[i];

                Quantity = RecipeIngredients[i];

                database.Quantity.Remove(Quantity);
                await database.SaveChangesAsync();

                Quantity = new Quantity
                {
                    Recipe = Recipe,
                    Ingredient = ingredient,
                    Measurement = unit,
                    Amount = amount
                };

                database.Quantity.Add(Quantity);

                await database.SaveChangesAsync();
            }

            // If place holder fields are posted then stop.
            // Tried IValidatableObject on Quantity but causes null reference exception.
            if (NewIngredientID == 0 || NewMeasurementID == 0 || NewAmountID == 0)
            {
                return RedirectToPage("./Edit", new { id = Recipe.ID });
            }

            // To avoid duplicate errors check if ingredient already exists in recipe.
            if (!database.Quantity.Any(q => q.IngredientID == NewIngredientID && q.RecipeID == Recipe.ID))
            {
                // Get ingredient
                int ingredientID = NewIngredientID;
                var ingredient = await database.Ingredient
                    .Where(i => i.ID == ingredientID)
                    .SingleAsync();

                // Get unit of measurement
                var unitID = NewMeasurementID;
                var unit = await database.Unit
                    .Where(m => m.ID == unitID)
                    .SingleAsync();

                // Get amount
                double amount = NewAmountID;

                Quantity = new Quantity
                {
                    Recipe = Recipe,
                    Ingredient = ingredient,
                    Measurement = unit,
                    Amount = amount
                };
                database.Quantity.Add(Quantity);

                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Edit", new { id = Recipe.ID });
        }

        private bool RecipeExists(int id)
        {
            return database.Recipe.Any(e => e.ID == id);
        }
    }
}
