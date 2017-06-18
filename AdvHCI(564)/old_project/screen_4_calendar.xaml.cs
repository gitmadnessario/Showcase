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
    public partial class screen_4_calendar : UserControl, ISwitchable
    {
        Environment env;
        private BarcodeReader barcodeReader;
        List<Recipe> currentRecipes = new List<Recipe>();
        List<TextBlock> days = new List<TextBlock>();
        int tell = 0;
        System.Timers.Timer myTimer = new System.Timers.Timer();
        int extramenuitem = 0;
        List<Border> borders = new List<Border>();

        public screen_4_calendar(Environment env)
        {
            InitializeComponent();

            days.Add(cal_0);//monday
            days.Add(cal_1);//tuesday
            days.Add(cal_2);//wed
            days.Add(cal_3);//thur
            days.Add(cal_4);//friday
            days.Add(cal_5);//satur
            days.Add(cal_6);//sunday

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
            env.menulist.Add(wback); extramenuitem++;
            env.menulist.Add(wnext); extramenuitem++;
            this.Loaded += delegate
            {
                //env.intf[0].ratiometric = true;
                Keyboard.Focus(schedule);
                env.focused = env.menulist[9];
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


            for (int i = 0; i < env.recipebook.amountOfRecipes(); i++)
            {
                currentRecipes.Add(env.recipebook.getRecipe(i));
            }

            if (env.recipeMode == 2)
            {
                currentRecipes = currentRecipes.OrderBy(o => o.cookingTime).ToList();
            }

            //refreshData();

            Console.WriteLine(DateTime.Now.Month);
            Console.WriteLine(DateTime.Now.Year);
            Console.WriteLine(DateTime.Now.ToString());
            DateTime dateValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int test = (int)dateValue.DayOfWeek;
            Console.WriteLine(test);

            //Dispatcher.BeginInvoke((Action)(() => { days[test].Text = day.ToString() + "/" + month.ToString() ; }));
            DateTime input = DateTime.Now;
            int delta = DayOfWeek.Monday - input.DayOfWeek;

            DateTime calculate = input.AddDays(delta);
            int month = calculate.Month;
            int day = calculate.Day;
            Console.WriteLine(calculate.ToString("dd/MM"));
            Console.WriteLine(day + "" + month);

            /* for (int m=0;m < days.Count-1; m++)
            {
               // Dispatcher.BeginInvoke((Action)(() => { days[i].Text = day.ToString() + "/" + month.ToString() ; }));
                //Dispatcher.BeginInvoke((Action)(() => { days[i].Text = i.ToString(); }));
                Console.WriteLine((day+m).ToString() + "/" + (month).ToString());
                Dispatcher.BeginInvoke((Action)(() => { days[m].Text = (day+m).ToString() + "/" + (month).ToString(); }));
            }
             */
            /*
            int j = 0;
            Dispatcher.BeginInvoke((Action)(() => { days[j].Text = (day + 0).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j+1].Text = (day + 1).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j + 2].Text = (day + 2).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j + 3].Text = (day + 3).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j + 4].Text = (day + 4).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j + 5].Text = (day + 5).ToString() + "/" + (month).ToString(); }));
            Dispatcher.BeginInvoke((Action)(() => { days[j + 6].Text = (day + 6).ToString() + "/" + (month).ToString(); }));
            
           Console.WriteLine(calculate.ToString("dd/MM"));
            */

            calendar();



            //Dispatcher.BeginInvoke((Action)(() => { days[test-1].Text = (day-1).ToString() + "/" + month.ToString(); }));
            /*for (int i=0; test < days.Count; test++)
            {
                Dispatcher.BeginInvoke((Action)(() => { days[test+i].Text = (day+i).ToString() + "/" + (month+i).ToString() ; }));
            }
            /*
            for (int i = 1; test < 0; test--)
            {
                Dispatcher.BeginInvoke((Action)(() => { days[test - i].Text = (day - i).ToString() + "/" + (month - i).ToString(); }));
            }

           /*
            Console.WriteLine(day+"/"+month);
          
           

            /*for (int i = 0; i < 7; i++)
            {
                Dispatcher.BeginInvoke((Action)(() => { days[test].Text = DateTime.Now.ToString("dd/MM"); }));


            }*/
            DateTime thisDate1 = DateTime.Now;
            Console.WriteLine("Today is " + thisDate1.ToString("dd/MM") + ".");
            /* while (true) {

                
            
             }*/

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

        void MainWindow_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {

           
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
                case "ToIngredients":
                    ViewSwitcher.Switch(new screen_3_store_ingredients(env));
                    break;
                case "wnext":
                    tell++;
                    calendar();
                    break;
                case "wback":
                    tell--;
                    calendar();
                    break;

                    
            }

        }


        public void calendar()
        {

            moncont.Children.Clear();
            tuecont.Children.Clear();
            wedcont.Children.Clear();
            thurcont.Children.Clear();
            fricont.Children.Clear();
            satcont.Children.Clear();
            suncont.Children.Clear();
            DateTime input;
            int delta, month, day;
            DateTime date_in = DateTime.Now.AddDays(7 * tell);
            Console.WriteLine("\\\\\\\\\\\\\\\\\\" + DateTime.Now.DayOfWeek.ToString() + DateTime.Now.DayOfWeek.ToString().Equals("Sunday").ToString());
            var bc = new BrushConverter();
            if (DateTime.Now.DayOfWeek.ToString().Equals("Sunday"))
            {
                date_in = date_in.AddDays(-7);

            }


            input = date_in;
            delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta);
            month = monday.Month;
            day = monday.Day;
            // Console.WriteLine(calculate.ToString("\n"+"dd/MM"));
            Console.WriteLine("MONDAY" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[0].Text = monday.Day.ToString() + "/" + monday.Month.ToString(); }));
            // Dispatcher.BeginInvoke((Action)(() => { Monday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); 
            if (DateTime.Now.Equals(monday)) { Dispatcher.BeginInvoke((Action)(() => { Monday.Fill = (Brush)bc.ConvertFrom("#00a3ff");  })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Monday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }


            input = date_in;
            delta = DayOfWeek.Tuesday - input.DayOfWeek;
            DateTime tuesday = input.AddDays(delta);
            month = tuesday.Month;
            day = tuesday.Day;
            // Console.WriteLine(calculate.ToString("\n" + "dd/MM"));
            Console.WriteLine("TUESDAY" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[1].Text = tuesday.Day.ToString() + "/" + tuesday.Month.ToString(); }));

            if (DateTime.Now.Equals(tuesday)) { Dispatcher.BeginInvoke((Action)(() => { Tuesday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Tuesday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }

            input = date_in;
            delta = DayOfWeek.Wednesday - input.DayOfWeek;
            DateTime wednesday = input.AddDays(delta);
            month = wednesday.Month;
            day = wednesday.Day;
            // Console.WriteLine(calculate.ToString("\n" + "dd/MM"));
            Console.WriteLine("WED" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[2].Text = wednesday.Day.ToString() + "/" + wednesday.Month.ToString(); }));

            if (DateTime.Now.Equals(wednesday)) { Dispatcher.BeginInvoke((Action)(() => { Wednesday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Wednesday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }

            input = date_in;
            delta = DayOfWeek.Thursday - input.DayOfWeek;
            DateTime thursday = input.AddDays(delta);
            month = thursday.Month;
            day = thursday.Day;
            // Console.WriteLine(calculate.ToString("\n" + "dd/MM"));
            Console.WriteLine("THUR" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[3].Text = thursday.Day.ToString() + "/" + thursday.Month.ToString(); }));

            if (DateTime.Now.Equals(thursday)) { Dispatcher.BeginInvoke((Action)(() => { Thursday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Thursday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }

            input = date_in;
            delta = DayOfWeek.Friday - input.DayOfWeek;
            DateTime friday = input.AddDays(delta);
            month = friday.Month;
            day = friday.Day;
            // Console.WriteLine(calculate.ToString("\n" + "dd/MM"));
            Console.WriteLine("FRID" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[4].Text = friday.Day.ToString() + "/" + friday.Month.ToString(); }));

            if (DateTime.Now.Equals(friday)) { Dispatcher.BeginInvoke((Action)(() => { Friday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Friday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }

            input = date_in;
            delta = DayOfWeek.Saturday - input.DayOfWeek;
            DateTime saturday = input.AddDays(delta);
            month = saturday.Month;
            day = saturday.Day;
            //Console.WriteLine(calculate.ToString("\n" + "dd/MM"));
            Console.WriteLine("AEKAEKAEKAKAE   " + DateTime.Now.ToString() + " " + saturday.ToString());
            Dispatcher.BeginInvoke((Action)(() => { days[5].Text = saturday.Day.ToString() + "/" + saturday.Month.ToString(); }));

            if (DateTime.Now.Equals(saturday)) { Dispatcher.BeginInvoke((Action)(() => { Saturday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); })); }
            else { Dispatcher.BeginInvoke((Action)(() => { Saturday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White); })); }

            input = date_in.AddDays(7);
            delta = DayOfWeek.Sunday - input.DayOfWeek;
            DateTime sunday = input.AddDays(delta);
            month = sunday.Month;
            day = sunday.Day;
            Console.WriteLine(sunday.ToString("\n" + "dd/MM"));
            Console.WriteLine("SUNDAY" + day + "" + month + "\n");
            Dispatcher.BeginInvoke((Action)(() => { days[6].Text = sunday.Day.ToString() + "/" + sunday.Month.ToString();
            WeekRange.Text = days[0].Text + " - " + days[6].Text;
            
            }));

            if (DateTime.Now.Equals(sunday))
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    Sunday.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red);


                }));
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() =>
                {
                    Sunday.Fill = new SolidColorBrush(System.Windows.Media.Colors.White);


                }));
            }

            Dispatcher.BeginInvoke((Action)(() =>
            { checkdate(sunday, suncont); checkdate(monday, moncont); checkdate(tuesday, tuecont); checkdate(wednesday, wedcont); 
                checkdate(thursday, thurcont); 
                checkdate(friday, fricont); checkdate(saturday, satcont); }));
            // Console.WriteLine(sunday.ToString() + "  " + env.agenda.getScheduled(0).getDate().ToString());
        }

        void checkdate(DateTime day,WrapPanel panel)
        {

            if (env.agenda.events.Count > 0)
            { if ((day.Month.Equals(env.agenda.getScheduled(0).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(0).getDate().Day)))
                    { Dispatcher.BeginInvoke((Action)(() =>{
                        panel.Children.Add(new TextBlock
                            {
                                TextWrapping = TextWrapping.WrapWithOverflow,
                                FontSize = 20,
                                Text = env.agenda.events[0].getrec().getName().ToString(),
                                Margin = new Thickness(5)
                            });}));
                }

            }
            
            if (env.agenda.events.Count > 1)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(1).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(1).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[1].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }
            
            if (env.agenda.events.Count > 2)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(2).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(2).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[2].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }

            /*
            if (env.agenda.events.Count > 3)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(3).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(3).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[3].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }
            if (env.agenda.events.Count > 4)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(4).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(4).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[4].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }
            if (env.agenda.events.Count > 0)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(5).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(5).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[5].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }
            if (env.agenda.events.Count > 6)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(6).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(6).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[6].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }
            if (env.agenda.events.Count > 7)
            {
                if ((day.Month.Equals(env.agenda.getScheduled(7).getDate().Month)) && (day.Day.Equals(env.agenda.getScheduled(7).getDate().Day)))
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        panel.Children.Add(new TextBlock
                        {
                            TextWrapping = TextWrapping.WrapWithOverflow,
                            FontSize = 20,
                            Text = env.agenda.events[7].getrec().getName().ToString(),
                            Margin = new Thickness(5)
                        });
                    }));
                }

            }


            */





        }

        void refresh_GestureRecognized(GestureType arg1, int arg2)
        {
            if (arg1 == GestureType.SwipeLeft)
            {
                Console.WriteLine("SwipeLeft");

                tell++;
                calendar();
            }
            if (arg1 == GestureType.SwipeRight)
            {
                tell--;
                calendar();
                Console.WriteLine("SwipeRight");
               
            }
            if (arg1 == GestureType.SwipeDownLeft)
            {
                Console.WriteLine("SwipeDownLeft");
                // env.intf[0].outputs[2] = true;
            }
            if (arg1 == GestureType.SwipeDownRight)
            {
                Console.WriteLine("SwipeDownRight");
                // env.intf[0].outputs[2] = true;
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
                        if ((env.focused == schedule))
                        {

                            background_change = true;

                            Dispatcher.BeginInvoke((Action)(() => { env.focused = env.menulist[8]; focus_previous(schedule); env.focused.Focus(); focus_next(env.focused); })); ;
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
                        env.focused = env.menulist[3];
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
        void dayOrnight()
        {
            var src = DateTime.Now;
            Console.WriteLine(src.Hour + ":" + src.Minute);
            if ((src.Hour > 18) || (src.Hour < 6)) { env.intf[0].outputs[2] = true; }
            else env.intf[0].outputs[2] = false;

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



    }
}
