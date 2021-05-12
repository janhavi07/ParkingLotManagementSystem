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
    [Authorize(Roles ="Driver")]
    public class DriverController : ControllerBase
    {
        private IParkingManager parkingManager;

        public DriverController(IParkingManager parkmanager)
        {
            this.parkingManager = parkmanager;
        }

        [HttpPost]
        [Route("Park")]
        public IActionResult ParkVehicle(Parking parking)
        {
            try
            {
                int userId = TokenUserId();
                parking.UserId = userId;
                var parkdetails = this.parkingManager.ParkVehicle(parking);
                if (parkdetails != null)
                    return this.Ok(new { status = "True", message = "Vehicle parked succesfully", data = parkdetails });
                return this.BadRequest(new { status = "False", message = "Vehicle not parked", data = parkdetails });
            }
            catch (Exception)
            {
                return this.BadRequest(new { status = "False", message = "Invalid details sent" });
            }
        }
        [HttpGet]
        [Route("Unpark")]
        public IActionResult UnParkVehicle(int parkingId)
        {
            try
            {
                int userId = TokenUserId();
                var unparkDetails = this.parkingManager.UnParkVehicle(parkingId, userId);
                if (unparkDetails!=null)
                    return this.Ok(new { status = "True", message = "Vehicle Unparked succesfully", data = unparkDetails });
                return this.BadRequest(new { status = "False", message = "Vehicle not Unparked", data = unparkDetails });
            }
            catch
            {
                return this.BadRequest(new { status = "False", message = "Incorrect user" });
            }
        }
        private int TokenUserId()
        {
            return Convert.ToInt32(User.FindFirst("UserId").Value);
        }
    }
}
