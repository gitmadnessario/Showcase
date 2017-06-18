using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeMiniServer.Beans;

namespace OfficeMiniServer.Handlers
{

    /// <summary>
    /// Custom Gestures Holder Singleton class.
    /// </summary>
    public class CustomGesturesHandler
    {
        public List<CustomGesture> Gestures { get; private set; }

        public static CustomGesturesHandler _gestureHandler;

        private CustomGesturesHandler()
        {
            Gestures = new List<CustomGesture>();
        }

        public static CustomGesturesHandler Instance()
        {
            if(_gestureHandler == null)
            {
                _gestureHandler = new CustomGesturesHandler();
            }

            return _gestureHandler;
        }

        public void RegisterCustomGesture(CustomGesture gesture)
        {
            Gestures.Add(gesture);
        }

        public void UnregisterCustomGesture(CustomGesture gesture)
        {
            Gestures.Remove(gesture);
        }

        
    }
}
