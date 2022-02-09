using FullStackRecipeApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FullStackRecipeApp.Data
{
    public class RecipeDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public RecipeDbContext(DbContextOptions<RecipeDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.UseCollation("Finnish_Swedish_CI_AS");
            
            base.OnModelCreating(builder);
            builder.Entity<Quantity>()
                .HasKey(i => new { i.IngredientID, i.RecipeID });

            builder.Entity<Recipe>()
                .Property(r => r.MealCategory)
                .HasConversion<string>();

            builder.Entity<Ingredient>()
                .Property(i => i.DietCategory)
                .HasConversion<string>();

            builder.Entity<RecipeMealPlan>()
                .Property(i => i.WeekDay)
                .HasConversion<string>();

            builder.Entity<Recipe>()
                .Property(r => r.Difficulty)
                .HasDefaultValue(1);

        }

        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Quantity> Quantity { get; set; }
        public DbSet<MealPlan> MealPlan { get; set; }
        public DbSet<RecipeMealPlan> RecipeMealPlan { get; set; }


    }
}
