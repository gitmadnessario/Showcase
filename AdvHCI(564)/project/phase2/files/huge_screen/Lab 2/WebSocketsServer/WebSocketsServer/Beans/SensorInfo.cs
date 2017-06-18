using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeMiniServer.Beans
{
    public class SensorInfo
    {
        public string KitType           { get; set; }
        public int    SerialNumber      { get; set; }
        public int    Index             { get; set; }
        public int    Value             { get; set; }
        public bool   IsValidSensorInfo { get; private set; }

        public SensorInfo(string kitType, int serialNumber, int index, int value) : this(kitType, serialNumber, index, value, true) { }

        public SensorInfo(string kitType      = "",
                          int    serialNumber = 0,
                          int    index        = 0,
                          int    value        = 0,
                          bool   isValidInfo  = false)
        {
            KitType           = kitType;
            SerialNumber      = serialNumber;
            Index             = index;
            Value             = value;
            IsValidSensorInfo = isValidInfo;
        }

        public string ToJSON()
        {
            return "{ \"kitType\": \""        + KitType           +
                   "\", \"serialNumber\": \"" + SerialNumber      +
                   "\", \"index\": \""        + Index             +
                   "\", \"value\": \""        + Value             +
                   "\", \"isValid\": \""      + IsValidSensorInfo +
                   "\" }";
        }
    }
}
