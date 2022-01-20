﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.Recipes
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
        public Recipe Recipe { get; set; }

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

            Recipe = await database.Recipe.FirstOrDefaultAsync(m => m.ID == id);

            if (!accessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            if (Recipe == null)
            {
                return NotFound();
            }
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

            Recipe = await database.Recipe.FindAsync(id);

            if (!accessControl.UserHasAccess(Recipe))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }

            if (Recipe != null)
            {
                database.Recipe.Remove(Recipe);
                await database.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
