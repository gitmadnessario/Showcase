using OfficeMiniServer.Beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OfficeMiniServer.Handlers
{

    public class SensorsHandler
    {

        public Phidgets.Manager      PhidgetsManager { get; set; }

        //Kits are held in separate variables here, for easy use,
        //Although they all use the same callback for sensor changes here.
        public Phidgets.InterfaceKit LinearKit       { get; set; }
        public Phidgets.InterfaceKit InterfaceKit    { get; set; }
        public Phidgets.InterfaceKit RotaryKit       { get; set; }
        public Phidgets.InterfaceKit Linear2Kit      { get; set; }

        public int Linear1SerialNumber               { get; private set; }
        public int Linear2SerialNumber               { get; private set; }

        private static SensorsHandler _sensorManager;

        public static SensorsHandler Instance()
        {
            if(_sensorManager == null)
            {
                throw new InvalidOperationException("Sensors Handler not initialized.");
            }
            return _sensorManager;
        }


        public static SensorsHandler Initialize()
        {
            return _sensorManager = new SensorsHandler();
        }

        private SensorsHandler()
        {
            PhidgetsManager = new Phidgets.Manager();
            PhidgetsManager.open();
            Thread.Sleep(1000);
            LinearKit       = new Phidgets.InterfaceKit();
            Linear2Kit      = new Phidgets.InterfaceKit();
            InterfaceKit    = new Phidgets.InterfaceKit();
            RotaryKit       = new Phidgets.InterfaceKit();

            Linear1SerialNumber = 65464;    // Vertical
            Linear2SerialNumber = 65462;    // Horizontal

            foreach (var device in PhidgetsManager.Devices)
            {
                Phidgets.InterfaceKit phidgetsInterfaceKit = device as Phidgets.InterfaceKit;
                
                Console.WriteLine("Kit type: {0}, serial {1}", phidgetsInterfaceKit.ID, phidgetsInterfaceKit.SerialNumber);

                if (phidgetsInterfaceKit.ID == Phidgets.Phidget.PhidgetID.LINEAR_TOUCH)
                {
                    if (phidgetsInterfaceKit.SerialNumber == Linear1SerialNumber)
                    {
                        LinearKit.open(phidgetsInterfaceKit.SerialNumber);
                        LinearKit.SensorChange += Kit_SensorChange;
                    }
                    else 
                    {
                        Linear2Kit.open(phidgetsInterfaceKit.SerialNumber);
                        Linear2Kit.SensorChange += Kit_SensorChange;
                    }

                }
                else if(phidgetsInterfaceKit.ID == Phidgets.Phidget.PhidgetID.ROTARY_TOUCH)
                {
                    RotaryKit.open(phidgetsInterfaceKit.SerialNumber);
                    RotaryKit.SensorChange += Kit_SensorChange;
                }
                else if (phidgetsInterfaceKit.ID == Phidgets.Phidget.PhidgetID.INTERFACEKIT_8_8_8)
                {
                    InterfaceKit.open(phidgetsInterfaceKit.SerialNumber);
                    InterfaceKit.SensorChange += Kit_SensorChange;
                }
            }
            //End Of Sensors Management
        }

        private void Kit_SensorChange(object sender, Phidgets.Events.SensorChangeEventArgs e)
        {
            Phidgets.InterfaceKit kit = sender as Phidgets.InterfaceKit;
            
            ServerMessage message = new ServerMessage()
            {
                OtherSensorsInfo = new SensorInfo(kit.ID.ToString(), kit.SerialNumber, e.Index, e.Value)
            };
            WebSocketsHandler.Instance().SendToClients(message.ToJSON());
        }


        public void ChangeLight(bool r, bool g, bool b)
        {
            Thread t = new Thread(() =>
            {
                if (!InterfaceKit.Attached)
                {

                    Thread.Sleep(1000);
                }
                else
                {
                    InterfaceKit.outputs[0] = r;
                    InterfaceKit.outputs[1] = g;
                    InterfaceKit.outputs[2] = b;
                }
            }

            );
            t.Start();

        }


        public void Close()
        {
            LinearKit.close();
            InterfaceKit.close();
            Linear2Kit.close();
            RotaryKit.close();
            PhidgetsManager.close();
        }
    }
}
