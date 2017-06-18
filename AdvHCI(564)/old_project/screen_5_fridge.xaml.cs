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
    public partial class screen_5_fridge : UserControl, ISwitchable
    {


        Environment env;
        BarcodeReader barcodeReader;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        int extramenuitem = 0;
        List<Border> borders = new List<Border>();

        public screen_5_fridge(Environment env)
        {
            InitializeComponent();
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

            // 8

            //
            //
            myTimer = new System.Timers.Timer(50);
            myTimer.Enabled = false;
            myTimer.Elapsed += OnTimedEvent;
            env.focused = fridgeMenu;
            /*string newString = string.Concat(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('.'), 0)), "_hover.png");
            Console.WriteLine(newString);
            Dispatcher.BeginInvoke((Action)(() => { recipes.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));*/
            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(fridgeMenu);
                env.focused = env.menulist[4];
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

            refreshData();

            if (env.using_Sensors == true)
            {
                env.intf[0].SensorChange += Fridge_SensorChange;
                
                Console.WriteLine("BLAAAAAAAAAAAAAAA");
                if (env.using_sonars == true)
                {
                    env.intf[0].SensorChange += SensorChange;
                    //intf[1].SensorChange += MainWindow_SensorChange2;
                }
            }
        }


        public void refreshData()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                N0.Text = env.fridge.checkSlot(0).getProductName();
                V0.Text = env.fridge.checkSlot(0).getProductExpDate();
                N1.Text = env.fridge.checkSlot(1).getProductName();
                V1.Text = env.fridge.checkSlot(1).getProductExpDate();
                N2.Text = env.fridge.checkSlot(2).getProductName();
                V2.Text = env.fridge.checkSlot(1).getProductExpDate();
                N3.Text = env.fridge.checkSlot(2).getProductName();
                V3.Text = env.fridge.checkSlot(3).getProductExpDate();
                N4.Text = env.fridge.checkSlot(4).getProductName();
                V4.Text = env.fridge.checkSlot(4).getProductExpDate();
                Console.WriteLine("this" + env.fridge.checkSlot(0).getProductName());
            }));


            if (env.scannedProducts.Count > 0)
                Dispatcher.BeginInvoke((Action)(() => { S0.Text = env.scannedProducts.ElementAt(0).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { S0.Text = ""; }));
            if (env.scannedProducts.Count > 1)
                Dispatcher.BeginInvoke((Action)(() => { S1.Text = env.scannedProducts.ElementAt(1).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { S1.Text = ""; }));
            if (env.scannedProducts.Count > 2)
                Dispatcher.BeginInvoke((Action)(() => { S2.Text = env.scannedProducts.ElementAt(2).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { S2.Text = ""; }));
            if (env.scannedProducts.Count > 3)
                Dispatcher.BeginInvoke((Action)(() => { S3.Text = env.scannedProducts.ElementAt(3).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { S3.Text = ""; }));
            if (env.scannedProducts.Count > 4)
                Dispatcher.BeginInvoke((Action)(() => { S4.Text = env.scannedProducts.ElementAt(4).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { S4.Text = ""; }));

            if (env.removedItems.Count > 0)
                Dispatcher.BeginInvoke((Action)(() => { R0.Text = env.removedItems.ElementAt(0).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { R0.Text = ""; }));
            if (env.removedItems.Count > 1)
                Dispatcher.BeginInvoke((Action)(() => { R1.Text = env.removedItems.ElementAt(1).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { R1.Text = ""; }));
            if (env.removedItems.Count > 2)
                Dispatcher.BeginInvoke((Action)(() => { R2.Text = env.removedItems.ElementAt(2).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { R2.Text = ""; }));
            if (env.removedItems.Count > 3)
                Dispatcher.BeginInvoke((Action)(() => { R3.Text = env.removedItems.ElementAt(3).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { R3.Text = ""; }));
            if (env.removedItems.Count > 4)
                Dispatcher.BeginInvoke((Action)(() => { R4.Text = env.removedItems.ElementAt(4).getProductName(); }));
            else
                Dispatcher.BeginInvoke((Action)(() => { R4.Text = ""; }));
            Console.WriteLine("BLAAAAAAAAAAAAAAA");
            // dayOrnight();
            lightHandling();
        }

        public void lightHandling()
        {
            if (env.using_Sensors == true)
            {
                if (env.fridge.checkSlot(0).getProductName() == "empty")
                    env.intf[0].outputs[3] = true;
                else
                    env.intf[0].outputs[3] = false;

                if (env.fridge.checkSlot(1).getProductName() == "empty")
                    env.intf[0].outputs[4] = true;
                else
                    env.intf[0].outputs[4] = false;

                if (env.fridge.checkSlot(2).getProductName() == "empty")
                    env.intf[0].outputs[6] = true;
                else
                    env.intf[0].outputs[6] = false;

                if (env.fridge.checkSlot(3).getProductName() == "empty")
                    env.intf[0].outputs[5] = true;
                else
                    env.intf[0].outputs[5] = false;

                if (env.fridge.checkSlot(4).getProductName() == "empty")
                    env.intf[0].outputs[7] = true;
                else
                    env.intf[0].outputs[7] = false;
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
                env.intf[0].SensorChange -= Fridge_SensorChange;
                env.intf[0].SensorChange -= SensorChange;
                myTimer.Elapsed -= OnTimedEvent;
            }

            if (env.using_Kinect == true)
            {
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady -= NewSensor_ColorFrameReady;
                BindingOperations.ClearBinding(this.kinectRegion, KinectRegion.KinectSensorProperty);
            }
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
                    ViewSwitcher.Switch(new login(env));
                    break;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            ViewSwitcher.Switch(new fscreen());
        }

        void NewSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    var qr_Result = barcodeReader.Decode(colorFrame.GetRawPixelData(), colorFrame.Width, colorFrame.Height,
                                                       ZXing.RGBLuminanceSource.BitmapFormat.RGB32);

                    if (qr_Result != null)
                    {
                        Console.WriteLine("DETECTED QR = " + qr_Result.Text);
                        // this.ScreenLabel.Content = qr_Result.Text;
                        string[] words = qr_Result.Text.Split(' ');

                        //   Dispatcher.BeginInvoke((Action)(() => { LogInf.Content = qr_Result.Text; }));

                        if (words[0] == "Product:")
                        {
                            bool change = true;
                            Product product = new Product(words[1], words[2]);
                            if (env.scannedProducts.Count > 0)
                            {
                                for (int i = 0; i < env.scannedProducts.Count; i++)
                                {
                                    if (env.scannedProducts.ElementAt(i).getProductName() == product.getProductName())
                                    {
                                        change = false;
                                    }
                                }
                            }
                            if (change)
                            {
                                env.scannedProducts.Enqueue(product);
                                refreshData();
                            }

                        }
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

        void Fridge_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            int index = e.Index;
            int value = e.Value;
            int fridgeSlot = index - 3;
            if (index > 2 && index < 8)
            {
                Product product = new Product("empty", "no");
                if (value < 900)
                {
                    Console.WriteLine("PRESSED SENSOR = " + index);
                    if (env.fridge.checkSlot(fridgeSlot).getProductName() == "empty")
                    {
                        if (env.scannedProducts.Count > 0)
                        {
                            product = env.scannedProducts.Dequeue();
                        }
                        else
                        {
                            if (env.removedItems.Count > 0)
                            {
                                product = env.removedItems.Pop();
                            }
                        }
                        env.fridge.addProductToFridge(product, fridgeSlot);
                    }
                }
                else
                {
                    if (env.fridge.checkSlot(index - 3).getProductName() != "empty")
                    {
                        product = env.fridge.removeProductFromFridge(fridgeSlot);
                        if (product.getProductName() != "empty")
                        {
                            env.removedItems.Push(product);
                        }
                    }
                }

                refreshData();
            }
            Thread.Sleep(100);
        }

        //navigate


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
                        /* if ((env.focused == fridgeMenu))
                         {

                             background_change = true;

                             Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; focus_previous(recipes); env.focused.Focus(); focus_next(env.focused); })); ;
                             return;//focus_previous(env.menulist[i]);env.focused.Focus(); focus_next(env.focused); 
                             //if flag focusprev(env focused) env focused = tade focus_next(tade) env.focused.focus();
                         }*/
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
                        env.focused = env.menulist[1];
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

                    /*if (i == 14)
                    {
                        env.focused = env.menulist[8];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        break;
                    }*/

                    if (i > -1 && i < 7)
                    {
                        //Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; env.focused.Focus();})); ;
                        env.focused = env.menulist[i + 1];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        // System.Diagnostics.Debug.WriteLine(env.focused);
                        //System.Diagnostics.Debug.WriteLine("next");
                        break;


                    }/*
                    if (i > 7 && i < 14)
                    {
                        env.focused = env.menulist[i + 1];
                        Dispatcher.BeginInvoke((Action)(() => { focus_previous(env.menulist[i]); env.focused.Focus(); focus_next(env.focused); })); ;
                        // System.Diagnostics.Debug.WriteLine(env.focused);
                        //System.Diagnostics.Debug.WriteLine("next");
                        break;


                    }*/
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
      
        void SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            dayOrnight();
            int index = e.Index;
            int value = e.Value;
            //  Debug.WriteLine(value);
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
                        if ((tickCounter > 5) && (tickCounter < 8) && (right_sensor))
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

        void dayOrnight()
        {
            var src = DateTime.Now;
            Console.WriteLine(src.Hour + ":" + src.Minute);
            if ((src.Hour > 18) || (src.Hour < 6)) { env.intf[0].outputs[2] = true; }
            else env.intf[0].outputs[2] = false;

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
    }
}
