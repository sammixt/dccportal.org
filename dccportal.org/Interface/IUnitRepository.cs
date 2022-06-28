using dccportal.org.Dto;

namespace dccportal.org.Interface
{
    public interface IUnitRepository
    {
        Task<string> CountUnit();
        Task<string> CountUnitInDept(int deptId);
        Task<int> CreateUnit(UnitDto dto);
        Task<bool> DeleteUnit(string unitId);
        Task<int> EditUnit(UnitDto dto);
        Task<DataTableDto<UnitDto>> GetAllUnits(DataTableRequestDto dtRequest);
        Task<DataTableDto<UnitDto>> GetAllUnits(DataTableRequestDto dtRequest, int deptId);
        Task<DataTableDto<UnitDashboardDto>> GetAllUnitsDashboard(DataTableRequestDto dtRequest, int deptId);
        Task<UnitDto> GetUnit(string unit);
        Task<List<UnitDto>> GetUnitPerDept(int department);
    }
}