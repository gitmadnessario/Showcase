using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeMiniServer.Beans
{
    /// <summary>
    /// Custom gesture class, used in order to
    /// hold on custom gesture behavior for Kinect.
    /// </summary>
    public class CustomGesture
    {
        //enums for increased readability.
        public static readonly bool SUCCESS = true;
        public static readonly bool FAILURE = false;

        public string               Name        { get; private set; }
        public Func<Skeleton, bool> GestureFunc { get; private set; }

        /// <summary>
        /// The custom gesture constructor
        /// takes as arguments a name, as well as the delegate with the
        /// gesture code.
        /// </summary>
        /// <param name="name">The Gesture Name</param>
        /// <param name="gestureFunc">The Gesture Delegate</param>
        public CustomGesture(string name, Func<Skeleton, bool> gestureFunc)
        {
            Name        = name;
            GestureFunc = gestureFunc;
        }

        /// <summary>
        /// Checks a gesture by executing holding delegate.
        /// </summary>
        /// <param name="skeleton">The skeleton used in the gesture</param>
        /// <returns></returns>

        public bool Check(Skeleton skeleton)
        {
            return GestureFunc(skeleton);
        }
    }
}
