using dccportal.org.Dto;
using dccportal.org.Helper;

namespace dccportal.org.Interface
{
    public interface IBelieverRepository
    {
        Task<PagedList<BelieversDto>> GetAllMembers(PaginationParams paginationParams);
        Task<DataTableDto<BelieversDto>> GetAllMembers(DataTableRequestDto dtRequest);
        Task<bool> DeleteMember(int memberId);
        Task<BelieversDto> GetMember(string memberId);
        Task<int> EditUser(BelieversDto dto);
        Task<int> CreateUser(BelieversDto dto);
        Task<Tuple<bool,int>> CreateUserViaUpload(BelieversDto dto);
        Task<string> CountBelivers();
    }
}