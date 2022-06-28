using dccportal.org.Dto;

namespace dccportal.org.Interface
{
    public interface IAttendanceRepository
    {
        Task<bool> CheckAttendanceRecordExist(DateTime date, int DeptId, string group);
        Task<int> GetAndInsertUserToAttendanceTable(WorkerAttendanceDto dto);
        Task<WorkerAttendanceOutput> GetAttendanceInfo(string _idstr);
        Task<int> SearchAttendanceRecordExist(DateTime date, int DeptId, string group);
        Task UpdateStatus(AttendanceStatusUpdate dto);
    }
}