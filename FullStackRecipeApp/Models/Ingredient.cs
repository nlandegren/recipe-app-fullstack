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
        All,
        [Display(Name = "Lakto-ovo")]
        LactoOvo,
        Vegetarisk,
        Karnivor,
        Pesceterian,
        Annan
    }

    public class Ingredient
    {
        public int ID { get; set; }
        [Display(Name = "Ingrediensnamn")]
        [Required]
        public string Name { get; set; }
        
        [Required, Display(Name = "Matkategori")]
        public string FoodCategory { get; set; }
        [Display(Name = "Dietkategori")]
        public DietCategory DietCategory { get; set; }
        public List<Quantity> Quantities { get; set; }
  
        public string UserID { get; set; }
    }
}
