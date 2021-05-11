using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParkingLotML
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        
        [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public int RoleId { get; set; }

         
    }
}
