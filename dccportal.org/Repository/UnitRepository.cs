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

    public class UnitRepository : IUnitRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public UnitRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CountUnit(){
            int TotalUnits = await _context.Units.CountAsync();
            return Convert.ToString(TotalUnits);
        }

         public async Task<string> CountUnitInDept(int deptId){
            int TotalUnits = await _context.Units.Where(u => u.DeptId == deptId).CountAsync();
            return Convert.ToString(TotalUnits);
        }
        public async Task<DataTableDto<UnitDto>> GetAllUnits(DataTableRequestDto dtRequest)
        {
            try
            {
                var query = _context.Units.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.UnitName.Contains(dtRequest.SearchValue)
                                                || m.Dept.DeptName.Contains(dtRequest.SearchValue)
                                                || m.UnitShortCode.Contains(dtRequest.SearchValue));
                }

                var size = await query.CountAsync();

                var result = await query
                    .AsNoTracking()
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)
                    .Take(dtRequest.PageSize)
                    .Include(x => x.Dept)
                    .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();
                
                return (new DataTableDto<UnitDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = result,
                    RecordsFiltered = size,
                    RecordsTotal = size
                });
            }
            catch (Exception ex)
            {
                return (new DataTableDto<UnitDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = new List<UnitDto>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                });
            }
        }

         public async Task<DataTableDto<UnitDto>> GetAllUnits(DataTableRequestDto dtRequest, int deptId)
        {
            try
            {
                var query = _context.Units.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.UnitName.Contains(dtRequest.SearchValue)
                                                || m.Dept.DeptName.Contains(dtRequest.SearchValue)
                                                || m.UnitShortCode.Contains(dtRequest.SearchValue));
                }

                var size = await query.Where(x => x.DeptId == deptId).CountAsync();

                var result = await query
                    .Where(x => x.DeptId == deptId)
                    .AsNoTracking()
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)
                    .Take(dtRequest.PageSize)
                    .Include(x => x.Dept)
                    .ProjectTo<UnitDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync();
                
                return (new DataTableDto<UnitDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = result,
                    RecordsFiltered = size,
                    RecordsTotal = size
                });
            }
            catch (Exception ex)
            {
                return (new DataTableDto<UnitDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = new List<UnitDto>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                });
            }
        }

         public async Task<DataTableDto<UnitDashboardDto>> GetAllUnitsDashboard(DataTableRequestDto dtRequest, int deptId)
        {
            try
            {
                var query = _context.Units.AsQueryable();
                if (!string.IsNullOrEmpty(dtRequest.SearchValue))
                {
                    query = query.Where(m => m.UnitName.Contains(dtRequest.SearchValue));
                }

                var size = await query.Where(x => x.DeptId == deptId).CountAsync();

                var result = await query
                    .Where(x => x.DeptId == deptId)
                    .AsNoTracking()
                    .Skip((dtRequest.Skip / dtRequest.PageSize) * dtRequest.PageSize)
                    .Take(dtRequest.PageSize)
                    .Include(x => x.Members)
                    .Select(m => new UnitDashboardDto() {
                        UnitName = m.UnitName,
                        UnitFunction = m.UnitFunction,
                        Members = m.Members.Count()
                    })
                    .ToArrayAsync();
                
                return (new DataTableDto<UnitDashboardDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = result,
                    RecordsFiltered = size,
                    RecordsTotal = size
                });
            }
            catch (Exception ex)
            {
                return (new DataTableDto<UnitDashboardDto>
                {
                    Draw = Convert.ToInt32(dtRequest.Draw),
                    Data = new List<UnitDashboardDto>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0
                });
            }
        }

        public async Task<int> CreateUnit(UnitDto dto)
        {
           try{
                var unit = _mapper.Map<UnitDto,Unit>(dto);
                var unitExist = await _context.Units
                                        .Where(x => x.UnitName.ToLower() == dto.UnitName.ToLower())
                                        .Where(d => d.DeptId == dto.DeptId)
                                        .AnyAsync();
                var dept = await _context.Departments.FirstOrDefaultAsync(x => x.DeptId == dto.DeptId);
                if(dept == null) return -2;
                unit.Dept = dept;
                if(unitExist) return -1;
                _context.Units.Add(unit);
                return await _context.SaveChangesAsync();
           }catch(Exception ex){
               throw;
           }
        }

        public async Task<UnitDto> GetUnit(string unit)
        {
            try{
                 var unitIdString = Encrypter.Decrypt(unit,Constants.PASSPHRASE);
                int unitIdInt = Convert.ToInt32(unitIdString);
                  var query = _context.Units.AsQueryable();
                  var pQuery = query.ProjectTo<UnitDto>(_mapper
              .ConfigurationProvider).AsNoTracking();
            var units = await pQuery.FirstOrDefaultAsync(x => x.UnitId == unitIdInt);
            return units;
            }catch(Exception ex){
               throw;
           }
           
        }

        public async Task<List<UnitDto>> GetUnitPerDept(int department)
        {
            try{
                 
                  var query = _context.Units.AsQueryable();
                  var pQuery = query.ProjectTo<UnitDto>(_mapper
              .ConfigurationProvider).AsNoTracking();
            var units = await pQuery.Where(x => x.DeptId == department).ToListAsync();
            return units;
            }catch(Exception ex){
               throw;
           }
           
        }

        public async Task<int> EditUnit(UnitDto dto)
        {
            try{
            var unitIdString = Encrypter.Decrypt(dto.SetUnitIdString,Constants.PASSPHRASE);
            int unitIdInt = Convert.ToInt32(unitIdString);
            dto.UnitId = unitIdInt;
            var unit = _mapper.Map<UnitDto,Unit>(dto);
            var currentUnit = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == unitIdInt);
            if(currentUnit == null) return -1;

            currentUnit.UnitName = unit.UnitName ?? currentUnit.UnitName;
            currentUnit.UnitFunction = unit.UnitFunction ?? currentUnit.UnitFunction;
            currentUnit.UnitShortCode = unit.UnitShortCode ?? currentUnit.UnitShortCode;
            //currentUnit.DeptId = unit.DeptId;
            
            return await _context.SaveChangesAsync();
            }catch(Exception ex){
                throw;
            }
            
        }

        public async Task<bool> DeleteUnit(string unitId)
        {
            bool output = false;
            try{
                var unitIdString = Encrypter.Decrypt(unitId,Constants.PASSPHRASE);
                int unitIdInt = Convert.ToInt32(unitIdString);
                var unit = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == unitIdInt);
                if(unit != null){
                    _context.Units.Remove(unit);
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