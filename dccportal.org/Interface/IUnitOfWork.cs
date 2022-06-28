using dccportal.org.Interface;

namespace dccportal.org.Interface
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IBelieverRepository BelieverRepository { get; }
        IMemberRepository MemberRepository { get; }
        ISettingsRepository SettingsRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IUnitRepository UnitRepository { get; }
        IAttendanceRepository AttendanceRepository { get; }
        IFinanceRepository FinanceRepository { get; }

        Task<bool> Complete();
        bool HasChanges();
    }
}