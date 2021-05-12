using ParkingLotBL.IManager;
using ParkingLotML;
using ParkingLotRL.IRepository;
using System;
using System.Collections.Generic;


namespace ParkingLotBL.Manager
{
    public class ParkingManager : IParkingManager
    {
        private IParkingRepository parkingRepository;
        public ParkingManager(IParkingRepository parkingRepository)
        {
            this.parkingRepository = parkingRepository;
        }
        public bool DeleteEmptySlot()
        {
            try
            {
               return this.parkingRepository.DeleteEmptySlot();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<ParkingResponse> GetAllParkedVehicles()
        {
            try
            {
                return this.parkingRepository.GetAllParkedVehicles();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public List<Slot> GetEmptySlots()
        {
            try
            {
                return this.parkingRepository.GetEmptySlots();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ParkingResponse ParkVehicle(Parking parking)
        {
            try
            {
                return this.parkingRepository.ParkVehicle(parking);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ParkingResponse SearchVehicleByVehicleNumber(string vehicleNumber)
        {
            try
            {
                return this.parkingRepository.SearchVehicleByVehicleNumber(vehicleNumber);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public UnparkResponse UnParkVehicle(int parkingId,int userId)
        {
            try
            {
                return this.parkingRepository.UnParkVehicle(parkingId,userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
