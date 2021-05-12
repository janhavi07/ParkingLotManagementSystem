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
                oracleConnection.Open();
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

        public List<Slot> GetEmptySlots()
        {
            try
            {
                List<Slot> emptySlots = new List<Slot>();
                connection();
                OracleCommand com = new OracleCommand("sp_getSlots", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                oracleConnection.Open();
                com.ExecuteNonQuery();
                OracleDataReader reader = com.ExecuteReader();
                if (!reader.HasRows) return null;
                while (reader.Read())
                {
                    emptySlots.Add(
                        new Slot
                        {
                            SlotNumber = reader.GetInt32(0)
                        });
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

        public ParkingResponse ParkVehicle(Parking parking)
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
                int result = com.ExecuteNonQuery();
                oracleConnection.Close();
                if (result != 0)
                {
                    return SearchVehicleByVehicleNumber(parking.VehicleNumber);
                }
                else
                    return null;
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
                oracleConnection.Open();
                com.ExecuteNonQuery();
                
                OracleDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        parkingResponse.ParkingId = reader.GetInt32(0);
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

        public UnparkResponse UnParkVehicle(int parkingId, int userId)
        {
            try
            {
                UnparkResponse unpark = new UnparkResponse();
                connection();
                OracleCommand com = new OracleCommand("sp_UnparkVehicle", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@parking_number", parkingId);
                com.Parameters.Add("@user_id", userId);
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                oracleConnection.Open();
                OracleDataReader reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        unpark.VehicleNumber = reader.GetString(0);
                        unpark.ParkedSlot = reader.GetInt32(1);
                        unpark.ExitTime = reader.GetString(2);
                    }
                    return unpark;
                }
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

        public List<ParkingResponse> GetAllParkedVehicles()
        {
            try
            {
                connection();
                List<ParkingResponse> parkingList = new List<ParkingResponse>();
                OracleCommand com = new OracleCommand("sp_getAllParkedVehicles", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("Cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                OracleDataAdapter adapter = new OracleDataAdapter(com);
                DataTable table = new DataTable();
                oracleConnection.Open();
                adapter.Fill(table);
                oracleConnection.Close();
                if (table.Rows.Count.Equals(0))
                    return null;
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
