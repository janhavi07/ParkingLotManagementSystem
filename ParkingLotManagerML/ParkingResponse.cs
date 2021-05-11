using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotML
{
    public class ParkingResponse
    {
        public int ParkingId { get; set; }
        public int UserId { get; set; }

        public string VehicleNumber { get; set; }

        public string ParkingType { get; set; }

        public string RoleType { get; set; }

        public int NoOfWheels { get; set; }

        public string UserEmail { get; set; }

        public string EntryTime { get; set; }

    }
}
