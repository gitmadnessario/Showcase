using AmlProject;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public class Environment
    {
        public Person user1;
        public Person user2;
        public Fridge fridge;
        public Stack<Product> removedItems;
        public Queue<Product> scannedProducts;
        public List<Product> expired;
        public RecipeBook recipebook;
        public Recipe cookingRecipe;
        public int recipeStep;
        public bool using_Kinect;
        public bool using_sonars;
        public bool using_Sensors;
        public AllProducts allProducts;
        Phidgets.Manager PM;
        public Phidgets.InterfaceKit[] intf;
        public int recipeMode = 0;
        public SpeechSynthesizer speech;
        public bool skipIngredients = true;
        //
        public List<KinectTileButton> menulist;
       // public List<String> borders;
       
        public KinectTileButton focused;
        public string current;//
        public Agenda agenda;
        //

        public Stack<Dictionary<int, Product>> shoppingLists;
        public Environment()
        {
            speech = new SpeechSynthesizer();
            speech.Rate = -3;
            //phigets code
            PM = new Phidgets.Manager();
            PM.open();
            Thread.Sleep(1000);
            intf = new Phidgets.InterfaceKit[2];
            int n = 0;
            foreach (var d in PM.Devices)
            {
                Phidgets.InterfaceKit pik = d as Phidgets.InterfaceKit;
                intf[n] = new Phidgets.InterfaceKit();
                intf[n].open(pik.SerialNumber);
                Console.WriteLine(pik.ID);
                n++;
            }
        }

        public void close()
        {
            int n = 0;
            foreach (var d in PM.Devices)
            {
                Phidgets.InterfaceKit pik = d as Phidgets.InterfaceKit;
                intf[n].close();
                n++;
            }
        }
    }
}
