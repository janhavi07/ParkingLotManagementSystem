using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotML
{
    public class Parking
    {
        public int ParkingId { get; set; }
        public string VehicleNumber { get; set; }
        public int ParkingType { get; set; }
        public int VehicleType { get; set; }
        public int UserId { get; set; }
        public int ParkingSlot { get; set; }
        public char IsDisabled { get; set; }
        public string EntryTime { get; set; }
        public string ExitTime { get; set; }
    }
}
