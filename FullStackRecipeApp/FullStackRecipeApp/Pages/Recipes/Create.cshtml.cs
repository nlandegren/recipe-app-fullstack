﻿using System;
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

        public IActionResult OnGet()
        {
            IsLoggedIn = accessControl.IsLoggedIn();
            if (!IsLoggedIn)
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsLoggedIn = accessControl.IsLoggedIn();
            if (!IsLoggedIn)
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            Recipe.UserID = accessControl.LoggedInUserID;
            // Because UserID was added after model state evaluation, we need to remove error associated with it. This might cause problems if there are other errors.
            ModelState.Remove("Recipe.UserID");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            database.Recipe.Add(Recipe);
            
            await database.SaveChangesAsync();

            return RedirectToPage("./Edit", new { id = Recipe.ID});
        }
    }
}
