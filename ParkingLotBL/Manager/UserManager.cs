using ParkingLotBL.IManager;
using ParkingLotML;
using ParkingLotRL.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBL.Manager
{
    public class UserManager : IUserManager
    {
        private IUserRepository userRepository;

        public UserManager(IUserRepository userRepository)
        {
               this.userRepository = userRepository;
        }
        public string Login(Login login)
        {
            try
            {
                return this.userRepository.Login(login);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User RegisteUser(User user)
        {
            try 
            { 
                return this.userRepository.RegisterUser(user);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
}
    }
}
