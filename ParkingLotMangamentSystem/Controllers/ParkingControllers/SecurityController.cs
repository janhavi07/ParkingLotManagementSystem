using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingLotBL.IManager;
using ParkingLotML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotMangamentSystem.Controllers.ParkingControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Security")]
    public class SecurityController : ControllerBase
    {
        private IParkingManager parkingManager;

        public SecurityController(IParkingManager parkingManager)
        {
            this.parkingManager = parkingManager;
        }
        [HttpGet]
        [Route("EmptySlots")]
        public IActionResult GetEmptySlots()
        {
            try
            {
                List<Slot> emptySlots = this.parkingManager.GetEmptySlots();
                if (emptySlots != null)
                {
                    return this.Ok(new { status = "True", message = "Empty Slots", data = emptySlots });
                }
                else
                    return this.NotFound(new { status = "False", message = "Slots not available", data = emptySlots });
            }
            catch
            {
                return this.BadRequest(new { status = "False", message = "Error parsing empty slots" });
            }
        }
        [HttpGet]
        [Route("ParkedVehicles")]
        public IActionResult GetAllParkedVehicles()
        {
            try
            {
                var parkingResponses = this.parkingManager.GetAllParkedVehicles();
                if(parkingResponses!=null) return this.Ok(new { status = "True", message = "List of parked vehicles", data = parkingResponses });
                else return this.NotFound(new { status = "False", message = "No Parked vahicles", data = parkingResponses });
            }
            catch
            {
                return this.BadRequest(new { status = "False", message = "Error getting parked vehicles" });
            }
        }
        [HttpGet]
        [Route("{vehicleNumber}")]
        public IActionResult SearchVehicleByNumber(string vehicleNumber)
        {
            try
            {
                var parkingResponse = this.parkingManager.SearchVehicleByVehicleNumber(vehicleNumber);
                if(parkingResponse!=null) return this.Ok(new { status = "True", message = "Vehicle Found", data = parkingResponse });
                else return this.NotFound(new { status = "False", message = "Vehicle Not Found", data = parkingResponse });
            }
            catch
            {
                return this.BadRequest(new { status = "False", message = "Error getting parked vehicles" });
            }
        }
        [HttpDelete]
        public IActionResult DeleteEmptySlots()
        {
            try
            {
                bool result = this.parkingManager.DeleteEmptySlot();
                if (result) return this.Ok(new { status = "True", message = "Empty Slots deleted", data = result });
                else return this.NotFound(new { status = "False", message = "Slots not deleted", data = result });
            }catch
            {
                return this.BadRequest(new { status = "False", message = "Error while deleting slots" });
            }
        }
    }
}
