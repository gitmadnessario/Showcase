using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeMiniServer.Beans
{

    /// <summary>
    /// Bean data class used
    /// for data transferring from the server to clients.
    /// NOTICE: Can be extended for further usage.
    /// </summary>
    public class ServerMessage
    {
        //Kinect info
        public double X                    { get; set; }
        public double Y                    { get; set; }
        public double PressLevel           { get; set; }
        public string SpeechText           { get; set; }
        public string Gesture              { get; set; }

        //Other Sensors info
        public SensorInfo OtherSensorsInfo { get; set; }

        //Sensors

        public ServerMessage() : this(0, 0, 0, "", "", new SensorInfo()) { }

        public ServerMessage(double x,
                             double y,
                             double z,
                             string speechText,
                             string gesture,
                             SensorInfo otherSensorInfo)
        {
            X                = x;
            Y                = y;
            PressLevel       = z;
            SpeechText       = speechText;
            Gesture          = gesture;
            OtherSensorsInfo = otherSensorInfo;
        }

        public string ToJSON()
        {
            //return JsonConvert.SerializeObject(this);
            return "{\"x\":" + this.X             +
                   ",\"y\":" + this.Y             +
                   ",\"z\":" + this.PressLevel    +
                   ", \"speechText\":\""          +
                   this.SpeechText                + 
                   "\", \"gesture\":\""           +
                   this.Gesture                   + 
                   "\", \"sensorInfo\":"          +
                   OtherSensorsInfo.ToJSON() +  "}";
        }
    }
}
