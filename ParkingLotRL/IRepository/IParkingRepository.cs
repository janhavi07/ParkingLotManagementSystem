using System;
using System.Collections.Generic;
using System.Text;
using ParkingLotML;


namespace ParkingLotRL.IRepository
{
    public interface IParkingRepository
    {
        Parking ParkVehicle(Parking parking);
        bool UnParkVehicle(int parkingId);
        List<int> GetEmptySlots();
        bool DeleteEmptySlot();
        Parking SearchVehicleByVehicleNumber(string vehicleNumber);
        Parking GetAllParkingVehicles();
        
    }

}
