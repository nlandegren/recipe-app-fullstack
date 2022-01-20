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
        [Display(Name = "Måndag")]
        Monday,
        [Display(Name = "Tisdag")]
        Tuesday,
        [Display(Name = "Onsdag")]
        Wednesday,
        [Display(Name = "Torsdag")]
        Thursday,
        [Display(Name = "Fredag")]
        Friday,
        [Display(Name = "Lördag")]
        Saturday,
        [Display(Name = "Söndag")]
        Sunday,
    }
    public class MealPlan
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int WeekNumber { get; set; }

        public List<RecipeMealPlan> Meals { get; set; }
        //[Required]
        //public string UserID { get; set; }
        //public IdentityUser User { get; set; }
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
