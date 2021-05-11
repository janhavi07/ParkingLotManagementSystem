using System;
using System.Collections.Generic;
using System.Text;
using ParkingLotML;


namespace ParkingLotRL.IRepository
{
    public interface IParkingRepository
    {
        Parking ParkVehicle(Parking parking);
        bool UnParkVehicle(int parkingId,int userId);
        List<int> GetEmptySlots();
        bool DeleteEmptySlot();
        ParkingResponse SearchVehicleByVehicleNumber(string vehicleNumber);
        List<ParkingResponse> GetAllParkedVehicles();

    }

}
