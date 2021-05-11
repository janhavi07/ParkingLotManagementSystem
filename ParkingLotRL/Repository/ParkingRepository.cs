using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using ParkingLotML;
using ParkingLotRL.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ParkingLotRL.Repository
{
    public class ParkingRepository : IParkingRepository
    {
        private IConfiguration configuration;
        private OracleConnection oracleConnection;

        public ParkingRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        private void connection()
        {
            string constr = this.configuration.GetConnectionString("UserDbConnection");
            oracleConnection = new OracleConnection(constr);

        }
        public bool DeleteEmptySlot()
        {
            try
            {
                connection();
                OracleCommand com = new OracleCommand("sp_deleteUnparkVehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                int result = com.ExecuteNonQuery();
                oracleConnection.Close();
                if (result != 0)
                    return true;
                else
                    return false;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> GetEmptySlots()
        {
            try
            {
                List<int> emptySlots = new List<int>();
                connection();
                OracleCommand com = new OracleCommand("sp_getAllEmptySlots", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();
                OracleDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    emptySlots.Add(reader.GetInt32(1));
                }
                return emptySlots;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Parking ParkVehicle(Parking parking)
        {
            try
            {
                connection();
                OracleCommand com = new OracleCommand("sp_parkvehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@vehicle_number", parking.VehicleNumber);
                com.Parameters.Add("@parking_type", parking.ParkingType);
                com.Parameters.Add("@vehicle_type", parking.VehicleType);
                com.Parameters.Add("@user_id", parking.UserId);
                com.Parameters.Add("@parking_slot", parking.ParkingSlot);
                com.Parameters.Add("@is_disabled", parking.IsDisabled);
                oracleConnection.Open();
                int rowAffected = com.ExecuteNonQuery();
                oracleConnection.Close();
                if (rowAffected != 0) { return parking; }
                else return null;
                
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public Parking SearchVehicleByVehicleNumber(string vehicleNumber)
        {
            try
            {
                Parking parking = new Parking();
                connection();
                OracleCommand com = new OracleCommand("sp_searchVehicleByNumber", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@VehicleNumber", parking.VehicleNumber);
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();
                OracleDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    parking.ParkingId = reader.GetInt32(0);
                    parking.VehicleNumber = reader.GetString(8);
                    parking.EntryTime = reader.GetString(1);
                    parking.ParkingType = reader.GetInt32(2);
                    parking.VehicleType = reader.GetInt32(3);
                    parking.UserId = reader.GetInt32(4);
                    parking.ParkingSlot = reader.GetInt32(5);
                    parking.IsDisabled = reader.GetChar(6);
                    parking.ExitTime = reader.GetString(7);
                }
                return parking;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool UnParkVehicle(int parkingId)
        {
            try
            {
                Parking parking = new Parking();
                connection();
                OracleCommand com = new OracleCommand("sp_UnparkVehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@parking_number", parkingId);
                oracleConnection.Open();
                int rowAffected = com.ExecuteNonQuery();
                if (rowAffected != 0)
                    return true;
                else
                    return false;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public Parking GetAllParkingVehicles()
        {
            throw new NotImplementedException();
        }
    }
}
