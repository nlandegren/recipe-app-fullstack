using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Models
{
    public enum WeekDay
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }
    public class MealPlan
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int WeekNumber { get; set; }

        public List<RecipeMealPlan> Meals { get; set; }
        // make not foreign key
        //public string UserID { get; set; }
    }

    public class RecipeMealPlan
    {
        public int ID { get; set; }
        public int RecipeID { get; set; }
        public int MealPlanID { get; set; }
        public WeekDay WeekDay { get; set; }
        public Recipe Recipe { get; set; }

        public MealPlan MealPlan { get; set; }
    }
}
