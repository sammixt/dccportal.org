using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Extensions;
using dccportal.org.Interface;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dccportal.org.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class MembersController : Controller
    {
        private readonly ILogger<MembersController> _logger;
         private readonly IUnitOfWork _unitOfWork;

        public MembersController(ILogger<MembersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.PageName = $"Workers";
            return View();
        }

        public async Task<ActionResult> Department(string dept)
        {
            var department = await _unitOfWork.DepartmentRepository.GetDepartment(dept);
            ViewBag.PageName = $"Members in {department?.DeptName}";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MembersInDept(string dept)
        {
            var DataTableRequest = Request.GetDataTableRequestForm();
            var believers =  await _unitOfWork.MemberRepository.GetMembersInDept(DataTableRequest,dept);
            return Ok(believers);
        }

        [HttpPost]
        public async Task<ActionResult> Members()
        {
            var DataTableRequest = Request.GetDataTableRequestForm();
            var believers =  await _unitOfWork.MemberRepository.GetMembers(DataTableRequest);
            return Ok(believers);
        }

         [HttpPost]
        public async Task<ActionResult> Create([FromBody] MemberDto model)
        {
           try
           {
                var create = await _unitOfWork.MemberRepository.InsertMember(model);
                if(create == -1) return BadRequest(new ApiResponse(400, "User is already a member of this Dept."));
                if(create > 0) return Ok(new ApiResponse(200, "Users added to  dept"));
           }
           catch (System.Exception ex)
           {
               _logger.LogError("Create",ex);
           }
           return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to assign to department"));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] MemberDepartmentDto model)
        {
           try
           {
                var create = await _unitOfWork.MemberRepository.DeleteMember(model);
                if(create == -1) return BadRequest(new ApiResponse(400, "Record does not exist."));
                if(create > 0) return Ok(new ApiResponse(200, "Member removed from dept"));
           }
           catch (System.Exception ex)
           {
                _logger.LogError("Delete",ex);
           }
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to remove member from department"));
        }

        public async Task<ActionResult<List<RolesDto>>> GetRoles(){
            return await _unitOfWork.SettingsRepository.GetRoles();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}