using Fizbin.Kinect.Gestures;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2
{
    public class HY564KinectManager
    {

        private static HY564KinectManager _gm = null;

        private GestureGenerator gestureGenerator;
        private KinectSensorChooser sensorChooser;


        private HY564KinectManager()
        {
            gestureGenerator = new GestureGenerator();

            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooser.Start();
        }

        public static HY564KinectManager Instance()
        {
            if (_gm == null)
            {
                _gm = new HY564KinectManager();
            }

            return _gm;
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            // Initialize Gesture Generator
            this.gestureGenerator.Initialize(args.OldSensor, args.NewSensor);

            // Handle old and new sensor normally..

            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                    args.OldSensor.ColorStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    //OPTIONAL Change skeleton tracking mode from default to seated if needed
                    args.NewSensor.SkeletonStream.Enable();
                    args.NewSensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);
                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }


        public KinectSensorChooser GetKinectSensorChooser() { return this.sensorChooser; }
        public GestureGenerator GetGestureGenerator() { return this.gestureGenerator; }
    }
}