using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FullStackRecipeApp.Pages.Recipes
{
    public enum MealCategory
    {
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
    public class CreateModel : PageModel
    {
        private readonly RecipeDbContext database;
        private readonly AccessControl accessControl;

        public CreateModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.accessControl = accessControl;
        }

        public bool IsLoggedIn { get; set; }

        [BindProperty]
        public Recipe Recipe { get; set; }


        private void CreateEmptyRecipe()
        {
            Recipe = new Recipe
            {
                UserID = accessControl.LoggedInUserID
            };
        }

        public IActionResult OnGet()
        {
            CreateEmptyRecipe();

            if (!accessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(Recipe recipe)
        {
            CreateEmptyRecipe();

            if (!accessControl.IsLoggedIn())
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Recipe.Name = recipe.Name;
            Recipe.Description = recipe.Description;
            Recipe.Instructions = recipe.Description;
            Recipe.MealCategory = recipe.MealCategory;

            database.Recipe.Add(recipe);
            
            await database.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = recipe.ID});
        }
    }
}
