using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Models
{

    public enum DietCategory
    {
        LactoOvo,
        Vegetarisk,
        Karnivor,
        Pesceterian,
        Annan
    }

    public class Ingredient
    {
        public int ID { get; set; }
        [Display(Name = "Ingredient")]
        [Required]
        public string Name { get; set; }
        [Required]
        public string FoodCategory { get; set; }
        public DietCategory DietCategory { get; set; }
        public List<Quantity> Quantities { get; set; }

        //[Required]
        //public string UserID { get; set; }
        //public IdentityUser User { get; set; }
    }
}
