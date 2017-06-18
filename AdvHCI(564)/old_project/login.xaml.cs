using AmlProject;
using Microsoft.Kinect;
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
using ZXing;


namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class login : UserControl, ISwitchable
    {
        Environment env;
        BarcodeReader barcodeReader;

        private void KinectPressButton(object sender, RoutedEventArgs e)
        {
            String name = ((Microsoft.Kinect.Toolkit.Controls.KinectTileButton)e.OriginalSource).Name;

            switch (name)
            {
                case "loginbutton":
                    ViewSwitcher.Switch(new screen_1_home_logged_in(env));
                    break;
                case "user1":
                    if (!env.user1.isActive())
                    {
                        env.speech.SpeakAsyncCancelAll();
                        env.speech.SpeakAsync("Welcome " + env.user1.getUserName());
                        env.user1.setActive(true);
                        Dispatcher.BeginInvoke((Action)(() => { user1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#58B000"); }));
                    }
                    else
                    {
                        env.speech.SpeakAsyncCancelAll();
                        env.speech.SpeakAsync("Goodbye " + env.user1.getUserName());
                        env.user1.setActive(false);
                        Dispatcher.BeginInvoke((Action)(() => { user1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#888888 "); }));

                    }
                    break;
                case "user2":
                    if (!env.user2.isActive())
                    {
                        env.speech.SpeakAsyncCancelAll();
                        env.speech.SpeakAsync("Welcome " + env.user2.getUserName());
                        env.user2.setActive(true);
                    }
                    else
                    {
                        env.speech.SpeakAsyncCancelAll();
                        env.speech.SpeakAsync("Goodbye " + env.user2.getUserName());
                        env.user2.setActive(false);
                    }
                    break;

            }
            refreshData();

        }

        public void refreshData()
        {
            if (env.user1.isActive())
            {
                Dispatcher.BeginInvoke((Action)(() => { user1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#58B000"); }));
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() => { user1.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#888888 "); }));
            }
            if (env.user2.isActive())
            {
                Dispatcher.BeginInvoke((Action)(() => { user2.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#58B000"); }));
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() => { user2.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#888888 "); }));
            }
        }


        public login(Environment env)
        {
            this.env = env;
            InitializeComponent();

            if (env.using_Kinect == true)
            {
                var regionSensorBinding = new Binding("Kinect")
                {
                    Source =
                        HY564KinectManager.Instance().GetKinectSensorChooser()
                };
                BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
                // HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                // HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.SkeletonStream.Enable();
                // HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorStream.Enable(ColorImageFormat.RawBayerResolution1280x960Fps12);
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady += NewSensor_ColorFrameReady;
                barcodeReader = new BarcodeReader();
                barcodeReader.Options.PossibleFormats = new List<BarcodeFormat> { ZXing.BarcodeFormat.QR_CODE };
            }

            refreshData();
        }




        /// <summary>
        /// Use Kinect's RGB Data to find any existing QR Codes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                        //Dispatcher.BeginInvoke((Action)(() => { LogInf.Content = qr_Result.Text; }));

                        if (words[0] == "User:")
                        {
                            if (words[1] == env.user1.getUserName())
                            {
                                if (!env.user1.isActive())
                                {
                                    env.speech.SpeakAsyncCancelAll();
                                    env.speech.SpeakAsync("Welcome " + env.user1.getUserName());
                                    env.user1.setActive(true);
                                    ViewSwitcher.Switch(new screen_1_home_logged_in(env));
                                }

                            }
                            if (words[1] == env.user2.getUserName())
                            {
                                if (!env.user2.isActive())
                                {
                                    env.speech.SpeakAsyncCancelAll();
                                    env.speech.SpeakAsync("Welcome " + env.user2.getUserName());
                                    env.user2.setActive(true);
                                    ViewSwitcher.Switch(new screen_1_home_logged_in(env));
                                }
                            }
                        }
                    }
                    UpdateKinectFrame(colorFrame);
                }
            }
        }

        private WriteableBitmap _outputBitmap = null;
        private byte[] _pixelData = new byte[0];

        public void UpdateKinectFrame(ColorImageFrame kinectRGB_Input)
        {
            // Checks if the length is 0 (first time we receive a frame)
            if (_pixelData.Length == 0)
            {
                // Creates a buffer long enough to receive all the data of a frame
                _pixelData = new byte[kinectRGB_Input.PixelDataLength];

                // Initialize new WriteableBitmap
                _outputBitmap = new WriteableBitmap(kinectRGB_Input.Width,
                                                    kinectRGB_Input.Height,

                                                    // Standard DPI
                                                    96, 96,

                                                    // Current format for the ColorImageFormat
                                                    PixelFormats.Bgr32,

                                                    // BitmapPalette
                                                    null);

                // Assign the writeablebitmap to the imagecontrol
                KinectImage.Source = _outputBitmap;
            }
            #region Update Kinect Image
            // Copies the data from the frame to the pixelData array
            kinectRGB_Input.CopyPixelDataTo(_pixelData);

            // Update the writeable bitmap
            _outputBitmap.WritePixels(

                // Represents the size of our image
                new Int32Rect(0, 0, kinectRGB_Input.Width, kinectRGB_Input.Height),

                // Our image data
                _pixelData,

                // Stride - How much bytes are there in a single row?
                kinectRGB_Input.Width * kinectRGB_Input.BytesPerPixel,

                // Offset for the buffer, where does he need to start
                0);
            #endregion
        }




        public void UtilizeState(object state)
        {
        }

        public void Destroy()
        {
            if (env.using_Kinect == true)
            {
                HY564KinectManager.Instance().GetKinectSensorChooser().Kinect.ColorFrameReady -= NewSensor_ColorFrameReady;
            }
        }
    }
}
