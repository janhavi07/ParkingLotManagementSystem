using Microsoft.AspNetCore.Mvc;
using ParkingLotML;
using ParkingLotBL.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotMangamentSystem.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            try
            {
                var result = this.userManager.RegisteUser(user);
                if (result != null)
                {
                    return this.Ok(new { status = "True", message = "User Registered successfully", data = result });
                }
                else
                {
                    return this.NotFound(new { status = "False", message = "User Not registered", data = result });
                }
            }
            catch(Exception)
            {
                return this.BadRequest(new { status = "False", message = "Invalid details sent"});
            }

        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Login login)
        {
            try
            {
                string result = this.userManager.Login(login);
                if (result != null)
                {
                    return this.Ok(new { status = "True", message = " Login successful", data = result });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Login not successful", data = result });
                }
            }catch(Exception)
            {
                return this.BadRequest(new { status = "False", message = "Invalid details sent" });
            }
        }
    }
}
