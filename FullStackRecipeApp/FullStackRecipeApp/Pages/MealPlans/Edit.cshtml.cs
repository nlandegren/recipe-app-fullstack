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
        private readonly AccessControl accessControl;

        public EditModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.accessControl = accessControl;
        }
        public bool IsLoggedIn { get; set; }

        [BindProperty]
        public MealPlan MealPlan { get; set; }
        
        [BindProperty]
        public int RecipeID { get; set; }
        [BindProperty]
        public WeekDay WeekDay { get; set; }
        
        public RecipeMealPlan RecipeMealPlan { get; set; }

        public List<SelectListItem> Options { get; set; }

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

            MealPlan = await database.MealPlan.Where(m => m.ID == id).SingleAsync();

            if (MealPlan == null)
            {
                return NotFound();
            }

            Options = await database.Recipe.Select(r =>
                  new SelectListItem
                  {
                      Value = r.ID.ToString(),
                      Text = r.Name
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var recipe = await database.Recipe.Where(r => r.ID == RecipeID).SingleAsync();

            RecipeMealPlan = new RecipeMealPlan
            {
                WeekDay = WeekDay,
                Recipe = recipe,
                MealPlan = MealPlan
            };

            database.RecipeMealPlan.Add(RecipeMealPlan);                

            database.Attach(MealPlan).State = EntityState.Modified;
            

            try
            {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MealPlanExists(MealPlan.ID))
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

        private bool MealPlanExists(int id)
        {
            return database.MealPlan.Any(e => e.ID == id);
        }
    }
}
