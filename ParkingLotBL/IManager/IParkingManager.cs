﻿using ParkingLotML;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBL.IManager
{
   public interface IParkingManager
    {
        Parking ParkVehicle(Parking parking);
        bool UnParkVehicle(int parkingId);
        List<int> GetEmptySlots();
        bool DeleteEmptySlot();
        Parking SearchVehicleByVehicleNumber(string vehicleNumber);
    }
}