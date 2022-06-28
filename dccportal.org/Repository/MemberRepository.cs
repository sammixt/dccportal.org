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

    public class MemberRepository : IMemberRepository
    {

        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public MemberRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CountMembers(){

             int TotalMembers = await _context.Members.Select(b => b.BelieverId).Distinct().CountAsync();
             return Convert.ToString(TotalMembers);
        }

        public async Task<string> CountMembersInDept(int deptId){

             int TotalMembers = await _context.Members.Where(b => b.DeptId == deptId).CountAsync();
             return Convert.ToString(TotalMembers);
        }
        public async Task<int> CountUserDept(string _memberId)
        {
            int count = 0;
             var memberIdString = Encrypter.Decrypt(_memberId,Constants.PASSPHRASE);
            int memberIdInt = Convert.ToInt32(memberIdString);
            try
            {
                count = await _context.Members.Where(b => b.BelieverId == memberIdInt).CountAsync();
                return count;
            }
            catch (Exception ex)
            {
                //ErrorLog.log(ex.Message + "+ -------------------------------- + " + ex.StackTrace);
                return count;
            }
        }

        public async Task<List<MemberDepartmentDto>> GetMemberDetails(string _memberId)
        {
            try
            {
                 var memberIdString = Encrypter.Decrypt(_memberId,Constants.PASSPHRASE);
                int memberIdInt = Convert.ToInt32(memberIdString);
                var query = await _context.Members.Include(p => p.Dept).Include(d => d.Unit).DefaultIfEmpty().Where(m => m.BelieverId == memberIdInt)
                .Select(x => new MemberDepartmentDto()
                {
                    MemberId = x.MemberId,
                    DepartmentName = x.Dept.DeptName,
                    UnitName = x.Unit != null ? x.Unit.UnitName : null,
                    Group = x.Groups,
                    Status = x.Status,
                    Post = x.Post,
                    DeptId = (int)x.DeptId

                }).ToListAsync();
                query.ForEach(x => x.Post =  GetPostName(x.Post));
                return query;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetPostName(string postId)
        {
           if(string.IsNullOrEmpty(postId)) return null;
            var query =  _context.Posts.FirstOrDefault(x => x.PostId.Equals(postId));
            return query?.PostName;

        }

        public async Task<int> InsertMember(MemberDto memberDto)
        {
            try{
                 var believerIdString = Encrypter.Decrypt(memberDto.BelieverIdString,Constants.PASSPHRASE);
                memberDto.BelieverId = Convert.ToInt32(believerIdString);
                var member = _mapper.Map<MemberDto, Member>(memberDto);
                bool isInDept = await _context.Members
                .Where(x => x.BelieverId ==  memberDto.BelieverId)
                .Where(d => d.DeptId == memberDto.DeptId)
                .AnyAsync();
                if(isInDept) return -1;
                _context.Members.Add(member);
               return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> addUserToDept(int UserId, int DeptId)
        {
            try{
                bool memberExist = await _context.Members
                                    .Where(m => m.BelieverId == UserId)
                                    .Where(d => d.DeptId == DeptId)
                                    .AnyAsync();
                if(memberExist) return -1;
                Member member = new Member();
                member.BelieverId = UserId;
                member.DeptId = DeptId;
                _context.Members.Add(member);
               return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> AddMemberToUnit(MemberDto member)
        {
            var user = await _context.Members
                .Where(x => x.MemberId ==  member.MemberId)
                .FirstOrDefaultAsync();
            if(member == null) return -1;

            user.UnitId = member.UnitId;
            user.Unit = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == member.UnitId);
            user.Post = member.Post;
            user.Groups = member.Groups;
            user.Status = member.Status;

             return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteMember(MemberDepartmentDto memberDto)
        {
            try{
                 var memberIdString = Encrypter.Decrypt(memberDto.SetMemberIdString,Constants.PASSPHRASE);
                int memberId = Convert.ToInt32(memberIdString);
                var member = await _context.Members
                .Where(x => x.MemberId ==  memberId)
                .FirstOrDefaultAsync();
                if(member == null) return -1;
                _context.Members.Remove(member);
               return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTableDto<BelieversDto>> GetMembersInDept(DataTableRequestDto dtRequest,string dept)
        {
            try{
                var deptIdString = Encrypter.Decrypt(dept,Constants.PASSPHRASE);
                int deptIdInt = Convert.ToInt32(deptIdString);
                
                var query = _context.Members.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.Believer.FirstName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.LastName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.PhoneNumber.Contains(dtRequest.SearchValue) 
                                                || m.Believer.Sex.Contains(dtRequest.SearchValue) );
                }

                var size = await query.Where(x => x.DeptId == deptIdInt).CountAsync();  
  
                var result = await query 
                .Where(x => x.DeptId == deptIdInt)
                    .AsNoTracking()  
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .Select(x => new BelieversDto(){
                        FirstName = x.Believer.FirstName,
                        LastName = x.Believer.LastName,
                        Sex = x.Believer.Sex,
                        DateOfBirth = x.Believer.DateOfBirth,
                        PhoneNumber = x.Believer.PhoneNumber,
                        MemberId = x.Believer.MemberId,
                        DeptId = (int)x.DeptId,
                        MemberInDeptId = x.MemberId
                    })
                    //.ProjectTo<BelieversDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<BelieversDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }catch (Exception ex)
            {
                throw;
            }
             
        }

        public async Task<DataTableDto<BelieversDto>> GetMembersInDept(DataTableRequestDto dtRequest,int dept)
        {
            try{
                
                var query = _context.Members.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.Believer.FirstName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.LastName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.PhoneNumber.Contains(dtRequest.SearchValue) 
                                                || m.Believer.Sex.Contains(dtRequest.SearchValue) );
                }

                var size = await query.Where(x => x.DeptId == dept).CountAsync();  
  
                var result = await query 
                .Where(x => x.DeptId == dept)
                    .AsNoTracking()  
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .Select(x => new BelieversDto(){
                        FirstName = x.Believer.FirstName,
                        LastName = x.Believer.LastName,
                        Sex = x.Believer.Sex,
                        DateOfBirth = x.Believer.DateOfBirth,
                        PhoneNumber = x.Believer.PhoneNumber,
                        MemberId = x.Believer.MemberId,
                        DeptId = (int)x.DeptId,
                        MemberInDeptId = x.MemberId
                    })
                    //.ProjectTo<BelieversDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<BelieversDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }catch (Exception ex)
            {
                throw;
            }
             
        }

        public async Task<DataTableDto<BelieversDto>> GetMembersInUnit(DataTableRequestDto dtRequest,int dept,string unitId)
        {
            try{
                 var unitIdString = Encrypter.Decrypt(unitId,Constants.PASSPHRASE);
                int unitIdInt = Convert.ToInt32(unitIdString);

                var query = _context.Members.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.Believer.FirstName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.LastName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.PhoneNumber.Contains(dtRequest.SearchValue) 
                                                || m.Believer.Sex.Contains(dtRequest.SearchValue) );
                }

                var size = await query.Where(x => x.DeptId == dept && x.UnitId == unitIdInt).CountAsync();  
  
                var result = await query 
                .Where(x => x.DeptId == dept && x.UnitId == unitIdInt)
                    .AsNoTracking()  
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .Select(x => new BelieversDto(){
                        FirstName = x.Believer.FirstName,
                        LastName = x.Believer.LastName,
                        Sex = x.Believer.Sex,
                        DateOfBirth = x.Believer.DateOfBirth,
                        PhoneNumber = x.Believer.PhoneNumber,
                        MemberId = x.Believer.MemberId,
                        DeptId = (int)x.DeptId,
                        MemberInDeptId = x.MemberId
                    })
                    //.ProjectTo<BelieversDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<BelieversDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }catch (Exception ex)
            {
                throw;
            }
             
        }

        public async Task<DataTableDto<AllMembersDto>> GetMembers(DataTableRequestDto dtRequest)
        {
            try{
                
                var query = _context.Members.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.Believer.FirstName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.LastName.Contains(dtRequest.SearchValue) 
                                                || m.Believer.PhoneNumber.Contains(dtRequest.SearchValue) 
                                                || m.Believer.Sex.Contains(dtRequest.SearchValue) );
                }

                var size = await query.CountAsync();  
  
                var result = await query 
                    .AsNoTracking()
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .Select(x => new AllMembersDto(){
                        FirstName = x.Believer.FirstName,
                        LastName = x.Believer.LastName,
                        Sex = x.Believer.Sex,
                        DateOfBirth = x.Believer.DateOfBirth,
                        PhoneNumber = x.Believer.PhoneNumber,
                        MemberId = x.Believer.MemberId,
                        DeptId = (int)x.DeptId,
                        MemberInDeptId = x.MemberId,
                        Department = x.Dept.DeptName,
                        Unit = x.Unit!=null?x.Unit.UnitName:null
                    })
                    //.ProjectTo<BelieversDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<AllMembersDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }catch (Exception ex)
            {
                throw;
            }
             
        }

    }
}