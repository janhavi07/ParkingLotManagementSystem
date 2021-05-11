﻿using Microsoft.AspNetCore.Authorization;
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
                Parking parkdetails = this.parkingManager.ParkVehicle(parking);
                if (parkdetails != null)
                {
                    return this.Ok(new { status = "True", message = "Vehicle parked succesfully", data = parking });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Vehicle not parked", data = parkdetails });
                }
            }catch(Exception)
            {
                return this.BadRequest(new { status = "False", message = "Invalid details sent" });
            }
        }
        [HttpGet]
        [Route("Unpark")]
        public IActionResult UnParkVehicle(int parkingId)
        {
            bool unparkDetails = this.parkingManager.UnParkVehicle(parkingId);
            if(unparkDetails)
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
