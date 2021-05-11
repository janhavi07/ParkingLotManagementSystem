 using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using ParkingLotML;
using ParkingLotRL.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingLotRL.Repository
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration configuration;
        private OracleConnection oracleConnection;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        private void connection()
        {
            string constr = this.configuration.GetConnectionString("UserDbConnection");
            oracleConnection = new OracleConnection(constr);

        }
        public string Login(Login login)
        {
            try
            {
                connection();
                OracleCommand com = new OracleCommand("sp_login", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@user_email", login.Email);
                com.Parameters.Add("@user_pass", login.Password);
                com.Parameters.Add("@CS", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                oracleConnection.Open();
                DataTable datatable = new DataTable();
                var data = new OracleDataAdapter(com);
                data.Fill(datatable);
                oracleConnection.Close();
                LoginResponse response = new LoginResponse();
                foreach (DataRow row in datatable.Rows)
                {
                    response.UserId = Convert.ToInt32(row["userid"]);
                    response.UserPassword = Convert.ToString(row["password"]);
                    response.UserRole = Convert.ToString(row["roles"]);
                }
                if (response.Equals(null))
                {
                    return null;
                }
                string decryptedPass = Decryptdata(response.UserPassword);
                if (login.Password.Equals(decryptedPass))
                    return GenrateJWTToken(response.UserId, response.UserRole);
                else
                    return null;
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public User RegisterUser(User user)
        {
            try
            {
                connection();
                OracleCommand com = new OracleCommand("sp_register", this.oracleConnection);
                com.CommandType = CommandType.StoredProcedure;
                string password = Encryptdata(user.Password);
                com.Parameters.Add("@Email", user.Email);
                com.Parameters.Add("@Password", password);
                com.Parameters.Add("@RoleId", user.RoleId);
                oracleConnection.Open();
                int rowAffected = com.ExecuteNonQuery();
                oracleConnection.Close();
                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                oracleConnection.Close();
            }

        }
        private static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        private static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }


        private string GenrateJWTToken(int userId, string Role)
        {
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Key"]));
            var signinCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
                        {
                            new Claim("userId", userId.ToString()),
                            new Claim(ClaimTypes.Role, Role),
                        };
            var tokenOptionOne = new JwtSecurityToken(

                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signinCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptionOne);
            return token;
        }
    }
}
