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
    public partial class screen_1_home_logged_in : UserControl, ISwitchable
    {
        Environment env;
        private BarcodeReader barcodeReader;
        Queue<Product> productsScanned;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        System.Timers.Timer myTimer1 = new System.Timers.Timer();
        System.Timers.Timer myTimer3 = new System.Timers.Timer();
        List<Border> borders = new List<Border>();
        Dictionary<int, Product> shoppingList;


        public screen_1_home_logged_in(Environment env)
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
            
            Console.WriteLine(homeBorder.Name.ToString());
            Console.WriteLine(homeBorder.Name.ToString().Equals("homeBorder"));

            myTimer = new System.Timers.Timer(50);
            myTimer.Enabled = false;
            myTimer.Elapsed += OnTimedEvent;

            myTimer3 = new System.Timers.Timer(50);
            myTimer3.Enabled = false;
            myTimer3.Elapsed += OnTimedEvent3;

            myTimer1 = new System.Timers.Timer(500);
            myTimer1.Enabled = false;
            myTimer1.Elapsed += OnTimedEvent1;
            env.focused = home;

            List<Label> expLabels = new List<Label>();
          //  expLabels.Add(exp1);
            //expLabels.Add(exp2);
            //expLabels.Add(exp3);
            //expLabels.Add(exp1);

           
            // 16

            //
            //
            
            env.focused = home;
            taskfill(); shopfill();
            /*string newString = string.Concat(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(recipes.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('.'), 0)), "_hover.png");
            Console.WriteLine(newString);
            Dispatcher.BeginInvoke((Action)(() => { recipes.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));*/
            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(home);
                env.focused = env.menulist[0];
                Dispatcher.BeginInvoke((Action)(() => { env.focused.Focus(); focus_next(env.focused); })); ;
                if (myTimer == null)
                {
                    myTimer = new System.Timers.Timer(50);
                    myTimer.Enabled = false;
                    myTimer.Elapsed += OnTimedEvent;
                }
            };


            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(home);


                if (myTimer == null)
                {
                    myTimer = new System.Timers.Timer(50);
                    myTimer.Enabled = false;
                    myTimer.Elapsed += OnTimedEvent;
                }

                //if (env.expired.Count > 0) { Dispatcher.BeginInvoke((Action)(() => { expLabels[0].Content = env.expired[0].getProductExpDate(); })); }
               // if (env.expired.Count > 1) { Dispatcher.BeginInvoke((Action)(() => { expLabels[1].Content = env.expired[1].getProductExpDate(); })); }
               // if (env.expired.Count > 2) { Dispatcher.BeginInvoke((Action)(() => { expLabels[2].Content = env.expired[2].getProductExpDate(); })); }

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


            productsScanned = new Queue<Product>();

            if (env.using_Sensors == true)
            {
                if (env.using_sonars == true) { 
                env.intf[0].SensorChange += SensorChange;
                env.intf[1].SensorChange += rotatorChange;
                //intf[1].SensorChange += MainWindow_SensorChange2;
                }
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
                //env.intf[0].SensorChange -= Fridge_SensorChange;
                env.intf[0].SensorChange -= SensorChange;
                env.intf[1].SensorChange -= rotatorChange;

                myTimer.Elapsed -= OnTimedEvent;
            }
            if (env.using_Kinect == true)
            {
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady -= NewSensor_ColorFrameReady;
            }

        }

        void MainWindow_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {

            /*   if (e.Index == 0 && e.Value < 500)
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

               //            }*/

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
                case "mid00":
                    ViewSwitcher.Switch(new screen_2_recipes(env));
                    break;
                case "shopping":
                case "shopfast":
                    ViewSwitcher.Switch(new screen_3_shopping_List(env));
                    break;
                case "schedule":
                case "taskfast":
                    ViewSwitcher.Switch(new screen_4_calendar(env));
                    break;
                case "fridgeMenu":
                case "mid11":
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
                //browse options
                case "browse_all":
                    env.recipeMode = 0;
                    ViewSwitcher.Switch(new screen_2_recipes_grid(env));
                    break;
                case "available":
                    env.recipeMode = 1;
                    ViewSwitcher.Switch(new screen_2_recipes_grid(env));
                    break;
                case "time":
                    env.recipeMode = 2;
                    ViewSwitcher.Switch(new screen_2_recipes_grid(env));
                    break;

            }

        }

        //
        int tickCounter = 0;
        Boolean left_sensor = false;
        Boolean right_sensor = false;
        int select = 0;
        Boolean background_change = false;
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
                        /*if ((env.focused == recipes))
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


                    }
                    /*if (i > 7 && i < 14)
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








        void focus_next(KinectTileButton button)
        {

            try
            {
                string newString = string.Concat(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('.'), 0)), "_hover.png");
                Console.WriteLine(newString);
                Dispatcher.BeginInvoke((Action)(() => { button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));
                bordernext(button);
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
                string newString = string.Concat(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().Substring(0, Math.Max(button.Background.GetValue(ImageBrush.ImageSourceProperty).ToString().IndexOf('_'), 0)), ".png");
                Console.WriteLine("PREV" + newString);
                Dispatcher.BeginInvoke((Action)(() => { button.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), newString))); }));
                borderback(button);
            }
            catch (System.IO.IOException)
            {
                Debug.WriteLine("testprev");
            }
        }
        //
        //debugging
        //Steps++;
        int start = 2000;
        int end = 3000;
        int timetick = 0;
        int send = 5000;
        Boolean gotvalue = false;
        Boolean rotatorInUse = false;
        void rotatorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            tickCounter = 0;
            myTimer.Enabled = false;
            rotatorInUse = true;
            int index = e.Index;
            int value = e.Value;
            Debug.WriteLine(value + "" + env.intf[1].inputs[0] + "" + env.intf[1].inputs[1]);
            //Debug.WriteLine("sensorchange");

            if (env.intf[1].inputs[0] == env.intf[1].inputs[1] == false)
            {
                if (!(gotvalue))
                {
                    gotvalue = true;
                    send = end - start;
                    Debug.WriteLine(send + " = " + end + " - " + start + "" + gotvalue);
                    gotvalue = true;
                    Debug.WriteLine(gotvalue);

                }
                else
                {
                    send = end - start;
                    gotvalue = false;
                    myTimer3.Enabled = false;
                    Debug.WriteLine(send + " = " + end + " - " + start + "" + gotvalue + "BLABLABLALBLA1234213213");
                    send = 5000;
                    timetick = 0;
                }

            }
            if (env.intf[1].inputs[0] == env.intf[1].inputs[1] == true)
            {
                if (myTimer3.Enabled == false)
                {
                    myTimer3.Enabled = true;
                    start = value;
                    Debug.WriteLine(send + " = " + end + " - " + start);

                }
                else
                {
                    if (value < 400)
                    {
                        value = value + 1000;
                    }

                    end = value; send = end - start; Debug.WriteLine(send + " = " + end + " - " + start);
                }

            }



        }

        void OnTimedEvent3(Object myObject, EventArgs myEventArgs)
        {
            timetick++;
            if (send < 0) send = send + 1000;
            if ((env.intf[1].inputs[1] == false) && (send < 5000))
            {
                if ((send > 0) && (send < 250))
                {
                    Debug.WriteLine("5 lepta");
                    Debug.WriteLine(timetick + " " + send + " = " + end + " - " + start + "" + gotvalue); send = 5000; myTimer3.Enabled = false; gotvalue = false; timetick = 0; Debug.WriteLine("BLABLALBLABLABL");
                    alarmCounter = 2;
                    myTimer1.Enabled = true;
                    rotatorInUse = false;
                    return;
                }
                if ((send > 0) && (send < 380))
                {
                    Debug.WriteLine("15 lepta");
                    Debug.WriteLine(timetick + " " + send + " = " + end + " - " + start + "" + gotvalue); send = 5000; myTimer3.Enabled = false; gotvalue = false; timetick = 0; Debug.WriteLine("BLABLALBLABLABL");

                    alarmCounter = 4;
                    myTimer1.Enabled = true;
                    rotatorInUse = false;
                    return;
                }
                if ((send > 0) && (send < 600))
                {
                    Debug.WriteLine("30 lepta");
                    Debug.WriteLine(timetick + " " + send + " = " + end + " - " + start + "" + gotvalue); send = 5000; myTimer3.Enabled = false; gotvalue = false; timetick = 0; Debug.WriteLine("BLABLALBLABLABL");
                    alarmCounter = 6;
                    myTimer1.Enabled = true;
                    rotatorInUse = false;
                    return;
                }
                if (send > 0)
                {
                    Debug.WriteLine("45 lepta");
                    Debug.WriteLine(timetick + " " + send + " = " + end + " - " + start + "" + gotvalue); send = 5000; myTimer3.Enabled = false; gotvalue = false; timetick = 0; Debug.WriteLine("BLABLALBLABLABL");
                    alarmCounter = 8;
                    myTimer1.Enabled = true;
                    rotatorInUse = false;
                    return;
                }
                Debug.WriteLine("60 lepta");
                Debug.WriteLine(timetick + " " + send + " = " + end + " - " + start + "" + gotvalue); send = 5000; myTimer3.Enabled = false; gotvalue = false; timetick = 0; Debug.WriteLine("BLABLALBLABLABL");
                alarmCounter = 10;
                myTimer1.Enabled = true;
                rotatorInUse = false;
            }

        }

        int alarmCounter = 20;
        void OnTimedEvent1(Object myObject, EventArgs myEventArgs)
        {
            System.Diagnostics.Debug.WriteLine("this bla tick" + tickCounter);
            alarmCounter--;
            if (alarmCounter == 0) { lightchange(); myTimer1.Enabled = false; }
            else { lightchange(); }
        }

        void lightchange()
        {

            if (env.intf[0].outputs[0] == false) { env.intf[0].outputs[0] = true; }
            else env.intf[0].outputs[0] = false;

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
        void dayOrnight()
        {
            var src = DateTime.Now;
            Console.WriteLine(src.Hour + ":" + src.Minute);
            if ((src.Hour > 18) || (src.Hour < 6)) { env.intf[0].outputs[2] = true; }
            else env.intf[0].outputs[2] = false;

        }


        void taskfill()
        {

            if (env.agenda.events.Count > 0)
            {
               
             Dispatcher.BeginInvoke((Action)(() => { 
                 r0.Text = env.agenda.events[0].getrec().getName().ToString();
             d0.Text = env.agenda.events[0].getDate().Date.ToString("dd/MM/yyyy");
             
             }));
                
            }
            if (env.agenda.events.Count > 1)
            {

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    r1.Text = env.agenda.events[0].getrec().getName().ToString();
                    d1.Text = env.agenda.events[0].getDate().Date.ToString("dd/MM/yyyy");

                }));

            }
            if (env.agenda.events.Count > 2)
            {

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    r2.Text = env.agenda.events[0].getrec().getName().ToString();
                    d2.Text = env.agenda.events[0].getDate().Date.ToString("dd/MM/yyyy");

                }));

            }
        }

        void shopfill() 
        {
            if (env.shoppingLists.Count != 0)
            {
                shoppingList = env.shoppingLists.Peek();
                Product[] products = new Product[16];
                shoppingList.Values.CopyTo(products, 0);

                if (products[0] != null)
                {
                    sh0.Text = products[0].getProductName();
                }
                if (products[1] != null)
                {
                    sh1.Text = products[1].getProductName();
                }
                if (products[2] != null)
                {
                    sh2.Text = products[2].getProductName();
                }
                if (products[3] != null)
                {
                    sh3.Text = products[3].getProductName();
                }
                if (products[4] != null)
                {
                    sh4.Text = products[4].getProductName();
                } if (products[5] != null)
                {
                    sh5.Text = products[5].getProductName();
                }
                if (products[6] != null)
                {
                    sh6.Text = products[6].getProductName();
                }
                if (products[7] != null)
                {
                    sh7.Text = products[7].getProductName();
                }
                


            }
        
        
        
        
        
        }


    }
}
