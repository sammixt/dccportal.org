using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using dccportal.org.Dto;
using dccportal.org.Entities;
using dccportal.org.Interface;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using dccportal.org.Helper;

namespace dccportal.org.Repository
{

    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public AttendanceRepository(dccportaldbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckAttendanceRecordExist(DateTime date, int DeptId, string group)
        {

            bool outcome = true;
            int AttendanceRecord = 0;
            try
            {
                var query = _context.WorkerAttendances.AsQueryable();
                if (string.IsNullOrEmpty(group))
                {
                    AttendanceRecord = await query.Where(d => d.Date == date)
                   .Where(m => m.Department == DeptId).CountAsync();
                }
                else
                {
                    AttendanceRecord = await query
                      .Where(d => d.Date == date)
                      .Where(g => g.DepartmentGroup.Equals(group))
                      .Where(m => m.Department == DeptId).CountAsync();
                }

                if (AttendanceRecord > 0)
                    outcome = true;
                else
                    outcome = false;
            }
            catch (Exception ex)
            {
                throw;
            }
            return outcome;
        }

        public async Task<int> SearchAttendanceRecordExist(DateTime date, int DeptId, string group)
        {

            int outcome = 0;
            WorkerAttendance AttendanceRecord = null;
            try
            {
                var query = _context.WorkerAttendances.AsQueryable();
                if (string.IsNullOrEmpty(group))
                {
                    AttendanceRecord = await query.Where(d => d.Date == date)
                   .Where(m => m.Department == DeptId).FirstOrDefaultAsync();
                }
                else
                {
                    AttendanceRecord = await query
                      .Where(d => d.Date == date)
                      .Where(g => g.DepartmentGroup.Equals(group))
                      .Where(m => m.Department == DeptId).FirstOrDefaultAsync();
                }

                if (AttendanceRecord != null)
                    outcome = AttendanceRecord.Id;
                else
                    outcome = 0;
            }
            catch (Exception ex)
            {
                throw;
            }
            return outcome;
        }

        public async Task<int> GetAndInsertUserToAttendanceTable(WorkerAttendanceDto dto)
        {
            List<WorkersAttendanceRegister> member = new List<WorkersAttendanceRegister>();
            try
            {
                var query = _context.Members.AsQueryable();
                if (!string.IsNullOrEmpty(dto.DepartmentGroup))
                {
                    query =  query.Where(m => m.DeptId == dto.Department && m.Groups == dto.DepartmentGroup);
                    
                }
                else
                {
                   query =  query.Where(m => m.DeptId == dto.Department);
                }

                member = await query.Select(x => new WorkersAttendanceRegister(){
                        FirstName = x.Believer.FirstName,
                        LastName = x.Believer.LastName,
                        BelieverId = x.Believer.MemberId,
                        MemberId = x.MemberId
                    }).ToListAsync();
                if(member.Count() > 1)
                {
                    var model = _mapper.Map<WorkerAttendanceDto,WorkerAttendance>(dto);
                    string data = JsonSerializer.Serialize(member);
                    model.Value = data;
                    _context.WorkerAttendances.Add(model);
                    int res =  await _context.SaveChangesAsync();
                    if(res > 0)
                        return model.Id;
                    else
                        return -2;
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<WorkerAttendanceOutput>GetAttendanceInfo(string _idstr)
        {
            WorkerAttendanceOutput output = new WorkerAttendanceOutput();
            try{
                string idString = Encrypter.Decrypt(_idstr,Constants.PASSPHRASE);
                int id = Convert.ToInt32(idString);
                var model = await _context.WorkerAttendances.FirstOrDefaultAsync(x => x.Id == id);
                if(model != null){
                    output.Date = model.Date;
                    output.Department = model.Department;
                    output.DepartmentGroup = model.DepartmentGroup;
                    output.AttendanceRegisters = JsonSerializer.Deserialize<List<WorkersAttendanceRegister>>(model.Value);
                }

                return output;
            }catch(Exception e){
                throw;
            }

        }

        public async Task UpdateStatus(AttendanceStatusUpdate dto)
        {
            Func<int,int,bool,bool> update = (a, b,c) => {
                if(a == b){
                    return dto.Status;
                }
                return c;
            };

            try{
                var model = await _context.WorkerAttendances.FirstOrDefaultAsync(x => x.Id == dto.AttendanceId);
                List<WorkersAttendanceRegister> attendance = JsonSerializer.Deserialize<List<WorkersAttendanceRegister>>(model.Value);
                attendance.ForEach(x => x.Status = update(x.MemberId,dto.MemberId,x.Status));
                model.Value = JsonSerializer.Serialize(attendance);
                _context.Update<WorkerAttendance>(model);
                await _context.SaveChangesAsync();

            }catch(Exception e){
                throw;
            }
        }
        
    }
}