using ElGamal.DAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Interfaces
{
    public interface IUserBL
    {
        UserDTO Login(LoginDTO item);

        List<UserDTO> GetAllUsers(string role);

        int RegisterUser(RegisterDTO item);

        int DeleteUser(Guid userID);

        int EditUser(UserDTO data);



    }
}
