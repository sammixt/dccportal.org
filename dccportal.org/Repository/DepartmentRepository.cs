using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using dccportal.org.Dto;
using dccportal.org.Interface;
using dccportal.org.Entities;
using Microsoft.EntityFrameworkCore;
using dccportal.org.Helper;

namespace dccportal.org.Repository
{

    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public DepartmentRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CountDepts()
        {
            int TotalDepts = await _context.Departments.CountAsync();
            return Convert.ToString(TotalDepts);
        }
        public async Task<List<DepartmentDto>> GetAllDeptsAsync()
        {
            var query = _context.Departments.AsQueryable();
            var query2 = _context.Departments.ToList();
            var pQuery = query.ProjectTo<DepartmentDto>(_mapper
              .ConfigurationProvider).AsNoTracking();
            var departments = await pQuery.ToListAsync();
            return departments;
        }

        public async Task<DataTableDto<DepartmentDto>> GetAllDepartments(DataTableRequestDto dtRequest)
        {
            try
            {
                var query = _context.Departments.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.DeptName.Contains(dtRequest.SearchValue) 
                                                || m.DeptDesc.Contains(dtRequest.SearchValue) 
                                                || m.ShortCode.Contains(dtRequest.SearchValue));
                }

                var size = await query.CountAsync();  
  
                var result = await query  
                    .AsNoTracking()  
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize)  
                    .ProjectTo<DepartmentDto>(_mapper.ConfigurationProvider)  
                    .ToArrayAsync();  

                return  (new DataTableDto<DepartmentDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }
            catch (Exception ex)
            {
                return  (new DataTableDto<DepartmentDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = new List<DepartmentDto>(),  
                    RecordsFiltered = 0,  
                    RecordsTotal = 0  
                });
            }
        }

        public async Task<DataTableDto<DepartmentDashbordDto>> GetAllDepartmentsDashboard(DataTableRequestDto dtRequest)
        {
            try
            {
                var query = _context.Departments.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.DeptName.Contains(dtRequest.SearchValue) );
                }

                var size = await query.CountAsync();  
  
                var result = await query  
                    .AsNoTracking()  
                    .Include(x => x.Members)
                    .Include(d => d.Units)
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)  
                    .Take(dtRequest.PageSize) 
                    .Select(y => new DepartmentDashbordDto(){
                        DeptName = y.DeptName,
                        Members = y.Members.Count(),
                        Units = y.Units.Count()
                    })
                    .ToArrayAsync();  

                return  (new DataTableDto<DepartmentDashbordDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = result,  
                    RecordsFiltered = size,  
                    RecordsTotal = size  
                });
            }
            catch (Exception ex)
            {
                return  (new DataTableDto<DepartmentDashbordDto> {  
                    Draw = Convert.ToInt32(dtRequest.Draw),  
                    Data = new List<DepartmentDashbordDto>(),  
                    RecordsFiltered = 0,  
                    RecordsTotal = 0  
                });
            }
        }
        public async Task<int> CreateDepartment(DepartmentDto dto)
        {
           try{
                var department = _mapper.Map<DepartmentDto,Department>(dto);
                var deptExist = await _context.Departments
                                        .Where(x => x.DeptName.ToLower() == department.DeptName.ToLower())
                                        .AnyAsync();
                if(deptExist) return -1;
                _context.Departments.Add(department);
                return await _context.SaveChangesAsync();
           }catch(Exception ex){
               throw;
           }
        }

        public async Task<DepartmentDto> GetDepartment(string department)
        {
            try{
                 var deptIdString = Encrypter.Decrypt(department,Constants.PASSPHRASE);
                int deptIdInt = Convert.ToInt32(deptIdString);
                  var query = _context.Departments.AsQueryable();
                  var pQuery = query.ProjectTo<DepartmentDto>(_mapper
              .ConfigurationProvider).AsNoTracking();
            var departments = await pQuery.FirstOrDefaultAsync(x => x.DeptId == deptIdInt);
            return departments;
            }catch(Exception ex){
               throw;
           }
           
        }

        public async Task<int> EditDept(DepartmentDto dto)
        {
            try{
            var deptIdString = Encrypter.Decrypt(dto.SetDeptIdString,Constants.PASSPHRASE);
            int deptIdInt = Convert.ToInt32(deptIdString);
            dto.DeptId = deptIdInt;
            var department = _mapper.Map<DepartmentDto,Department>(dto);
            var currentDepartment = await _context.Departments.FirstOrDefaultAsync(x => x.DeptId == deptIdInt);
            if(currentDepartment == null) return -1;

            currentDepartment.DeptName = department.DeptName ?? currentDepartment.DeptName;
            currentDepartment.DeptDesc = department.DeptDesc ?? currentDepartment.DeptDesc;
            currentDepartment.Vision = department.Vision ?? currentDepartment.Vision;
            currentDepartment.ShortCode = department.ShortCode??currentDepartment.ShortCode;
            
            return await _context.SaveChangesAsync();
            }catch(Exception ex){
                throw;
            }
            
        }

        public async Task<bool> DeleteDepartment(string deptId)
        {
            bool output = false;
            try{
                var deptIdString = Encrypter.Decrypt(deptId,Constants.PASSPHRASE);
                int deptIdInt = Convert.ToInt32(deptIdString);
                var department = await _context.Departments.FirstOrDefaultAsync(x => x.DeptId == deptIdInt);
                if(department != null){
                    _context.Departments.Remove(department);
                  var complete = await _context.SaveChangesAsync();
                  output= true;
                }
            }catch(Exception ex){
                throw;
            }
            return output;
        }
    }
}