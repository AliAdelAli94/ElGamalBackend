using ElGamal.BL.Interfaces;
using ElGamal.BL.Utils;
using ElGamal.DAL.DTOs;
using ElGamal.DAL.Entities;
using ElGamal.DAL.UOF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.BL.Classes
{
    public class UserBl : IUserBL
    {
        private IUnitOfWork iUnitOfWork;
        private PasswordEncruption passwordEncruption;

        public UserBl(IUnitOfWork iUOF)
        {
            this.iUnitOfWork = iUOF;
            this.passwordEncruption = new PasswordEncruption();
        }
        //public bool RegisterUser(string useName, string email, string password)
        //{
        //    try
        //    {
        //        user item = new user();
        //        password = passwordEncruption.Encrypt(password);

        //        item.id = Guid.NewGuid();
        //        item.username = useName;
        //        item.email = email;
        //        item.password = password;

        //        this.iUnitOfWork.UserRepository.Insert(item);
        //        this.iUnitOfWork.Save();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public bool CheckUserExist(string email)
        //{
        //    try
        //    {
        //        user item = this.iUnitOfWork.UserRepository.Get(u => u.email == email).FirstOrDefault();
        //        if (item == null)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public UserDTO Login(LoginDTO item)
        {
            try
            {
                item.password = passwordEncruption.Encrypt(item.password);
                User currentUser = this.iUnitOfWork.UserRepository.Get(u => u.email == item.email && u.password == item.password).FirstOrDefault();
                if (item == null)
                {
                    return null;
                }
                return new UserDTO()
                {
                    ID = currentUser.ID,
                    userName = currentUser.userName,
                    address = currentUser.address,
                    phoneNumber = currentUser.phoneNumber,
                    email = currentUser.email,
                    birthDate = currentUser.birthDate
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
