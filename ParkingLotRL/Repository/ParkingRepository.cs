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
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
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
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
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

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public ParkingResponse SearchVehicleByVehicleNumber(string vehicleNumber)
        {
            try
            {
                ParkingResponse parkingResponse = new ParkingResponse();
                connection();
                OracleCommand com = new OracleCommand("sp_searchVehicleByNumber", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@vehicle_number", vehicleNumber);
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                com.ExecuteNonQuery();
                OracleDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    parkingResponse.ParkingId= reader.GetInt32(0);
                    parkingResponse.UserId = reader.GetInt32(1);
                    parkingResponse.VehicleNumber = reader.GetString(2);
                    parkingResponse.ParkingType = reader.GetString(3);
                    parkingResponse.RoleType = reader.GetString(4);
                    parkingResponse.NoOfWheels = reader.GetInt32(5);
                    parkingResponse.UserEmail = reader.GetString(6);
                    parkingResponse.EntryTime = reader.GetString(7);
                    
                }
                return parkingResponse;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public bool UnParkVehicle(int parkingId, int userId)
        {
            try
            {
                Parking parking = new Parking();
                connection();
                OracleCommand com = new OracleCommand("sp_UnparkVehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@parking_number", parkingId);
                com.Parameters.Add("@user_id", userId);
                oracleConnection.Open();
                int rowAffected = com.ExecuteNonQuery();
                if (rowAffected != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw new Exception(message: "Incorrect user_id");
            }
            finally
            {
                oracleConnection.Close();
            }
        }

        public List<ParkingResponse> GetAllParkedVehicles()
        {
            try
            {
                connection();
                List<ParkingResponse> parkingList = new List<ParkingResponse>();
                OracleCommand com = new OracleCommand("sp_UnparkVehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                OracleDataAdapter adapter = new OracleDataAdapter(com);
                DataTable table = new DataTable();
                oracleConnection.Open();
                adapter.Fill(table);
                oracleConnection.Close();
                foreach (DataRow row in table.Rows)
                {
                    parkingList.Add(
                        new ParkingResponse
                        {
                            ParkingId = Convert.ToInt32(row["parking_id"]),
                            UserId = Convert.ToInt32(row["user_id"]),
                            VehicleNumber = Convert.ToString(row["vehiclenumber"]),
                            ParkingType = Convert.ToString(row["type"]),
                            RoleType = Convert.ToString(row["roles"]),
                            NoOfWheels = Convert.ToInt32(row["wheels"]),
                            UserEmail = Convert.ToString(row["email"]),
                            EntryTime = Convert.ToString(row["entry_time"])
                        }
                        );
                }
                return parkingList;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
