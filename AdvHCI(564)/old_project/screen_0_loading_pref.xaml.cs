using AmlProject;
using Fizbin.Kinect.Gestures;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class screen_0_loading_pref : UserControl, ISwitchable
    {




        Environment env;

                 

        public screen_0_loading_pref(Environment env)
        {
            
            this.env = env;
            InitializeComponent();
        
           

        var regionSensorBinding = new Binding("Kinect")
        {
            Source =
                HY564KinectManager.Instance().GetKinectSensorChooser()
        };
        BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty,regionSensorBinding);

        HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized += screen_0_loading_pref_GestureRecognized;

        }
        void screen_0_loading_pref_GestureRecognized(GestureType arg1, int arg2)
        {
            if (arg1 == GestureType.JoinedHands)
                ViewSwitcher.Switch(new screen_5_fridge(env));
        }


        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {

            HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized -= screen_0_loading_pref_GestureRecognized;

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
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "shopping":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
                    break;
                case "schedule":
                    ViewSwitcher.Switch(new screen_0_notInIntermediate(env));
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
            ViewSwitcher.Switch(new screen_5_fridge(env));
        }
    }
}
