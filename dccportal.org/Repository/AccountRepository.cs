using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using dccportal.org.Dto;
using dccportal.org.Entities;
using dccportal.org.Helper;
using dccportal.org.Interface;
using Microsoft.EntityFrameworkCore;

namespace dccportal.org.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public AccountRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsersAccountDto> AuthenticateUser(UsersAccountDto model)
        {
            try
            {
                return await _context.UsersAccounts
                .Where(m =>
                       m.UserName == model.UserName &&
                       m.Password == model.Password &&
                       m.DeptId == model.DeptId)
                .AsNoTracking()
                .ProjectTo<UsersAccountDto>(_mapper.ConfigurationProvider) 
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public  async Task<bool> CheckUserLoginAccountExist(UsersAccountDto Data)
        {
            try
            {
                var userExistInDept = await _context.UsersAccounts.Where(m => m.BelieverId == Data.BelieverId && m.DeptId == Data.DeptId).AnyAsync();
                return userExistInDept;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfUserNameExist(UsersAccountDto Data)
        {
            try
            {
                var userNameExist = await _context.UsersAccounts.Where(m => m.UserName.ToLower() == Data.UserName.ToLower()).AnyAsync();
                return userNameExist;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> CreateUserLogin(UsersAccountDto data)
        {
            try
            {
                 
                data.CreationDate = DateTime.Now;
                var model  = _mapper.Map<UsersAccountDto, UsersAccount>(data);
                _context.UsersAccounts.Add(model);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}