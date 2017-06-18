using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Speech.Recognition;
using Newtonsoft.Json;
using OfficeMiniServer.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using OfficeMiniServer.Beans;
using WebSocketsServer.Interfaces;
using Fizbin.Kinect.Gestures;
using Microsoft.Kinect.Toolkit.Controls;

namespace OfficeMiniServer.Handlers
{
    /// <summary>
    /// Kinect Holder class, used for manipulating
    /// kinect data and sending what is needed to the clients.
    /// </summary>
    public class KinectHandler : MessageObserver
    {
        public KinectSensorChooser   SensorChooser          { get; private set; }

        public WebSocketsHandler     Webserver              { get; private set; }

        public GestureGenerator      KinectGestureGenerator { get; private set; }

        private static KinectHandler _kinectHandler;

        private KinectHandler()
        {
            SensorChooser                             = new KinectSensorChooser();
            SensorChooser.KinectChanged              += SensorChooserOnKinectChanged;
            

            SensorChooser.Start();

            KinectGestureGenerator                    = new GestureGenerator();
            KinectGestureGenerator.GestureRecognized += KinectGestureGenerator_GestureRecognized;
            SensorChooser.Kinect.SkeletonFrameReady  += Kinect_SkeletonFrameReady;

            Webserver                                 = WebSocketsHandler.Instance();

            Webserver.Register(this);
        }

        public static KinectHandler Instance()
        {
            if(_kinectHandler == null)
            {
                _kinectHandler = new KinectHandler();
            }
            return _kinectHandler;
        }

        /// <summary>
        /// Fizbin gestures callback.
        /// </summary>
        /// <param name="gestureType">The gesture type</param>
        /// <param name="trackingID">The tracking ID</param>
        void KinectGestureGenerator_GestureRecognized(GestureType gestureType, int trackingID)
        {
            //This will call the KinectInfo constructor,
            //And then assign gestureType string to Gesture property.
            ServerMessage kinectInfo = new ServerMessage()
            {
                Gesture = gestureType.ToString()
            };
            Webserver.SendToClients(kinectInfo.ToJSON());
        }

        void Kinect_SkeletonFrameReady(object sender, Microsoft.Kinect.SkeletonFrameReadyEventArgs e)
        {
            

            ServerMessage info = new ServerMessage();
            info.X          = KinectCursorVisualizer.CurrentXPosition;
            info.Y          = KinectCursorVisualizer.CurrentYPosition;
            info.PressLevel = KinectCursorVisualizer.CurrentPressLevel;
            Console.WriteLine("{0} {1}",info.X, info.Y);
            if (info.X > 0 && info.Y > 0)
            {
                Webserver.SendToClients(info.ToJSON());

            }

            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                        skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    foreach (CustomGesture gesture in CustomGesturesHandler.Instance().Gestures)
                    {
                        if (gesture.Check(skeleton))
                        {
                            ServerMessage gestureInfo = new ServerMessage();
                            gestureInfo.Gesture = gesture.Name;
                            Webserver.SendToClients(gestureInfo.ToJSON());
                        }
                    }
                }
            }
        }





        /// <summary>
        /// Called when the KinectSensorChooser gets a new sensor
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="args">event arguments</param>
        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            // Initialize Gesture Generator
            KinectGestureGenerator.Initialize(args.OldSensor, args.NewSensor);

            // Handle old and new sensor normally..
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                    Console.Error.WriteLine("Sensor came to an invalid state.");
                }
            }

            if (args.NewSensor != null)
            {
                try
                {

                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
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
                    Console.Error.WriteLine("Sensor came to an invalid state.");
                }
            }
        }

        /// <summary>
        /// Notification callback for getting messages from the clients.
        /// </summary>
        /// <param name="message"></param>
        public void Notify(string message)
        {
            ClientMessage clientMessage = JsonConvert.DeserializeObject<ClientMessage>(message);
            List<string> contents       = clientMessage.Contents;

            if (clientMessage.MessageType == "Speech")
            {
                KinectAudioHandler.Initialize(this.SensorChooser.Kinect, contents);
            }
            else if (clientMessage.MessageType == "Light")
            {
                try
                {
                    SensorsHandler.Instance().ChangeLight(Convert.ToBoolean(contents[0]), Convert.ToBoolean(contents[1]), Convert.ToBoolean(contents[2]));
                }
                catch(FormatException)
                {
                    Console.Error.WriteLine("Incorrect format from data received from the clients. Data are ignored.");
                }
            }

            //TODO: You can add more code here for notifying more components using client-side messages properly.
        }
    }
}
