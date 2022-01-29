using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.UnitsOfMeasurement
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

        public async Task<IActionResult> OnPostAsync(Unit unit)
        {

            if (!AccessControl.IsLoggedIn() || !AccessControl.UserHasAccess(Unit))
            {
                return StatusCode(401, "Oops! You do not have access to this page!");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            database.Attach(Unit).State = EntityState.Modified;

            await database.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
