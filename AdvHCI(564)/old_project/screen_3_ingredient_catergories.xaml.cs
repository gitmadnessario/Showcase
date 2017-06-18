using AmlProject;
using Fizbin.Kinect.Gestures;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class screen_3_ingredient_catergories : UserControl, ISwitchable
    {
        Environment env;
        private BarcodeReader barcodeReader;
        List<Recipe> currentRecipes = new List<Recipe>();
        public screen_3_ingredient_catergories(Environment env)
        {
            InitializeComponent();

            this.env = env;
            string users = "";
            if (env.user1.isActive())
            {
                users = users + env.user1.getUserName() + " ";
            }
            if (env.user2.isActive())
            {
                users = users + env.user2.getUserName();
            }
            Dispatcher.BeginInvoke((Action)(() => { top.Text = users; }));



            if (env.using_Sensors == true)
            {
                //   intf[0].SensorChange += MainWindow_SensorChange;
                //    intf[1].SensorChange += MainWindow_SensorChange2;
            }

            if (env.using_Kinect == true)
            {
                var regionSensorBinding = new Binding("Kinect")
                {
                    Source =
                        HY564KinectManager.Instance().GetKinectSensorChooser()
                };
                BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady += NewSensor_ColorFrameReady;
                barcodeReader = new BarcodeReader();
                barcodeReader.Options.PossibleFormats = new List<BarcodeFormat> { ZXing.BarcodeFormat.QR_CODE };
            }


            for (int i = 0; i < env.recipebook.amountOfRecipes(); i++)
            {
                currentRecipes.Add(env.recipebook.getRecipe(i));
            }

            if (env.recipeMode == 2)
            {
                currentRecipes = currentRecipes.OrderBy(o => o.cookingTime).ToList();
            }

            refreshData();




        }



        public void refreshData()
        {
            /*rec - > cat (e.g. rec1,rec2 -> cat1,cat2 ) br is for border (every kinect button is inside a boreder
            so there is that little space actually in which kinectbutton enlarges )
            
            if (currentRecipes.Count > 0)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec1.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(0).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec1.Label = currentRecipes.ElementAt(0).getName(); }));
            }
            else { br1.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 1)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec2.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(1).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec2.Label = currentRecipes.ElementAt(1).getName(); }));
            }
            else { br2.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 2)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec3.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(2).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec3.Label = currentRecipes.ElementAt(2).getName(); }));
            }
            else { br3.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 3)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec4.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(3).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec4.Label = currentRecipes.ElementAt(3).getName(); }));
            }
            else { br4.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 4)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec5.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(4).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec5.Label = currentRecipes.ElementAt(4).getName(); }));
            }
            else { br5.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 5)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec6.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(5).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec6.Label = currentRecipes.ElementAt(5).getName(); }));
            }
            else { br6.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 6)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec7.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(6).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec7.Label = currentRecipes.ElementAt(6).getName(); }));
            }
            else { br7.Visibility = Visibility.Hidden; }
            if (currentRecipes.Count > 7)
            {
                Dispatcher.BeginInvoke((Action)(() => { rec8.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), currentRecipes.ElementAt(7).getPicturePath()))); }));
                Dispatcher.BeginInvoke((Action)(() => { rec8.Label = currentRecipes.ElementAt(7).getName(); }));
            }
            else { br8.Visibility = Visibility.Hidden; } */
        }


        void NewSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    //UpdateKinectFrame(colorFrame);
                    var qr_Result = barcodeReader.Decode(colorFrame.GetRawPixelData(), colorFrame.Width, colorFrame.Height,
                                            ZXing.RGBLuminanceSource.BitmapFormat.RGB32);

                    if (qr_Result != null)
                    {
                        // this.ScreenLabel.Content = qr_Result.Text;

                        string[] words = qr_Result.Text.Split(' ');

                        if (words[0] == "Product:")
                        {
                            //   productsScanned.Enqueue(new Product(words[1], words[2]));
                        }
                    }
                }
            }
        }


        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            if (env.using_Sensors == true)
            {
                // env.intf[0].SensorChange -= Fridge_SensorChange;
            }

            if (env.using_Kinect == true)
            {
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady -= NewSensor_ColorFrameReady;
                BindingOperations.ClearBinding(this.kinectRegion, KinectRegion.KinectSensorProperty);
            }
        }

        void MainWindow_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {

            /*       if (e.Index == 0 && e.Value < 500)
                   {

                       if (productsScanned.Count > 0 && env.fridge.checkSlot(e.Index).getProductName() == "empty")
                       {
                           env.fridge.addProductToFridge(productsScanned.Dequeue(), e.Index);

                       }
                   }

                   if (e.Index == 1 && e.Value < 500)
                   {
                       if (productsScanned.Count > 0 && env.fridge.checkSlot(e.Index).getProductName() == "empty")
                       {
                           env.fridge.addProductToFridge(productsScanned.Dequeue(), e.Index);

                       }
                   }

                   //    switch (e.Index)
                   //  {
                   //    case 0: Dispatcher.BeginInvoke((Action)(() => { touchValue.Content = e.Value; }));
                   //      break;

                   //            }
                   */
        }


        private void KinectPressButton(object sender, RoutedEventArgs e)
        {
            String name = ((Microsoft.Kinect.Toolkit.Controls.KinectTileButton)e.OriginalSource).Name;
            switch (name)
            {
                case "home":
                    ViewSwitcher.Switch(new screen_1_home_logged_in(env));
                    break;
                case "recipes":
                    ViewSwitcher.Switch(new screen_2_recipes(env));
                    break;
                case "shopping":
                    ViewSwitcher.Switch(new screen_3_shopping_List(env));
                    break;
                case "schedule":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "fridge":
                    ViewSwitcher.Switch(new screen_5_fridge(env));
                    break;
                case "alarms":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "preferences":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "logout":
                    env.user1.setActive(false);
                    env.user2.setActive(false);
                    ViewSwitcher.Switch(new login(env));
                    break;

                //recipe stuff i leave it intact so you can make equivalent for ingredients
                //next page is " screen_3_store_ingredients "

                case "rec1":
                    env.cookingRecipe = currentRecipes.ElementAt(0);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;

                case "rec2":
                    env.cookingRecipe = currentRecipes.ElementAt(1);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec3":
                    env.cookingRecipe = currentRecipes.ElementAt(2);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec4":
                    env.cookingRecipe = currentRecipes.ElementAt(3);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec5":
                    env.cookingRecipe = currentRecipes.ElementAt(4);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec6":
                    env.cookingRecipe = currentRecipes.ElementAt(5);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec7":
                    env.cookingRecipe = currentRecipes.ElementAt(6);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
                case "rec8":
                    env.cookingRecipe = currentRecipes.ElementAt(7);
                    Console.WriteLine("RECIPE PRESSED = " + env.cookingRecipe.getName());
                    ViewSwitcher.Switch(new screen_2_recipes_single(env));
                    break;
            }

        }





    }
}
