using ParkingLotML;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRL.IRepository
{
    public interface IUserRepository
    {
        User RegisterUser(User user);
        string Login(Login login);
    }
}
