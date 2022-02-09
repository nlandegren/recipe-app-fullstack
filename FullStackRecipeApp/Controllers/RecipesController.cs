using FullStackRecipeApp.Data;
using FullStackRecipeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly RecipeDbContext database;    
        
        public RecipesController(RecipeDbContext database)
        {
            this.database = database;
        }

        //GET: /Recipes?diet=pescetarian
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes(int diet,
                                                                        int? course,
                                                                        string ingredient,                                                                        
                                                                        bool sortByName,
                                                                        bool sortByDifficulty,
                                                                        int pageNumber = 1,
                                                                        int pageSize = 4
                                                                        )
        {

            var dietFilters = new Dictionary<DietCategory, DietCategory[]>
            {
                [DietCategory.Vegetarisk] = new DietCategory[] { DietCategory.Karnivor,
                                                                    DietCategory.LactoOvo,
                                                                    DietCategory.Pesceterian},
                [DietCategory.LactoOvo] = new DietCategory[] { DietCategory.Karnivor,
                                                                  DietCategory.Pesceterian},
                [DietCategory.Pesceterian] = new DietCategory[] { DietCategory.Karnivor },
                [DietCategory.Karnivor] = new DietCategory[] {}
            };

            var query = database.Recipe.AsNoTracking();

            // Filter by diet.
            if (diet != 0)
            {
                var dietFilter = (DietCategory)diet;

                query = query
                .Where(r => !(r.Quantities
                .Any(q => dietFilters[dietFilter].Contains(q.Ingredient.DietCategory))
                ));
            }



            // Filter by course
            if (course != null)
            {
                var courseCategory = (CourseCategory)course;

                query = query.Where(r => r.CourseCategory == courseCategory);
            }


            // Filter by ingredient search word (add option for multiple)
            if (ingredient != null)
            {
                ingredient = ingredient.ToLower();
                query = query.Where(r => r.Quantities.Any(q =>
                                    q.Ingredient.Name.ToLower().Contains(ingredient)));
            }


            // Sort by difficulty
            if (sortByDifficulty)
            {
                query = query.OrderBy(r => r.Difficulty);
            }


            // Sort by recipe name
            if (sortByName)
            {
                query = query.OrderBy(r => r.Name);
            }


            // To make sure we don't skip the first three results we start at 0.
            pageNumber -= 1;

            var result = await query.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
