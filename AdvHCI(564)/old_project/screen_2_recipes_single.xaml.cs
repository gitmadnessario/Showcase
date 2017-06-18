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
    public partial class screen_2_recipes_single : UserControl, ISwitchable
    {
        Environment env;
        private BarcodeReader barcodeReader;
        int index = 0;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        int extramenuitem = 0;
        
        List<Border> borders = new List<Border>();
        List<TextBlock> blocks = new List<TextBlock>();
        Boolean stack = false;
        TextBlock focused;



        public screen_2_recipes_single(Environment env)
        {
            InitializeComponent();
            addborders(); 
            addblocks();
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
            env.menulist.Add(addcal); extramenuitem++;
            env.menulist.Add(SelectRecipe); extramenuitem++;
            //10-1
            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(recipes);
                env.focused = env.menulist[8];
                Dispatcher.BeginInvoke((Action)(() => { env.focused.Focus(); focus_next(env.focused); })); ;
                if (myTimer == null)
                {
                    myTimer = new System.Timers.Timer(50);
                    myTimer.Enabled = false;
                    myTimer.Elapsed += OnTimedEvent;
                }
            };
            //
            //
            myTimer = new System.Timers.Timer(50);
            myTimer.Enabled = false;
            myTimer.Elapsed += OnTimedEvent;
            env.focused = recipes;
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
            InitializeDate();
            if (env.using_Sensors == true)
            {
                if (env.using_sonars == true)
                {
                    env.intf[0].SensorChange += SensorChange;
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
                    HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized += refresh_GestureRecognized;
                    HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady += NewSensor_ColorFrameReady;
                    barcodeReader = new BarcodeReader();
                    barcodeReader.Options.PossibleFormats = new List<BarcodeFormat> { ZXing.BarcodeFormat.QR_CODE };
                }

                env.skipIngredients = true;
            
            
            refreshData();
        }


        public void refreshData()
        {


            int final = env.cookingRecipe.getSteps().Count;
            string stepstring = "+";

            for (int i = 0; i <= final; i++)
            {
                stepstring += "--+";
            }


            Dictionary<String, String> ingredients = env.cookingRecipe.getIngredients();
            Dispatcher.BeginInvoke((Action)(() => { RTitle.Content = env.cookingRecipe.getName() + " (" + env.cookingRecipe.getCookingTime() + " minutes)"; }));
            env.speech.SpeakAsync(env.cookingRecipe.getName() + ". Cooking time: " + env.cookingRecipe.getCookingTime() + " minutes.");
            Dispatcher.BeginInvoke((Action)(() => { StepImage.Content = stepstring; }));
            Dispatcher.BeginInvoke((Action)(() => { RecipeImage.Source = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), env.cookingRecipe.getPicturePath())); }));

            if (ingredients.Count > 0)
            {
                Dispatcher.BeginInvoke((Action)(() => { i0.Text = ingredients.ElementAt(0).Value; }));
            }
            if (ingredients.Count > 1)
            {
                Dispatcher.BeginInvoke((Action)(() => { i1.Text = ingredients.ElementAt(1).Value; }));
            }
            if (ingredients.Count > 2)
            {
                Dispatcher.BeginInvoke((Action)(() => { i2.Text = ingredients.ElementAt(2).Value; }));
            }
            if (ingredients.Count > 3)
            {
                Dispatcher.BeginInvoke((Action)(() => { i3.Text = ingredients.ElementAt(3).Value; }));
            }
            if (ingredients.Count > 4)
            {
                Dispatcher.BeginInvoke((Action)(() => { i4.Text = ingredients.ElementAt(4).Value; }));
            }
            if (ingredients.Count > 5)
            {
                Dispatcher.BeginInvoke((Action)(() => { i5.Text = ingredients.ElementAt(5).Value; }));
            }
            if (ingredients.Count > 6)
            {
                Dispatcher.BeginInvoke((Action)(() => { i6.Text = ingredients.ElementAt(6).Value; }));
            }
            if (ingredients.Count > 7)
            {
                Dispatcher.BeginInvoke((Action)(() => { i7.Text = ingredients.ElementAt(7).Value; }));
            }
            if (ingredients.Count > 8)
            {
                Dispatcher.BeginInvoke((Action)(() => { i8.Text = ingredients.ElementAt(8).Value; }));
            }
            if (ingredients.Count > 9)
            {
                Dispatcher.BeginInvoke((Action)(() => { i9.Text = ingredients.ElementAt(9).Value; }));
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
                HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized -= refresh_GestureRecognized;

            }
        }



        private void KinectPressButton(object sender, RoutedEventArgs e)
        {
            env.speech.SpeakAsyncCancelAll();
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
                case "SelectRecipe":
                    ViewSwitcher.Switch(new screen_2_recipes_single_step_1(env));
                    break;
                case "addcal":
                    Dispatcher.BeginInvoke((Action)(() => { this.popup1.Visibility = Visibility.Visible; stack = true; blocks.ElementAt(0).Background = Brushes.Green; focused = Day; }));
                    break;
                case "addq":
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                      this.popup1.Visibility = Visibility.Hidden; stack=false;
                        DateTime newdate = new DateTime(Int32.Parse(Year.Text), Int32.Parse(Month.Text), Int32.Parse(Day.Text), Int32.Parse(Hours.Text), Int32.Parse(Mins.Text), 0);
                        scheduled eventToAdd = new scheduled(newdate, env.cookingRecipe);
                        env.agenda.events.Add(eventToAdd);
                        
                    }));
                   

                    break;



            }
        }

        public void InitializeDate()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Day.Text = DateTime.Now.Day.ToString();
                Month.Text = DateTime.Now.Month.ToString();
                Year.Text = DateTime.Now.Year.ToString();
                Hours.Text = "10";
                Mins.Text = "00";

            }));
        }

        void refresh_GestureRecognized(GestureType arg1, int arg2)
        {
            if (stack)
            {
                
            
            // myTimer.Stop();
            if (arg1 == GestureType.SwipeRight)
            {
                 Dispatcher.BeginInvoke((Action)(() =>
                {
                if (focused.Name.ToString() == blocks.ElementAt(0).Name.ToString())
                {
                    focused.Foreground = Brushes.Black;
                    focused.Background = Brushes.Transparent;
                    focused = blocks.ElementAt(4);
                    focused.Background = Brushes.Green;
                    focused.Foreground = Brushes.White;
                }
                else
                {
                    for (int i = 4; i > 0; i--)
                    {
                        Debug.WriteLine("check" + i);
                        if (focused == blocks.ElementAt(i))
                        {
                            Debug.WriteLine("found" + i);
                            focused.Foreground = Brushes.Black;
                            focused.Background = Brushes.Transparent;
                            focused = blocks.ElementAt(i - 1); 
                            focused.Background = Brushes.Green;
                            focused.Foreground = Brushes.White;
                            break;

                        }
                    }
                }
                }));
                Console.WriteLine("SwipeLeft");

                //Dispatcher.BeginInvoke((Action)(() => { time.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "Resources\\rectime.png"))); }));
                //var d = time.Background.GetValue(ImageBrush.ImageSourceProperty).ToString;
                // Console.WriteLine(time.Background.GetValue(ImageBrush.ImageSourceProperty).ToString());
                // env.intf[0].outputs[2] = false;


            }
            if (arg1 == GestureType.SwipeLeft)
            {
                 Dispatcher.BeginInvoke((Action)(() =>
                {
                if (focused.Name.ToString() == blocks.ElementAt(4).Name.ToString())
                {
                    focused.Foreground = Brushes.Black;
                    focused.Background = Brushes.Transparent;
                    focused = blocks.ElementAt(0);
                    focused.Background = Brushes.Green;
                    focused.Foreground = Brushes.White;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Debug.WriteLine("check" + i);
                        if (focused == blocks.ElementAt(i))
                        {
                            Debug.WriteLine("found" + i);
                            focused.Foreground = Brushes.Black;
                            focused.Background = Brushes.Transparent;
                            focused = blocks.ElementAt(i + 1);
                            focused.Background = Brushes.Green;
                            focused.Foreground = Brushes.White;
                            break;

                        }
                    }
                }
                }));


                Console.WriteLine("SwipeRight");
               
            }
            if (arg1 == GestureType.SwipeDownLeft)
            {
                Console.WriteLine("SwipeDownLeft");
                Dispatcher.BeginInvoke((Action)(() =>
                {
                   /* index++;
                    Day.Text = (DateTime.Now.Day + index).ToString();
                    //  Month.Text = (DateTime.Now.Month + index).ToString();
                    //  Year.Text = (DateTime.Now.Year + index).ToString();
                    Hours.Text = "10";
                    Mins.Text = "00";
                    */
                int userVal = int.Parse(focused.Text);
                userVal--;
                if( focused.Name.ToString()  == Day.Name.ToString()) {   if (userVal < 1){userVal = 30;}}
                if (focused.Name.ToString() == Month.Name.ToString()) { if (userVal <1) { userVal = 12; } }
                if (focused.Name.ToString() == Hours.Name.ToString()) { if (userVal < 1) { userVal = 24; } }
                if (focused.Name.ToString() == Mins.Name.ToString()) { userVal = userVal - 9; if (userVal < 0) { userVal = 50; } }

                focused.Text = userVal.ToString();
                }));
            }
            if (arg1 == GestureType.SwipeUpRight)
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    /*index--;
                    Day.Text = (DateTime.Now.Day - index).ToString();
                    Month.Text = (DateTime.Now.Month - index).ToString();
                    Year.Text = (DateTime.Now.Year - index).ToString();
                    Hours.Text = "10";
                    Mins.Text = "00"; */

               

                int userVal = int.Parse(focused.Text);
                userVal++;
                if( focused.Name.ToString()  == Day.Name.ToString()) {   if (userVal > 30){userVal = 1;}}
                if (focused.Name.ToString() == Month.Name.ToString()) { if (userVal > 12) { userVal = 1; } }
                if (focused.Name.ToString() == Hours.Name.ToString()) { if (userVal > 24) { userVal = 1; } }
                if (focused.Name.ToString() == Mins.Name.ToString()) { userVal = userVal + 9; if (userVal > 51) { userVal = 0; } }

                focused.Text = userVal.ToString();
                }));
                Console.WriteLine("SwipeUpright");
                // env.intf[0].outputs[2] = true;
            }
            if (arg1 == GestureType.JoinedHands)
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {

                    DateTime date;
                    scheduled eventToAdd;

                    date = new DateTime(Convert.ToInt32(Year.Text), Convert.ToInt32(Month.Text), Convert.ToInt32(Day.Text));
                    Recipe current = env.cookingRecipe;
                    eventToAdd = new scheduled(date, current);
                    env.agenda.events.Add(eventToAdd);
                    addq.Focus();

                }));
                Console.WriteLine("SwipeUpright");
                // env.intf[0].outputs[2] = true;
            }
            }
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
                        if ((env.focused == recipes))
                        {

                            background_change = true;

                            Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; focus_previous(recipes); env.focused.Focus(); focus_next(env.focused); })); ;
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

                    if (i == 9)
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
                    if (i > 7 && i < 9)
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

        void addblocks()
        {

            blocks.Add(Day);
            blocks.Add(Month);
            blocks.Add(Year);
            blocks.Add(Hours);
            blocks.Add(Mins);
            focused = Day;
            Day.Background = Brushes.Green;
            Day.Foreground = Brushes.White;
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
                        if ((tickCounter > 4) && (tickCounter < 8) && (right_sensor))
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
                        if ((tickCounter > 4) && (tickCounter < 8) && (left_sensor))
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
