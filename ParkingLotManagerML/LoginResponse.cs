using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotML
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
    }
}
