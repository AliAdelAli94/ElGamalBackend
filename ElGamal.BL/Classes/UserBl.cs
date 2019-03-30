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
        public int RegisterUser(RegisterDTO item)
        {
            try
            {
                if(this.iUnitOfWork.UserRepository.Get(x => x.email == item.email).Count() > 0)
                {
                    return 1;
                }
                else
                {
                    User currentUser = new User()
                    {
                        ID = Guid.NewGuid(),
                        birthDate = item.birthDate,
                        address = item.address,
                        email = item.email,
                        password = passwordEncruption.Encrypt(item.password),
                        userName = item.userName,
                        phoneNumber = item.phoneNumber
                    };
                    this.iUnitOfWork.UserRepository.Insert(currentUser);
                    this.iUnitOfWork.Save();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public UserDTO Login(LoginDTO item)
        {
            try
            {
                item.password = passwordEncruption.Encrypt(item.password);
                User currentUser = this.iUnitOfWork.UserRepository.Get(u => u.email == item.email && u.password == item.password).FirstOrDefault();
                if (currentUser == null)
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
                    birthDate = currentUser.birthDate.ToString()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            try
            {
                return this.iUnitOfWork.UserRepository.Get().Select(x => new UserDTO
                {
                    ID = x.ID,
                    address = x.address,
                    birthDate = (x.birthDate.HasValue) ? ((DateTime)x.birthDate).ToString("dd/MM/yyyy") : null,
                    email = x.email,
                    phoneNumber = x.phoneNumber,
                    userName = x.userName
                }).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int DeleteUser(Guid userID)
        {
            try
            {
                this.iUnitOfWork.UserRepository.Delete(userID);
                this.iUnitOfWork.Save();
                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

    }
}
