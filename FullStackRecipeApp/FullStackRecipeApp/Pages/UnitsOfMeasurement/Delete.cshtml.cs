using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.UnitsOfMeasurement
{
    public class DeleteModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public DeleteModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }
        public bool IsLoggedIn { get; set; }


        [BindProperty]
        public Unit Unit { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            Unit = await database.Unit.FirstOrDefaultAsync(m => m.ID == id);


            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Unit))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }


            if (Unit == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Unit = await database.Unit.FindAsync(id);

            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Unit))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (id == null)
            {
                return NotFound();
            }

            database.Unit.Remove(Unit);
            await database.SaveChangesAsync();
            

            return RedirectToPage("./Index");
        }
    }
}
