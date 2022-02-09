using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;

namespace FullStackRecipeApp.Pages.UnitsOfMeasurement
{
    public class IndexModel : PageModel
    {
        private readonly RecipeDbContext database;
        public AccessControl AccessControl;

        public IndexModel(RecipeDbContext context, AccessControl accessControl)
        {
            database = context;
            this.AccessControl = accessControl;
        }

        public IList<Unit> Unit { get;set; }

        public async Task OnGetAsync()
        {
            Unit = await database.Unit.ToListAsync();
        }
    }
}
