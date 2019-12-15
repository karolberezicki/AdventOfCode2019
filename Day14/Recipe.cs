using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Day14
{
    [DebuggerDisplay("{" + nameof(RecipeDescription) + ",nq}")]
    public class Recipe
    {
        public string RecipeDescription { get; set; }
        public string OutputChemical { get; }
        public long OutputCount { get; }
        public Dictionary<string, long> Ingredients { get; set; }

        public Recipe(string recipeDescription)
        {
            RecipeDescription = recipeDescription;
            var recipe = RecipeDescription.Split(new[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
            var product = recipe[1].Split(" ");
            OutputChemical = product[1].Trim();
            OutputCount = long.Parse(product[0].Trim());
            Ingredients = new Dictionary<string, long>();
            foreach (var ingredientDescription in recipe[0].Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                var ingredient = ingredientDescription.Trim().Split(" ");
                Ingredients.Add(ingredient[1].Trim(), long.Parse(ingredient[0].Trim()));
            }
        }
    }
}