using FullStackRecipeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Data
{
    public class AccessControl
    {
        public string LoggedInUserID { get; set; }
        public AccessControl(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            LoggedInUserID = userManager.GetUserId(httpContextAccessor.HttpContext.User);
        }

        public bool IsLoggedIn()
        {
            return LoggedInUserID != null;
        }

        public bool UserHasAccess(Recipe recipe)
        {
            return recipe.UserID == LoggedInUserID;
        }

        public bool UserHasAccess(Ingredient ingredient)
        {
            return ingredient.UserID == LoggedInUserID;
        }

        public bool UserHasAccess(MealPlan mealPlan)
        {
            return mealPlan.UserID == LoggedInUserID;
        }
        public bool UserHasAccess(Unit unit)
        {
            return unit.UserID == LoggedInUserID;
        }
    }
}
