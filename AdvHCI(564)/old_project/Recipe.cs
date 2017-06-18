using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Recipe
    {
        String name;
        String picturePath;
        public int cookingTime;
        Dictionary<String, String> ingredients;
        List<String> steps;
        List<String> assistance;

        public Recipe(String name, String picturePath, int cookingTime, Dictionary<String, String> ingredients, List<String> steps, List<String> assistance)
        {
            this.name = name;
            this.picturePath = picturePath;
            this.cookingTime = cookingTime;
            this.ingredients = ingredients;
            this.steps = steps;
            this.assistance = assistance;
        }

        public String getName()
        {
            return name;
        }

        public String getPicturePath()
        {
            return picturePath;
        }

        public int getCookingTime()
        {
            return cookingTime;
        }

        public Dictionary<String, String> getIngredients()
        {
            return ingredients;
        }

        public List<String> getSteps()
        {
            return steps;
        }

        public List<String> getAssistance()
        {
            return assistance;
        }

    }
}
