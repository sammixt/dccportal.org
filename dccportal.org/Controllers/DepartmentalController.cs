using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
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
    public class DepartmentalController : Controller
    {
        private readonly ILogger<DepartmentalController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentalController(ILogger<DepartmentalController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
              _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MembersInDept()
        {
            ViewBag.PageName = $"Members in IT";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MemberInDept()
        {
            try
            {
                var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int id = Convert.ToInt32(DeptId);
            
                var DataTableRequest = Request.GetDataTableRequestForm();
                var believers =  await _unitOfWork.MemberRepository.GetMembersInDept(DataTableRequest,id);
                return Ok(believers);
            }
            catch (System.Exception ex)
            {
                 _logger.LogError("MemberInDept",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        public async Task<ActionResult<List<PostDto>>> GetPost()
        {
            try
            {
                 var posts = await _unitOfWork.SettingsRepository.GetPost();
                return Ok(posts);
            }
            catch (System.Exception e)
            {
                 _logger.LogError("GetPost",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> MemberDetail(string _memberId)
        {
            try
            {
                var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int id = Convert.ToInt32(DeptId);
                ViewBag.PageName = $"Personal Info";
                var member = await _unitOfWork.BelieverRepository.GetMember(_memberId);
                ViewBag.States = await _unitOfWork.SettingsRepository.GetStates();
                var GetDeptCount = await _unitOfWork.MemberRepository.CountUserDept(_memberId);
                if (GetDeptCount == 0)
                {
                    ViewBag.DeptDetails = null;
                }
                else
                {
                    var DeptDetails = await _unitOfWork.MemberRepository.GetMemberDetails(_memberId);
                    ViewBag.DeptDetails =  DeptDetails.Where(x => x.DeptId == id).FirstOrDefault();
                }
                return View(member);
            }
            catch (System.Exception e)
            {
                _logger.LogError("MemberDetail",e);
                return View(new BelieversDto());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AssignToUnit([FromBody]MemberDto collection)
        {
           try
           {
               if (!ModelState.IsValid)
                {
                var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = modelErrors });
                }
                var believer = await _unitOfWork.MemberRepository.AddMemberToUnit(collection);
                if(believer == -1) return BadRequest(new ApiResponse(400, "record not found"));
                if(believer > 0) return Ok(new ApiResponse(200, "user added to unit"));

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ApiResponse(500, "Unable to add user to unit at this time"));
           }
           catch (System.Exception e)
           {
                _logger.LogError("AssignToUnit",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
           }
        }

        public IActionResult UnitsInDept()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Units()
        {

            try
            {
                var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int deptId = Convert.ToInt32(DeptId);
                var DataTableRequest = Request.GetDataTableRequestForm();
                var departments =  await _unitOfWork.UnitRepository.GetAllUnits(DataTableRequest,deptId);
                
                return Ok(departments);
            }
            catch (System.Exception e)
            {
                 _logger.LogError("Units",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddUnit([FromBody] UnitDto model)
        {
             try
             {
                 if(!ModelState.IsValid){
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = modelErrors });
                }
                var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int deptId = Convert.ToInt32(DeptId);
                model.DeptId = deptId;
                var unit = await _unitOfWork.UnitRepository.CreateUnit(model);
                if(unit == -1) return BadRequest(new ApiResponse(400, "Unit name already exist for Department"));
                if(unit > 0) return Ok(new ApiResponse(200, "Unit created"));
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to create unit at this time"));
             }
             catch (System.Exception e)
             {
                 _logger.LogError("AddUnit",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
             }
        }

        public async Task<ActionResult<UnitDto>> GetUnitDetail([FromQuery] UnitDto model)
        {
            try
            {
                 var units = await _unitOfWork.UnitRepository.GetUnit(model.SetUnitIdString);
                 return units;
            }
            catch (System.Exception e)
            {
                 _logger.LogError("GetUnitDetail",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> EditUnit([FromBody] UnitDto model)
        {
            try{
                    if(!ModelState.IsValid){
                    var modelErrors = new List<string>();
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var modelError in modelState.Errors)
                        {
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    }
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = modelErrors });
                }
                var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int deptId = Convert.ToInt32(DeptId);
                model.DeptId = deptId;
                var output = await _unitOfWork.UnitRepository.EditUnit(model);
                if(output == -1) return BadRequest(new ApiResponse(400, "Unit record does not exist"));
                if(output > 0) return Ok(new ApiResponse(200, "Unit record updated"));

                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update unit at this time"));
            }catch(Exception e){
                _logger.LogError("EditUnit",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult MembersInUnit(string _unitId)
        {
            ViewBag.PageName = $"Members in Web Unit";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MemberInUnit(string unitId)
        {
            try
            {
                 var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                int id = Convert.ToInt32(DeptId);
                var DataTableRequest = Request.GetDataTableRequestForm();
                var believers =  await _unitOfWork.MemberRepository.GetMembersInUnit(DataTableRequest,id,unitId);
                return Ok(believers);
            }
            catch (System.Exception e)
            {
                _logger.LogError("MemberInUnit",e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}