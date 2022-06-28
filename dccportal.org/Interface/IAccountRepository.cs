using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Entities;

namespace dccportal.org.Interface
{
     public interface IAccountRepository
    {
    
        Task<UsersAccountDto> AuthenticateUser(UsersAccountDto model);
        Task<bool> CheckIfUserNameExist(UsersAccountDto Data);
        Task<bool> CheckUserLoginAccountExist(UsersAccountDto Data);
        Task<int> CreateUserLogin(UsersAccountDto data);
    }
}