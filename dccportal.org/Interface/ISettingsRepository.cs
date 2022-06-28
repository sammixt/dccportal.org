using dccportal.org.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dccportal.org.Interface
{
    public interface ISettingsRepository
    {
        Task<List<PostDto>> GetPost();
        Task<List<RolesDto>> GetRoles();
        Task<List<SelectListItem>> GetStates();
    }
}