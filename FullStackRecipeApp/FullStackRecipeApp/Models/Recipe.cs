using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Models
{
    public enum MealCategory
    {
        [Display(Name = "Frukost")]
        Frukost,
        [Display(Name = "Brunch")]
        Brunch,
        [Display(Name = "Lunch")]
        Lunch,
        [Display(Name = "Middag")]
        Middag,
        [Display(Name = "Annan")]
        Annan
    }

    public enum CourseCategory
    {
        Starter,
        Main,
        Dessert
    }

    public class Recipe : IValidatableObject
    {
        public int ID { get; set; }
        [Required, Display(Name = "Namn")]
        public string Name { get; set; }
        [Required, Display(Name = "Instruktioner")]
        public string Instructions { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Målkategori")]
        public MealCategory MealCategory { get; set; }
        public CourseCategory CourseCategory { get; set; }

        public int Difficulty { get; set; }

        public List<Quantity> Quantities { get; set; }
        public List<RecipeMealPlan> Meals { get; set; }
        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Difficulty < 1 || Difficulty > 5)
            {
                yield return new ValidationResult(
                    "The difficulty of a recipe must be between 1 and 5",
                    new[] { nameof(Difficulty) }
                );
            }
        }
    }

    public class Unit
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }

    [Index(nameof(IngredientID), nameof(RecipeID), IsUnique = true)]
    public class Quantity
    {
        [Required]
        public int IngredientID { get; set; }
        [Required]
        public int RecipeID { get; set; }
        [Required]
        public int MeasurementID { get; set; }
        public double? Amount { get; set; }
        
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
        public Unit Measurement { get; set; }

    }
}
