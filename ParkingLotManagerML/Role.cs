using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParkingLotML
{
    public class Role
    {
        [Required]
        public int RoleId { get; set; }
        public string RoleType { get; set; }
    }
}
