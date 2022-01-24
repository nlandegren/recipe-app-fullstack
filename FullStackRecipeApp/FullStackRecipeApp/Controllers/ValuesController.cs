using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly RecipeDbContext database;    
        
        public ValuesController(RecipeDbContext database)
        {
            this.database = database;
        }




        // Step 1: Get all recipies
        // Step 2: Get all recipies filtered by diet
        // Step 3: Get all recipies and search by 1 keyword
        // Step 4: Get all recipies and search by multiple keywords
        //GET: /Recipes? diet = pescetarian
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe(string filterKey)
        //{
        //    // Filter by diet
        //    if (filterKey != DietCategory.All)
        //    {
        //        if (filterKey == DietCategory.Karnivor)
        //        {
        //            query = query
        //            .Where(p => p.Meals
        //            .Any(m => m.Recipe.Quantities
        //            .Any(q => q.Ingredient.DietCategory == filterKey)));
        //        }
        //        else if (filterKey == DietCategory.Pesceterian)
        //        {
        //            query = query
        //            .Where(p => p.Meals
        //            .Any(m => !m.Recipe.Quantities
        //            .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor)));
        //        }
        //        else if (filterKey == DietCategory.LactoOvo)
        //        {
        //            query = query
        //            .Where(p => p.Meals
        //            .Any(m => !m.Recipe.Quantities
        //            .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor ||
        //                      q.Ingredient.DietCategory == DietCategory.Pesceterian)));
        //        }
        //        else if (filterKey == DietCategory.Vegetarisk)
        //        {
        //            query = query
        //            .Where(p => p.Meals
        //            .Any(m => !m.Recipe.Quantities
        //            .Any(q => q.Ingredient.DietCategory == DietCategory.Karnivor ||
        //                      q.Ingredient.DietCategory == DietCategory.Pesceterian ||
        //                      q.Ingredient.DietCategory == DietCategory.LactoOvo)));
        //        }
        //    }
        //}
    }
}
