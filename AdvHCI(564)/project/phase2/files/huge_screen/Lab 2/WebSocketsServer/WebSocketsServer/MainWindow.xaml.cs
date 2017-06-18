using Fleck;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
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

//Make sure you have the speech SDK installed
//go to add reference, browse, navigate to program files, micrsoft SDKs
//speech, assemblies and select speech.dll
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using System.IO;
using Newtonsoft.Json;
using WebSocketsServer.Interfaces;
using System.Globalization;
using System.Threading;
using OfficeMiniServer.Handlers;
using OfficeMiniServer.Beans;

namespace OfficeMiniServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        KinectHandler MainKinectHandler { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //Setup culture settings
            CultureInfo customCulture                            = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator    = ".";

            Thread.CurrentThread.CurrentCulture = customCulture;

            //Initialize Web and sensors
            WebSocketsHandler.Initialize();
            SensorsHandler.Initialize();

            //Setup kinect settings
            MainKinectHandler = KinectHandler.Instance();

            SensorChooserUI.KinectSensorChooser = MainKinectHandler.SensorChooser;

            var regionSensorBinding = new Binding("Kinect")
            {
                Source = MainKinectHandler.SensorChooser
            };

            //Setup GUI bindings
            BindingOperations.SetBinding(this.MainKinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
            MainKinectRegion.KinectSensor = MainKinectHandler.SensorChooser.Kinect;

            //Setup & Register Custom Gestures

            //Both hands up registered custom gesture.
            CustomGesturesHandler.Instance().RegisterCustomGesture(
                new CustomGesture("BothHandsUp", (skeleton) =>
                {

                    Joint rightHand  = skeleton.Joints[JointType.HandRight];
                    Joint leftHand   = skeleton.Joints[JointType.HandLeft];
                    Joint leftElbow  = skeleton.Joints[JointType.ElbowLeft];
                    Joint rightElbow = skeleton.Joints[JointType.ElbowLeft];
                    Joint head       = skeleton.Joints[JointType.Head];

                    //Hands are in front.
                    if (leftHand.Position.Z < leftElbow.Position.Z && rightHand.Position.Z < rightElbow.Position.Z)
                    {
                        //Hands are over head.
                        return rightHand.Position.Y > head.Position.Y && leftHand.Position.Y > head.Position.Y;
                    }

                    return CustomGesture.FAILURE;
                })
            );

        }


    }

    
}
