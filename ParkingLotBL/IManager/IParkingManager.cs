using ParkingLotML;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBL.IManager
{
   public interface IParkingManager
    {
        ParkingResponse ParkVehicle(Parking parking);
        UnparkResponse UnParkVehicle(int parkingId,int userId);
        List<Slot> GetEmptySlots();
        bool DeleteEmptySlot();
        ParkingResponse SearchVehicleByVehicleNumber(string vehicleNumber);

        List<ParkingResponse> GetAllParkedVehicles();
    }
}
