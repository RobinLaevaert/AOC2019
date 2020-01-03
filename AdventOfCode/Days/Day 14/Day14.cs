using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day_14
{
    public class Day14 : Day
    {
        public List<Recipe> Recipes;

        public Day14()
        {
            DayNumber = 14;
            Title = "Space Stoichiometry";
        }

        public override void ReadFile()
        {
            var temp = File.ReadAllLines(GetFilePath());
            Recipes = temp.Select(Recipe.Create).ToList();
        }

        public override void Part1()
        {
            ReadFile();
            
            Console.WriteLine($"Total Ores needed : {Calculate()}");
        }

        public long Calculate()
        {
            var endRecipe = Recipes.First(x => x.EndProduct.Element == "FUEL");
            endRecipe.inputIngredients.ForEach(x => x.Amount *= endRecipe.EndProduct.Amount);
            return CalculateCost(endRecipe);
        }

        public override void Part2()
        {
            
            long cost = 0;
            var fuel = 2140000;
            while (cost < 1000000000000)
            {
                ReadFile();
                var endRecipe = Recipes.First(x => x.EndProduct.Element == "FUEL");
                endRecipe.EndProduct.Amount = fuel;
                cost = Calculate();
                Console.WriteLine($"Cost for {fuel} Fuel: {cost} ORES");
                fuel+=1;
                
            }

            Console.WriteLine($"Maximum fuel is : {fuel - 2}");
        }

        public long CalculateCost(Recipe recipe)
        {
            var neededIngredients = new List<Ingredient>();
            var ExcessIngredients = new List<Ingredient>();
            long amountOfOre = 0;
            recipe.inputIngredients.ForEach(x =>
            {
                if (neededIngredients.FirstOrDefault(y => y.Element == x.Element) == null) neededIngredients.Add(x);
                else
                {
                    neededIngredients.First(y => y.Element == x.Element).Amount += x.Amount;
                }
            });

            while (neededIngredients.Count > 0)
            {
                neededIngredients = neededIngredients.OrderByDescending(x => GetStepsToOre(x, Recipes)).ToList();
                if (neededIngredients[0].Amount == 0) {neededIngredients.Remove(neededIngredients[0]);
                    continue;
                }
                var test = GetIngredientsForElement(neededIngredients[0]);

                if(test.ExcessIngredient > 0) AddExcess(neededIngredients[0], test.ExcessIngredient);

                var temp = test.NeededIngredients;

                temp.ForEach(checkForExcess);

                temp.ForEach(AddNeeded);

                neededIngredients.Remove(neededIngredients[0]);
            }

            return amountOfOre;

            void checkForExcess(Ingredient ingredient)
            {
                var excessIngredient = ExcessIngredients.FirstOrDefault(x => x.Element == ingredient.Element);
                if (excessIngredient == null) return;
                if (excessIngredient.Amount > ingredient.Amount)
                {
                    excessIngredient.Amount -= ingredient.Amount;
                    ingredient.Amount = 0;
                }
                else
                {
                    ingredient.Amount -= excessIngredient.Amount;
                    excessIngredient.Amount = 0;
                }
            }

            void AddExcess(Ingredient ingredient, long amount)
            {
                var excessIngredient = ExcessIngredients.FirstOrDefault(x => x.Element == ingredient.Element);
                if (excessIngredient == null)
                {
                    ExcessIngredients.Add(new Ingredient()
                    {
                        Element = ingredient.Element,
                        Amount = amount
                    });
                }
                else
                {
                    excessIngredient.Amount += amount;
                }
            }

            void AddNeeded(Ingredient ingredient)
            {
                if (ingredient.Element == "ORE")
                {
                    amountOfOre += ingredient.Amount;
                    return;
                }
                var neededIngredient = neededIngredients.FirstOrDefault(x => x.Element == ingredient.Element);
                if (neededIngredient == null)
                {
                    neededIngredients.Add(new Ingredient()
                    {
                        Element = ingredient.Element,
                        Amount = ingredient.Amount
                    });
                }
                else
                {
                    neededIngredient.Amount += ingredient.Amount;
                }
            }
        }

        public ResultFromGettingIngrediens GetIngredientsForElement(Ingredient ingredient)
        {
            var recipe = Recipes.First(x => x.EndProduct.Element == ingredient.Element);
            var ActualElementsCreated = GetLeastCommonMultiplier(recipe.EndProduct.Amount, ingredient.Amount);
            var excessProducts = ActualElementsCreated - ingredient.Amount;
            recipe.inputIngredients.ForEach(x => x.Amount *= (ActualElementsCreated/ recipe.EndProduct.Amount));
            var neededIngredients = recipe.inputIngredients;
            return new ResultFromGettingIngrediens()
            {
                ExcessIngredient = excessProducts,
                NeededIngredients = neededIngredients
            };
        }

        public long GetLeastCommonMultiplier(long a, long b)
        {
            //var temp = a;
            //while (b > temp)
            //{
            //    temp += a;
            //}

            //return temp;


            return ((long)Math.Ceiling((float)b / (float)a)*a);

        }
        public long GetStepsToOre(Ingredient ingredient, List<Recipe> recipes)
        {
            var recipe = recipes.First(x => x.EndProduct.Element == ingredient.Element);
            if (recipe.inputIngredients.FirstOrDefault(x => x.Element == "ORE") != null) return 0;
            var outputs = recipe.inputIngredients.Select(x => GetStepsToOre(x, recipes));
            return 1 + outputs.Max();
        }
    }

    public class Recipe
    {
        public List<Ingredient> inputIngredients { get; set; }
        public Ingredient EndProduct { get; set; }

        public static Recipe Create(string inputRecipe)
        {
            var temp = inputRecipe.Split("=>");
            var leftSide = temp[0];
            var rightSide = temp[1];

            var leftsideSplit = leftSide.Split(',');
            var leftsideIngredients = leftsideSplit.Select(Ingredient.Create);
            var endProduct = Ingredient.Create(rightSide);

            var recipe =  new Recipe();
            recipe.inputIngredients = leftsideIngredients.ToList();
            recipe.EndProduct = endProduct;

            return recipe;
        }

        public Recipe Clone()
        {
            var newRecipe = new Recipe();
            newRecipe.inputIngredients = this.inputIngredients.Select(x => x.Clone()).ToList();
            newRecipe.EndProduct = this.EndProduct.Clone();
            return newRecipe;
        }

    }

    public class Ingredient
    {
        public long Amount { get; set; }
        public string Element { get; set; }

        public static Ingredient Create(string input)
        {
            var ingredient = new Ingredient();
            var SplittedInput = input.Trim().Split(" ");
            ingredient.Amount = long.Parse(SplittedInput[0]);
            ingredient.Element = SplittedInput[1];
            return ingredient;
        }

        public Ingredient Clone()
        {
            var newIngredient = new Ingredient();
            newIngredient.Amount = this.Amount;
            newIngredient.Element = this.Element;
            return newIngredient;
        }

        
    }

    public class ResultFromGettingIngrediens
    {
        public long ExcessIngredient { get; set; }
        public List<Ingredient> NeededIngredients { get; set; }
    }

   


}
