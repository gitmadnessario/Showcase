KinectExample.rar, v1.0.2
Changes since the previous version:

**SensorInfo.cs**
1) Added more information about sensors (type, serial number).

**SensorsHandler.js**
1) Passed more info when a sensor interaction occurs.
Note: It is suggested that you inspect this class, in order to understand
how to use each sensor separately. Each sensor is held in a different property,
in case you want to separate and use them easily server-side.

**Client.js**
1) Moved socketUrl from kinectHandler to socketHandler and changed references.
2) Changed printed data from sensor.
3) Modified lights example - check what
is the index of each of your sensors in order
to use them appropriately.