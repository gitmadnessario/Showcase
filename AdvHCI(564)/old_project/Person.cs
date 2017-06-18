using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmlProject
{
    public class Person
    {
        String name;
        String picturePath;
        HashSet<String> allergies;
        HashSet<String> dietaryPreferences;
        HashSet<String> favoriteRecipes;
        HashSet<String> hatedRecipes;
        Boolean active;
        public Person(String name)
        {
            this.name = name;
            this.active = false;
            this.allergies = new HashSet<string>();
            this.favoriteRecipes = new HashSet<string>();
            this.dietaryPreferences = new HashSet<string>();
           
        }

        public Boolean isActive()
        {
            return active;
        }
        public String getUserName()
        {
            return name;
        }

        public String getPicturePath()
        {
            return picturePath;
        }

        public HashSet<String> getAllergies()
        {
            return allergies;
        }

        public HashSet<String> getDietaryPreferences()
        {
            return dietaryPreferences;
        }

        public HashSet<String> getFavoriteRecipes()
        {
            return favoriteRecipes;
        }

        public HashSet<String> getHatedRecipes()
        {
            return hatedRecipes;
        }

        public void addAllergie(String allergy)
        {
            allergies.Add(allergy);
        }

        public void addDietaryPreference(String dietaryPreference)
        {
            dietaryPreferences.Add(dietaryPreference);
        }

        public void addFavoriteRecipe(String recipe)
        {
            favoriteRecipes.Add(recipe);
        }

        public void removeFavoriteRecipe(String recipe)
        {
            if (favoriteRecipes.Contains(recipe))
            {
                favoriteRecipes.Remove(recipe);
            }
        }

        public void resetFavoriteRecipes()
        {
            favoriteRecipes = new HashSet<string>();
        }

        public void addHatedRecipe(String recipe)
        {
            hatedRecipes.Add(recipe);
        }

        public void removeHatedRecipe(String recipe)
        {
            if (hatedRecipes.Contains(recipe))
            {
                hatedRecipes.Remove(recipe);
            }
        }

        public void resetHatedRecipes()
        {
            hatedRecipes = new HashSet<string>();
        }

        public void setPicturePath(String picturePath)
        {
            this.picturePath=picturePath;
        }

        public void setActive(Boolean isactive)
        {
            active = isactive;
        }
    }
}
