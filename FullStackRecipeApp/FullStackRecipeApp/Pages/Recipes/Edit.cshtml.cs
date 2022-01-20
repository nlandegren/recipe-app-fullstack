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
        private readonly AccessControl accessControl;

        public EditModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.accessControl = accessControl;
        }

        public bool IsLoggedIn { get; set; }

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

            Recipe = await database.Recipe
                .Include(r => r.Quantities)
                .FirstOrDefaultAsync(m => m.ID == id);

            
            if (!accessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            RecipeIngredients = Recipe.Quantities.ToList();
                

            if (Recipe == null)
            {
                return NotFound();
            }

            IngredientOptions = await database.Ingredient.Select(i =>
              new SelectListItem
              {
                  Value = i.ID.ToString(),
                  Text = i.Name
              }).ToListAsync();

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
        public async Task<IActionResult> OnPostAsync()
        {
            IsLoggedIn = accessControl.IsLoggedIn();
            if (!IsLoggedIn)
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            Recipe.UserID = accessControl.LoggedInUserID;
            ModelState.Remove("Recipe.UserID");

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!accessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            database.Attach(Recipe).State = EntityState.Modified;

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

            return RedirectToPage("./Index");
        }

        private bool RecipeExists(int id)
        {
            return database.Recipe.Any(e => e.ID == id);
        }
    }
}
