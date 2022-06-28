using dccportal.org.Dto;

namespace dccportal.org.Interface
{
    public interface IMemberRepository
    {
        Task<int> AddMemberToUnit(MemberDto member);
        Task<int> addUserToDept(int UserId, int DeptId);
        Task<string> CountMembers();
        Task<string> CountMembersInDept(int deptId);
        Task<int> CountUserDept(string _memberId);
        Task<int> DeleteMember(MemberDepartmentDto memberDto);
        Task<List<MemberDepartmentDto>> GetMemberDetails(string _memberId);
        Task<DataTableDto<AllMembersDto>> GetMembers(DataTableRequestDto dtRequest);
        Task<DataTableDto<BelieversDto>> GetMembersInDept(DataTableRequestDto dtRequest, string dept);
        Task<DataTableDto<BelieversDto>> GetMembersInDept(DataTableRequestDto dtRequest, int dept);
        Task<DataTableDto<BelieversDto>> GetMembersInUnit(DataTableRequestDto dtRequest, int dept, string unitId);
        Task<int> InsertMember(MemberDto memberDto);
    }
}