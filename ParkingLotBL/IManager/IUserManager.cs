using ParkingLotML;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBL.IManager
{
    public interface IUserManager
    {
        User RegisteUser(User user);
        string Login(Login login);
    }
}
