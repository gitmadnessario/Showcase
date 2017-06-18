using AmlProject;
using Fizbin.Kinect.Gestures;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class screen_3_store_ingredients : UserControl, ISwitchable
    {
        Environment env;
        Phidgets.Manager PM;
        Phidgets.InterfaceKit[] intf;
        private BarcodeReader barcodeReader;
        Queue<Product> productsScanned;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        int extramenuitem = 0;
                List<Border> borders = new List<Border>();
        

        //
        Dictionary<int, Product> shoppingList;
        ImageBrush greenBrush;
        public screen_3_store_ingredients(Environment env)
        {
            InitializeComponent();
            addborders();
            this.env = env;
            env.menulist.Clear();
            env.menulist.Add(home);
            env.menulist.Add(recipes);
            env.menulist.Add(shopping);
            env.menulist.Add(schedule);
            env.menulist.Add(fridgeMenu);
            env.menulist.Add(alarms);
            env.menulist.Add(preferences);
            env.menulist.Add(logout);
            env.menulist.Add(ing1); extramenuitem++;
            env.menulist.Add(ing2); extramenuitem++;
            env.menulist.Add(ing3); extramenuitem++;
            env.menulist.Add(ing4); extramenuitem++;
            env.menulist.Add(ing5); extramenuitem++;
            env.menulist.Add(ing6); extramenuitem++;
            env.menulist.Add(ing7); extramenuitem++;
            env.menulist.Add(ing8); extramenuitem++;
            env.menulist.Add(ing9); extramenuitem++;
            env.menulist.Add(ing10); extramenuitem++;
            env.menulist.Add(ing11); extramenuitem++;
            env.menulist.Add(ing12); extramenuitem++;
            env.menulist.Add(ing13); extramenuitem++;
            env.menulist.Add(ing14); extramenuitem++;
            env.menulist.Add(ing15); extramenuitem++;
            env.menulist.Add(ing16); extramenuitem++;
            //24


            //
            //
            myTimer = new System.Timers.Timer(50);
            myTimer.Enabled = false;
            myTimer.Elapsed += OnTimedEvent;
            env.focused = env.menulist[8];
            //env.focused = recipes;
            /*string newString = string.Concat(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('.'), 0)), "_hover.png");
            Console.WriteLine(newString);
            Dispatcher.BeginInvoke((Action)(() => { recipes.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));*/
            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(env.menulist[8]);
                env.focused = env.menulist[8];
                Dispatcher.BeginInvoke((Action)(() => { env.focused.Focus(); focus_next(env.focused); })); ;
                if (myTimer == null)
                {
                    myTimer = new System.Timers.Timer(50);
                    myTimer.Enabled = false;
                    myTimer.Elapsed += OnTimedEvent;
                }
            };
            string users = "";
            if (env.user1.isActive())
            {
                users = env.user1.getUserName();
            }
            if (env.user2.isActive())
            {
                if (env.user1.isActive())
                {
                    users = users + " and " + env.user2.getUserName();
                }
                else
                {
                    users = env.user2.getUserName();
                }
            }
            Dispatcher.BeginInvoke((Action)(() => { top.Text = users; }));

            //populate the screen with the products available

            List<Product> products = new List<Product>();
            products = env.allProducts.getProducts();
            Dispatcher.BeginInvoke((Action)(() =>
            {
                ing1.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[0].getPicturePath())));
                ing2.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[1].getPicturePath())));
                ing3.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[2].getPicturePath())));
                ing4.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[3].getPicturePath())));
                ing5.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[4].getPicturePath())));
                ing6.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[5].getPicturePath())));
                ing7.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[6].getPicturePath())));
                ing8.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[7].getPicturePath())));
                ing9.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[8].getPicturePath())));
                ing10.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[9].getPicturePath())));
                ing11.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[10].getPicturePath())));
                ing12.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[11].getPicturePath())));
                ing13.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[12].getPicturePath())));
                ing14.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[13].getPicturePath())));
                ing15.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[14].getPicturePath())));
                ing16.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), products[15].getPicturePath())));
            }));

            //initiallize greenBrush
            greenBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources\\colors\\darkgreen.png")));


            //create an empty shopping list to add products   
            shoppingList = new Dictionary<int, Product>();

            productsScanned = new Queue<Product>();

            if (env.using_Sensors == true)
            {
                if (env.using_sonars == true)
                {
                    env.intf[0].SensorChange += SensorChange;
                    //intf[1].SensorChange += MainWindow_SensorChange2;
                }
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
                        Console.WriteLine("DETECTED QR = " + qr_Result.Text);
                        // this.ScreenLabel.Content = qr_Result.Text;
                        string[] words = qr_Result.Text.Split(' ');

                        if (words[0] == "User:")
                        {
                            if (words[1] == env.user1.getUserName())
                            {
                                if (!env.user1.isActive())
                                {
                                    env.speech.SpeakAsync("Welcome " + env.user1.getUserName());
                                    env.user1.setActive(true);
                                }

                            }
                            if (words[1] == env.user2.getUserName())
                            {
                                if (!env.user2.isActive())
                                {
                                    env.speech.SpeakAsync("Welcome " + env.user2.getUserName());
                                    env.user2.setActive(true);
                                }
                            }
                            string users = "";
                            if (env.user1.isActive())
                            {
                                users = env.user1.getUserName();
                            }
                            if (env.user2.isActive())
                            {
                                if (env.user1.isActive())
                                {
                                    users = users + " and " + env.user2.getUserName();
                                }
                                else
                                {
                                    users = env.user2.getUserName();
                                }
                            }
                            Dispatcher.BeginInvoke((Action)(() => { top.Text = users; }));
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
                env.intf[0].SensorChange -= SensorChange;
                myTimer.Elapsed -= OnTimedEvent;
            }

            if (env.using_Kinect == true)
            {
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady -= NewSensor_ColorFrameReady;
                BindingOperations.ClearBinding(this.kinectRegion, KinectRegion.KinectSensorProperty);
               // HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized -= refresh_GestureRecognized;

            }
        }

        void MainWindow_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {

            if (e.Index == 0 && e.Value < 500)
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

        }


        private void KinectPressButton(object sender, RoutedEventArgs e)
        {
            String name = ((Microsoft.Kinect.Toolkit.Controls.KinectTileButton)e.OriginalSource).Name;
            int b = 0;
            if (name.Contains("ing") && name.Length < 6)
            {
                b = Convert.ToInt32(name.Substring(3));
                name = "ing";
            }
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
                    ViewSwitcher.Switch(new screen_4_calendar(env));
                    break;
                case "fridgeMenu":
                    ViewSwitcher.Switch(new screen_5_fridge(env));
                    break;
                case "alarms":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "preferences":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "logout":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "saveList":
                    env.shoppingLists.Push(shoppingList);
                    Debug.WriteLine("EDWEDW"+env.shoppingLists.ElementAt(0).ElementAt(0).ToString());
                    ViewSwitcher.Switch(new screen_3_shopping_List(env));
                    break;
                case "ing":
                    switch (b)
                    {
                        case 1:
                            if (bor1.Background == Brushes.Yellow)
                            {
                                bor1.Background = greenBrush;
                                bor1.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor1.Background = Brushes.Yellow;
                                bor1.Padding = new Thickness(8);
                            }

                            break;
                        case 2:
                            if (bor2.Background == Brushes.Yellow)
                            {
                                bor2.Background = greenBrush;
                                bor2.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor2.Background = Brushes.Yellow;
                                bor2.Padding = new Thickness(8);
                            }

                            break;
                        case 3:
                            if (bor3.Background == Brushes.Yellow)
                            {
                                bor3.Background = greenBrush;
                                bor3.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor3.Background = Brushes.Yellow;
                                bor3.Padding = new Thickness(8);
                            }

                            break;
                        case 4:
                            if (bor4.Background == Brushes.Yellow)
                            {
                                bor4.Background = greenBrush;
                                bor4.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor4.Background = Brushes.Yellow;
                                bor4.Padding = new Thickness(8);
                            }

                            break;
                        case 5:
                            if (bor5.Background == Brushes.Yellow)
                            {
                                bor5.Background = greenBrush;
                                bor5.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor5.Background = Brushes.Yellow;
                                bor5.Padding = new Thickness(8);
                            }

                            break;
                        case 6:
                            if (bor6.Background == Brushes.Yellow)
                            {
                                bor6.Background = greenBrush;
                                bor6.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor6.Background = Brushes.Yellow;
                                bor6.Padding = new Thickness(8);
                            }

                            break;
                        case 7:
                            if (bor7.Background == Brushes.Yellow)
                            {
                                bor7.Background = greenBrush;
                                bor7.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor7.Background = Brushes.Yellow;
                                bor7.Padding = new Thickness(8);
                            }

                            break;
                        case 8:
                            if (bor8.Background == Brushes.Yellow)
                            {
                                bor8.Background = greenBrush;
                                bor8.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor8.Background = Brushes.Yellow;
                                bor8.Padding = new Thickness(8);
                            }

                            break;
                        case 9:
                            if (bor9.Background == Brushes.Yellow)
                            {
                                bor9.Background = greenBrush;
                                bor9.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor9.Background = Brushes.Yellow;
                                bor9.Padding = new Thickness(8);
                            }

                            break;
                        case 10:
                            if (bor10.Background == Brushes.Yellow)
                            {
                                bor10.Background = greenBrush;
                                bor10.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor10.Background = Brushes.Yellow;
                                bor10.Padding = new Thickness(8);
                            }

                            break;
                        case 11:
                            if (bor11.Background == Brushes.Yellow)
                            {
                                bor11.Background = greenBrush;
                                bor11.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor11.Background = Brushes.Yellow;
                                bor11.Padding = new Thickness(8);
                            }

                            break;
                        case 12:
                            if (bor12.Background == Brushes.Yellow)
                            {
                                bor12.Background = greenBrush;
                                bor12.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor12.Background = Brushes.Yellow;
                                bor12.Padding = new Thickness(8);
                            }

                            break;
                        case 13:
                            if (bor13.Background == Brushes.Yellow)
                            {
                                bor13.Background = greenBrush;
                                bor13.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor13.Background = Brushes.Yellow;
                                bor13.Padding = new Thickness(8);
                            }

                            break;
                        case 14:
                            if (bor14.Background == Brushes.Yellow)
                            {
                                bor14.Background = greenBrush;
                                bor14.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor14.Background = Brushes.Yellow;
                                bor14.Padding = new Thickness(8);
                            }

                            break;
                        case 15:
                            if (bor15.Background == Brushes.Yellow)
                            {
                                bor15.Background = greenBrush;
                                bor15.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor15.Background = Brushes.Yellow;
                                bor15.Padding = new Thickness(8);
                            }

                            break;
                        case 16:
                            if (bor16.Background == Brushes.Yellow)
                            {
                                bor16.Background = greenBrush;
                                bor16.Padding = new Thickness(2);
                                shoppingList.Remove(b);
                            }
                            else
                            {
                                shoppingList.Add(b, env.allProducts.getProducts()[b - 1]);
                                bor16.Background = Brushes.Yellow;
                                bor16.Padding = new Thickness(8);
                            }

                            break;
                    }

                    break;

            }

        }



        int tickCounter = 0;
        Boolean left_sensor = false;
        Boolean right_sensor = false;
        Boolean background_change = false;
        int select = 0;
        void OnTimedEvent(Object myObject, EventArgs myEventArgs)
        {
            System.Diagnostics.Debug.WriteLine("this bla tick" + tickCounter);
            tickCounter++;
            if ((tickCounter > 11) && ((tickCounter % 12) == 0))
            {
                if (right_sensor && left_sensor)
                {
                    System.Diagnostics.Debug.WriteLine("before" + select);

                    select++;
                    System.Diagnostics.Debug.WriteLine("after" + select);

                    if (select > 0)
                    {
                        if ((env.focused == shopping))
                        {

                            background_change = true;

                            Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; focus_previous(shopping); env.focused.Focus(); focus_next(env.focused); })); ;
                            return;//focus_previous(env.menulist[i]);env.focused.Focus(); focus_next(env.focused); 
                            //if flag focusprev(env focused) env focused = tade focus_next(tade) env.focused.focus();
                        }
                        tickCounter = 0;
                        myTimer.Enabled = false;
                        right_sensor = left_sensor = false;
                        //env.test = false;
                        // myTimer = null;
                        this.Dispatcher.Invoke((Action)(() =>
                        { env.focused.RaiseEvent(new RoutedEventArgs(KinectTileButton.ClickEvent)); }));


                        // .RaiseEvent(new RoutedEventArgs(Keyboard.Kt));
                        // env.focused.RaiseEvent(new RoutedEventArgs(KinectTileButton.ClickEvent));
                    }
                }
                if (right_sensor && !(left_sensor)) { select = 0; System.Diagnostics.Debug.WriteLine("next"); next(); }
                if (left_sensor && !(right_sensor)) { previous(); select = 0; }

            }

        }

        public void previous()
        {
            Debug.WriteLine("PAME PISW PASOK");
            for (int i = 0; i < env.menulist.Count; i++)
            {


                if (env.menulist[i] == env.focused)
                {
                    if (i == 8)
                    {
                        env.focused = env.menulist[2];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }
                    if (i > 0 && i < env.menulist.Count)
                    {
                        env.focused = env.menulist[i - 1];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }
                    else
                    {
                        env.focused = env.menulist[7];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }
                }
            }
        }
        public void next()
        {
            Debug.WriteLine("SAMARAS");
            for (int i = 0; i < env.menulist.Count; i++)
            {
                if (env.menulist[i] == env.focused)
                {

                    if (i == 23)
                    {
                        env.focused = env.menulist[8];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }

                    if (i > -1 && i < 7)
                    {
                        //Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; env.focused.Focus();})); ;
                        env.focused = env.menulist[i + 1];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        // System.Diagnostics.Debug.WriteLine(env.focused);
                        //System.Diagnostics.Debug.WriteLine("next");
                        break;


                    }
                    if (i > 7 && i < 23)
                    {
                        env.focused = env.menulist[i + 1];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        // System.Diagnostics.Debug.WriteLine(env.focused);
                        //System.Diagnostics.Debug.WriteLine("next");
                        break;


                    }
                    else
                    {
                        env.focused = env.menulist[0];
                        // System.Diagnostics.Debug.WriteLine(env.focused);
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }
                }

            }
        }

        void focus_next(KinectTileButton button)
        {

            try
            {
                bordernext(button);
                string newString = string.Concat(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('.'), 0)), "_hover.png");
                Console.WriteLine(newString);
                Dispatcher.BeginInvoke((Action)(() => { button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));
            }
            catch (System.IO.IOException)
            {
                Debug.WriteLine("testnext");
            }
        }

        void focus_previous(KinectTileButton button)
        {
            try
            {
                borderback(button);
                string newString = string.Concat(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('_'), 0)), ".png");
                Console.WriteLine("PREV" + newString);
                Dispatcher.BeginInvoke((Action)(() => { button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));
            }
            catch (System.IO.IOException)
            {
                Debug.WriteLine("testprev");
            }
        }
        void addborders()
        {

            borders.Add(homeBorder);
            borders.Add(recipeBorder);
            borders.Add(shoppingBorder);
            borders.Add(scheduleBorder);
            borders.Add(fridgeBorder);
            borders.Add(alarmBorder);
            borders.Add(preferencesBorder);
            borders.Add(logoutBorder);

        }
        void dayOrnight()
        {
            var src = DateTime.Now;
            Console.WriteLine(src.Hour + ":" + src.Minute);
            if ((src.Hour > 18) || (src.Hour < 6)) { env.intf[0].outputs[2] = true; }
            else env.intf[0].outputs[2] = false;

        }

        void bordernext(KinectTileButton button)
        {
            var bc = new BrushConverter();

            if (button == env.menulist[0]) { borders[0].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[1]) { borders[1].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[2]) { borders[2].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[3]) { borders[3].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[4]) { borders[4].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[5]) { borders[5].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[6]) { borders[6].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }
            if (button == env.menulist[7]) { borders[7].Background = (Brush)bc.ConvertFrom("#882b7a"); return; }


        }
        void borderback(KinectTileButton button)
        {
            var bc = new BrushConverter();

            if (button == env.menulist[0]) { borders[0].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[1]) { borders[1].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[2]) { borders[2].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[3]) { borders[3].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[4]) { borders[4].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[5]) { borders[5].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[6]) { borders[6].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }
            if (button == env.menulist[7]) { borders[7].Background = (Brush)bc.ConvertFrom("#2E8C00"); return; }


        }
        void SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            dayOrnight();
            int index = e.Index;
            int value = e.Value;
            Debug.WriteLine(value);
            //Debug.WriteLine("sensorchange");
            if (!(myTimer == null))
            {
                if (!(right_sensor) && !(left_sensor)) { tickCounter = 0; }


                if (index == 1)
                {
                    //Debug.WriteLine("1");
                    if (value < 27)
                    {
                        right_sensor = true;
                        Debug.WriteLine("right true");
                        if (!(myTimer.Enabled))
                        {
                            myTimer.Enabled = true;
                            // Debug.WriteLine("timerstart");
                        }
                    }
                    else
                    {
                        if ((tickCounter > 3) && (tickCounter < 8) && (right_sensor))
                        {
                            next();
                            Debug.WriteLine("NEXT VALUE SENSOR  " + value);
                        }
                        right_sensor = false;


                        if (!(left_sensor) /* && (right_sensor)*/)
                        {
                            // Debug.WriteLine("right timerstop "+tickCounter);
                            // tickCounter = 0;
                            tickCounter = 0;
                            myTimer.Enabled = false;
                        }
                    }
                }

                if (index == 0)
                {
                    //Debug.WriteLine("0");
                    if (value < 27)
                    {
                        left_sensor = true;
                        // Debug.WriteLine("left true");
                        if (!(myTimer.Enabled))
                        {
                            myTimer.Enabled = true;
                            // Debug.WriteLine("left timerstart");
                        }
                    }
                    else
                    {
                        if ((tickCounter > 5) && (tickCounter < 8) && (left_sensor))
                        {
                            Debug.WriteLine("NEXT prev SENSOR  " + value);
                            previous(); Debug.WriteLine("die ");
                        }

                        left_sensor = false;
                        if (!(right_sensor)/* && (left_sensor)*/)
                        {
                            //Debug.WriteLine(" left timerstop " + tickCounter);
                            //tickCounter = 0;
                            tickCounter = 0;
                            myTimer.Enabled = false;
                        }
                    }
                }
            }
        }





    }
}
