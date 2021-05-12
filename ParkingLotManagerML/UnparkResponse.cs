using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotML
{
    public class UnparkResponse
    {
        public string VehicleNumber { get; set; }
        public int ParkedSlot { get; set; }
        public string ExitTime { get; set; }
    }
}
