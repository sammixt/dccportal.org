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

    public class BelieverRepository : IBelieverRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public BelieverRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CountBelivers()
        {
           int TotalBelievers = await _context.Believers.CountAsync();
           return Convert.ToString(TotalBelievers);
        }
        public async Task<PagedList<BelieversDto>> GetAllMembers(PaginationParams paginationParams)
        {
            try
            {
                var query = _context.Believers.AsQueryable();
                return await PagedList<BelieversDto>.CreateAsync(query.ProjectTo<BelieversDto>(_mapper
              .ConfigurationProvider).AsNoTracking(),
                  paginationParams.PageNumber, paginationParams.PageSize);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTableDto<BelieversDto>> GetAllMembers(DataTableRequestDto dtRequest)
        {
            try
            {
                var query = _context.Believers.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.FirstName.Contains(dtRequest.SearchValue) 
                                                || m.LastName.Contains(dtRequest.SearchValue) 
                                                || m.PhoneNumber.Contains(dtRequest.SearchValue) 
                                                || m.Sex.Contains(dtRequest.SearchValue) );
                }

                var size = await query.CountAsync();  
  
                var result = await query  
                    .AsNoTracking()  
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .ProjectTo<BelieversDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<BelieversDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }
            catch (Exception ex)
            {
                return  (new DataTableDto<BelieversDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = new List<BelieversDto>(),  
                    RecordsFiltered = 0,  
                    RecordsTotal = 0  
                });
            }
        }

        public async Task<bool> DeleteMember(int memberId)
        {
            bool output = false;
            try{
                var member = await _context.Believers.FirstOrDefaultAsync(x => x.MemberId == memberId );
                if(member != null){
                    _context.Believers.Remove(member);
                  var complete = await _context.SaveChangesAsync();
                  output= true;
                }
            }catch(Exception ex){

            }
            return output;
        }
    
        public async Task<BelieversDto> GetMember(string memberId)
        {
            var memberIdString = Encrypter.Decrypt(memberId,Constants.PASSPHRASE);
            int memberIdInt = Convert.ToInt32(memberIdString);
            var query = _context.Believers.AsQueryable();
            var pQuery = query.ProjectTo<BelieversDto>(_mapper
              .ConfigurationProvider).AsNoTracking();
            var member = await pQuery.FirstOrDefaultAsync(m => m.MemberId == memberIdInt);
            return member;
        }  

        public async Task<int> EditUser(BelieversDto dto)
        {
            try{
            var memberIdString = Encrypter.Decrypt(dto.SetMemberIdString,Constants.PASSPHRASE);
            int memberIdInt = Convert.ToInt32(memberIdString);
            var believer = _mapper.Map<BelieversDto,Believer>(dto);
            var currentBelieverRecord = await _context.Believers.FirstOrDefaultAsync(x => x.MemberId == memberIdInt);
            if(currentBelieverRecord == null) return -1;

            currentBelieverRecord.AltPhoneNumber = believer.AltPhoneNumber ?? currentBelieverRecord.AltPhoneNumber;
            currentBelieverRecord.City = believer.City ?? currentBelieverRecord.City;
            currentBelieverRecord.Country = believer.Country ?? currentBelieverRecord.Country;
            currentBelieverRecord.DateOfBirth = believer.DateOfBirth??currentBelieverRecord.DateOfBirth;
            currentBelieverRecord.Email = believer.Email ?? currentBelieverRecord.Email;
            currentBelieverRecord.FacebookName = believer.FacebookName ?? currentBelieverRecord.FacebookName;
            currentBelieverRecord.FirstName = believer.FirstName ?? currentBelieverRecord.FirstName;
            currentBelieverRecord.HomeAddressOne = believer.HomeAddressOne ?? currentBelieverRecord.HomeAddressOne;
            currentBelieverRecord.HomeAddressTwo = believer.HomeAddressTwo ?? currentBelieverRecord.HomeAddressTwo;
            currentBelieverRecord.InstagramHandle = believer.InstagramHandle ?? currentBelieverRecord.InstagramHandle;
            currentBelieverRecord.LastName = believer.LastName ?? currentBelieverRecord.LastName;
            currentBelieverRecord.MaritalStatus = believer.MaritalStatus ?? currentBelieverRecord.MaritalStatus;
            currentBelieverRecord.PhoneNumber = believer.PhoneNumber ?? currentBelieverRecord.PhoneNumber;
            currentBelieverRecord.Sex = believer.Sex ?? currentBelieverRecord.Sex;
            currentBelieverRecord.StateName = believer.StateName ?? currentBelieverRecord.StateName;
            currentBelieverRecord.TwitterHandle = believer.TwitterHandle ?? currentBelieverRecord.TwitterHandle;
            currentBelieverRecord.WeddingAnniversary = believer.WeddingAnniversary ?? currentBelieverRecord.WeddingAnniversary;
            return await _context.SaveChangesAsync();
            }catch(Exception ex){
                throw;
            }
            
        }

        public async Task<int> CreateUser(BelieversDto dto)
        {
           try{
                var believer = _mapper.Map<BelieversDto,Believer>(dto);
                var userExist = await _context.Believers
                                        .Where(x => x.FirstName.ToLower() == believer.FirstName.ToLower())
                                        .Where(l => l.LastName.ToLower() == believer.LastName.ToLower())
                                        .AnyAsync();
                if(userExist) return -1;
                _context.Believers.Add(believer);
                return await _context.SaveChangesAsync();
           }catch(Exception ex){
               throw;
           }
        }

        public async Task<Tuple<bool,int>> CreateUserViaUpload(BelieversDto dto)
        {
           try{
                var believer = _mapper.Map<BelieversDto,Believer>(dto);
                var userExist = await _context.Believers
                                        .Where(x => x.FirstName.ToLower() == believer.FirstName.ToLower())
                                        .Where(l => l.LastName.ToLower() == believer.LastName.ToLower())
                                        .FirstOrDefaultAsync();
                if(userExist != null) return new Tuple<bool, int>(false,userExist.MemberId) ;
                _context.Believers.Add(believer);
                int isCompleted = await _context.SaveChangesAsync();
                if(isCompleted > 0)
                     return new Tuple<bool, int>(true,believer.MemberId);
                else 
                    return new Tuple<bool, int>(false,-2); ;
           }catch(Exception ex){
               throw;
           }
        }
    }
}