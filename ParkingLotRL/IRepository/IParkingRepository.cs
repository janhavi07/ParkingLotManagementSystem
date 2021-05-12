using System;
using System.Collections.Generic;
using System.Text;
using ParkingLotML;


namespace ParkingLotRL.IRepository
{
    public interface IParkingRepository
    {
        ParkingResponse ParkVehicle(Parking parking);
        UnparkResponse UnParkVehicle(int parkingId,int userId);
        List<Slot> GetEmptySlots();
        bool DeleteEmptySlot();
        ParkingResponse SearchVehicleByVehicleNumber(string vehicleNumber);
        List<ParkingResponse> GetAllParkedVehicles();

    }

}
