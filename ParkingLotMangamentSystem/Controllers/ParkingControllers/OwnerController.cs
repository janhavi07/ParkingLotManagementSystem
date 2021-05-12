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
    [Authorize(Roles = "owner")]
    public class OwnerController : ControllerBase
    {
        private IParkingManager parkingManager;

        public OwnerController(IParkingManager parkingManager)
        {
            this.parkingManager = parkingManager;
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
                {
                    return this.Ok(new { status = "True", message = "Vehicle parked succesfully", data = parkdetails });
                }
                    return this.BadRequest(new { status = "False", message = "Vehicle not parked", data = parkdetails });
            }catch(Exception)
            {
                return this.BadRequest(new { status = "False", message = "Invalid details sent" });
            }
        }
        [HttpGet]
        [Route("{parkingId}")]
        public IActionResult UnParkVehicle(int parkingId)
        {
            int userId = TokenUserId();
            var unparkDetails = this.parkingManager.UnParkVehicle(parkingId,userId);//return parking id
            if(unparkDetails!=null)
            {
                return this.Ok(new { status = "True", message = "Vehicle Unparked succesfully", data = unparkDetails });
            }
            else
            {
                return this.BadRequest(new { status = "False", message = "Vehicle not Unparked", data = unparkDetails });
            }
        }
        private int TokenUserId()
        {
            return Convert.ToInt32(User.FindFirst("UserId").Value);
        }
    }
}
