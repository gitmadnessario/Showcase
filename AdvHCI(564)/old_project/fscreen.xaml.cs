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

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class fscreen : UserControl, ISwitchable
    {
        public fscreen()
        {
            InitializeComponent();
            var regionSensorBinding = new Binding("Kinect")
            {
                Source =
                    HY564KinectManager.Instance().GetKinectSensorChooser()
            };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized += fscreen_GestureRecognized;

        }
        void fscreen_GestureRecognized(GestureType arg1, int arg2)
        {
            if (arg1 == GestureType.JoinedHands)
                ViewSwitcher.Switch(new fscreen());
        }


        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {

            HY564KinectManager.Instance().GetGestureGenerator().GestureRecognized -= fscreen_GestureRecognized;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewSwitcher.Switch(new fscreen());
        }
    }
}
