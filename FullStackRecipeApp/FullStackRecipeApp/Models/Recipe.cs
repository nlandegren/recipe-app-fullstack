using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Models
{
    public class Recipe
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Instruction> Instructions { get; set; }

    }

    public class Ingredient
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
    [Index(nameof(StepNumber), nameof(RecipeID), IsUnique = true)]
    public class Instruction
    {
        public int StepNumber { get; set; }
        public int RecipeID { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public class Measurement
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class Quantity
    {
        public int ID { get; set; }
        [Required, ForeignKey("StepNumber, RecipeID")]
        public Instruction Instruction { get; set; }
        [Required]
        public int IngredientID { get; set; }
        
        public Ingredient Ingredient { get; set; }
        [Required]
        public int MeasurementID { get; set; }
        public Measurement Measurement { get; set; }
        public double Amount { get; set; }
    }
}
