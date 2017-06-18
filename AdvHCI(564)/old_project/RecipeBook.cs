using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class RecipeBook
    {
        public List<Recipe> recipes = new List<Recipe>();

        public int amountOfRecipes()
        {
            return recipes.Count;
        }

        public Recipe getRecipe(int number)
        {
            return recipes.ElementAt(number);
        }

        public RecipeBook()
        {
            String name;
            String picturePath;
            int cookingTime;
            Dictionary<String, String> ingredients;
            List<String> steps;
            List<String> assistance;
            Recipe recipe;



            name = "Sunny side-up eggs";
            picturePath = "Resources\\Recipes\\sunnysideupeggs.png";
            cookingTime = 5;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Olive oil", "1 tablespoon olive oil");
            ingredients.Add("Egg", "4 large free-range eggs");
            ingredients.Add("Salt", "1/16 teaspoon sea salt");
            ingredients.Add("Black pepper", "Freshly ground black pepper, to taste");

            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Get your frying pan on a medium to low heat and add enough olive oil to lightly coat the bottom of a large nonstick pan (about 1 tablespoon).");
            assistance.Add("");
            steps.Add("Crack the eggs into the pan. As the oil gets hotter you'll see it start to change the color of the eggs. If the oil starts to spit it's because it's too hot, so turn the heat right down. Cook until the tops of the whites are set but the yolk is still runny.");
            assistance.Add("http://www.youtube.com/watch?v=KaiBhWDKTNw");
            steps.Add("When they're ready, remove the pan from the heat and take the eggs out using a spatula. Place on a plate and dab them with some paper towels to soak up any excess oil. Serve on toast – no need to butter it – with a sprinkling of the sea salt and freshly ground black pepper.");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);




            name = "Rosemary roasted Jersey Royals";
            // http://www.jamieoliver.com/recipes/vegetables-recipes/rosemary-roasted-jersey-royals/
            picturePath = "Resources\\Recipes\\rosemary-royals.png";
            cookingTime = 60;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Potato", "1.2 kg Jersey Royal potatoes");
            ingredients.Add("Garlic", "1 bulb of garlic, unpeeled");
            ingredients.Add("Rosemary", "A few sprigs of fresh rosemary");
            ingredients.Add("Salt", "Sea salt");
            ingredients.Add("Black pepper", "Freshly ground black pepper");
            ingredients.Add("Olive oil", "Olive oil");

            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Preheat the oven to 190ºC.");
            assistance.Add("");
            steps.Add("Add the potatoes to a medium pan of boiling salted water, reduce the heat to medium and simmer for 10 to 15 minutes, or until cooked through.");
            assistance.Add("");
            steps.Add("Drain and leave to steam dry, then place in medium roasting tray and mash lightly with a potato masher.");
            assistance.Add("");
            steps.Add("Throw in the garlic and rosemary, season with salt and pepper and drizzle with a good lug of olive oil.");
            assistance.Add("");
            steps.Add("Place in the hot oven for around 40 minutes, or until crispy and golden.");
            assistance.Add("");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);

            

            name = "Summer pea & watercress soup";
            // http://www.jamieoliver.com/recipes/vegetables-recipes/summer-pea-and-watercress-soup/
            picturePath = "Resources\\Recipes\\pea-soup.png";
            cookingTime = 35;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Olive oil", "Olive oil");
            ingredients.Add("Onion", "1 onion, peeled and chopped");
            ingredients.Add("Celery", "2 sticks celery, trimmed and chopped");
            ingredients.Add("Potato", "1 medium potato, peeled and cubed");
            ingredients.Add("Salt", "Sea Salt");
            ingredients.Add("Black pepper", "Freshly ground black pepper");
            ingredients.Add("Chicken stock", "2 litres organic chicken or vegetable stock");
            ingredients.Add("Peas", "500 g fresh peas, podded");
            ingredients.Add("Watercress", "200 g watercress, washed and spun dry");
            ingredients.Add("Sour cream", "142 ml soured cream");

            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Heat a large saucepan and pour in a little olive oil. Throw in the chopped onion and celery. Turn the heat down and cook very gently with the lid on for 10 minutes, or until the onion has softened, but not browned.");
            assistance.Add("");
            steps.Add("Add the potato and stock and bring to the boil. Simmer for 10 minutes or until the potato is cooked.");
            assistance.Add("");
            steps.Add("Drop in the peas and watercress and simmer for a further 5 minutes until the peas are cooked.");
            assistance.Add("");
            steps.Add("Remove the pan from the heat.");
            assistance.Add("");
            steps.Add("Whiz the soup with a hand blender, or in a liquidizer, until smooth. Taste, then season with salt and pepper.");
            assistance.Add("");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);



            name = "Roasted baby leeks with thyme";
            // http://www.jamieoliver.com/recipes/vegetables-recipes/roasted-baby-leeks-with-thyme/
            picturePath = "Resources\\Recipes\\roasted-leek.png";
            cookingTime = 20;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Leek", "20 baby leeks");
            ingredients.Add("Olive oil", "Olive oil");
            ingredients.Add("Vinegar", "Red wine vinegar");
            ingredients.Add("Thyme", "1 teaspoon chopped fresh thyme leaves");
            ingredients.Add("Garlic", "2 cloves garlic, peeled and sliced");


            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Preheat your oven to 200°C.");
            assistance.Add("");
            steps.Add("Lightly trim both ends of the baby leeks and peel back the first or second layer of leaves and discard. ");
            assistance.Add("");
            steps.Add("Drop the leeks in a pan of boiling salted water for 2 to 3 minutes to soften - this is called blanching.");
            assistance.Add("");
            steps.Add("Drain them well (if there's too much water in them they won't roast properly) and toss in a bowl with a good lug of olive oil, a splash of red wine vinegar, the chopped thyme leaves and the garlic.");
            assistance.Add("");
            steps.Add("Arrange the leeks in one layer in a baking tray or earthenware dish and roast in the preheated oven for about 10 minutes until golden and almost caramelized.");
            assistance.Add("");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);




            name = "Perfect scrambled eggs";
            // http://www.bbcgoodfood.com/recipes/1720/perfect-scrambled-eggs
            picturePath = "Resources\\Recipes\\scrambled-eggs.png";
            cookingTime = 15;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Egg", "2 large free range eggs");
            ingredients.Add("Milk", "6 tbsp milk");
            ingredients.Add("Butter", "A knob of butter");


            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Crack the eggs open");
            assistance.Add("http://www.youtube.com/watch?v=KaiBhWDKTNw");
            steps.Add("Lightly whisk the eggs, cream and a pinch of salt together until all the ingredients are just combined and the mixture has one consistency.");
            assistance.Add("");
            steps.Add("Heat a small non-stick frying pan for a minute or so, then add the butter and let it melt. Don’t allow the butter to brown or it will discolour the eggs. Pour in the egg mixture and let it sit, without stirring, for 20 seconds. Stir with a wooden spoon, lifting and folding it over from the bottom of the pan. Let it sit for another 10 seconds then stir and fold again.");
            assistance.Add("");
            steps.Add("Repeat until the eggs are softly set and slightly runny in places, then remove from the heat and leave for a few seconds to finish cooking. Give a final stir and serve the velvety scramble without delay.");
            assistance.Add("");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);




            name = "Fresh pineapple with creme fraiche & mint";
            // http://www.jamieoliver.com/recipes/fruit-recipes/fresh-pineapple-with-cr-me-fra-che-and-mint/
            picturePath = "Resources\\Recipes\\fresh-pineapple.png";
            cookingTime = 10;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Creme fraiche", "1 small tub creme fraiche");
            ingredients.Add("Vanilla", "1 vanilla pod, scored lengthways and seeds scraped out");
            ingredients.Add("Pineapple", "1 pineapple");
            ingredients.Add("Sugar", "icing sugar, for dusting ");
            ingredients.Add("Mint", "1 handful fresh mint, leaves picked and finely sliced");


            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Preheat your barbecue.");
            assistance.Add("");
            steps.Add("Beat the creme fraiche with the seeds from the vanilla pod and put to one side.");
            assistance.Add("");
            steps.Add("Cut the ends off the pineapple. Sit it on a board and cut off the skin in wide strips from top to bottom, making sure you cut out any woody eyes.");
            assistance.Add("");
            steps.Add("Thinly slice the pineapple, and dust the slices with icing sugar.");
            assistance.Add("");
            steps.Add("Drop the slices of pineapple on the BBQ. Grill for about 30 seconds on each side then lift off with the tongs and transfer to a serving plate.");
            assistance.Add("");
            steps.Add("Serve scattered with mint and a generous dollop of vanilla-flavoured creme fraiche.");
            assistance.Add("");

            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);




            name = "Perfect roast chicken";
            // http://www.jamieoliver.com/recipes/chicken-recipes/perfect-roast-chicken/
            picturePath = "Resources\\Recipes\\roasted-chicken.png";
            cookingTime = 85;

            ingredients = new Dictionary<String, String>();
            ingredients.Add("Chicken", "1.6 kg higher-welfare chicken");
            ingredients.Add("Onion", "2 medium onions");
            ingredients.Add("Carrot", "2 carrots");
            ingredients.Add("Celery", "2 sticks celery");
            ingredients.Add("Garlic", "1 bulb garlic");
            ingredients.Add("Salt", "Sea salt");
            ingredients.Add("Black pepper", "Freshly ground black pepper");
            ingredients.Add("Olive oil", "Olive oil");
            ingredients.Add("Lemon", "1 lemon");
            ingredients.Add("Thyme", "1 small bunch fresh thyme");


            steps = new List<string>();
            assistance = new List<String>();
            steps.Add("Take your chicken out of the fridge 30 minutes before it goes into the oven.");
            assistance.Add("");
            steps.Add("Preheat your oven to 240°C.");
            assistance.Add("");
            steps.Add("There's no need to peel the vegetables – just give them a wash and roughly chop them.");
            assistance.Add("");
            steps.Add("Break the garlic bulb into cloves, leaving them unpeeled.");
            assistance.Add("");
            steps.Add("Pile all the veg and garlic into the middle of a large roasting tray and drizzle with olive oil.");
            assistance.Add("");
            steps.Add("Drizzle the chicken with olive oil and season well with salt and pepper, rubbing it all over the bird.");
            assistance.Add("");
            steps.Add("Carefully prick the lemon all over, using the tip of a sharp knife (if you have a microwave, you could pop the lemon in these for 40 seconds at this point as this will really bring out the flavour).");
            assistance.Add("");
            steps.Add("Put the lemon inside the chicken's cavity, with the bunch of herbs.");
            assistance.Add("");
            steps.Add("Place the chicken on top of the vegetables in the roasting tray and put it into the preheated oven.");
            assistance.Add("");
            steps.Add("Turn the heat down immediately to 200°C/400°F/gas 6 and cook the chicken for 1 hour and 20 minutes.");
            assistance.Add("");
            steps.Add("Baste the chicken halfway through cooking and if the veg look dry, add a splash of water to the tray to stop them burning.");
            assistance.Add("");
            steps.Add("When cooked, take the tray out of the oven and transfer the chicken to a board to rest for 15 minutes or so. Cover it with a layer of tinfoil and a tea towel and put aside.");
            assistance.Add("");


            recipe = new Recipe(name, picturePath, cookingTime, ingredients, steps, assistance);
            recipes.Add(recipe);


        }
    }
}
