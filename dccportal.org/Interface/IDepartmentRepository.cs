using dccportal.org.Dto;

namespace dccportal.org.Interface
{
    public interface IDepartmentRepository
    {
        Task<string> CountDepts();
        Task<int> CreateDepartment(DepartmentDto dto);
        Task<bool> DeleteDepartment(string deptId);
        Task<int> EditDept(DepartmentDto dto);
        Task<DataTableDto<DepartmentDto>> GetAllDepartments(DataTableRequestDto dtRequest);
        Task<DataTableDto<DepartmentDashbordDto>> GetAllDepartmentsDashboard(DataTableRequestDto dtRequest);
        Task<List<DepartmentDto>> GetAllDeptsAsync();
        Task<DepartmentDto> GetDepartment(string department);
    }
}