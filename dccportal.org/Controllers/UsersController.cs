using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Extensions;
using dccportal.org.Helper;
using dccportal.org.Interface;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dccportal.org.Controllers
{
    //[Route("[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(ILogger<UsersController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  IActionResult BelieversList()
        {
              ViewBag.PageName = $"Believers";
            return View();
        }


        public async Task<ActionResult<IEnumerable<BelieversDto>>> GetBelievers([FromQuery]PaginationParams pages)
        {
            var believers = await _unitOfWork.BelieverRepository.GetAllMembers(pages);
            Response.AddPaginationHeader(believers.CurrentPage, believers.PageSize,
                believers.TotalCount, believers.TotalPages);
            return believers;
        }

        [HttpPost]
        public async Task<ActionResult> Believers()
        {
            var DataTableRequest = Request.GetDataTableRequestForm();
            var believers =  await _unitOfWork.BelieverRepository.GetAllMembers(DataTableRequest);
            
            return Ok(believers);
        }

        [HttpPut]
        public async Task<ActionResult> EditUser([FromBody] BelieversDto model)
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
                if(!string.IsNullOrEmpty(model.SetDateOfBirth)) model.DateOfBirth = DateTime.ParseExact(model.SetDateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if(!string.IsNullOrEmpty(model.SetWeddingAnniversary)) model.WeddingAnniversary = DateTime.ParseExact(model.SetWeddingAnniversary, "yyyy-MM-dd", CultureInfo.InvariantCulture);;
                var believer = await _unitOfWork.BelieverRepository.EditUser(model);
                if(believer == -1) return BadRequest(new ApiResponse(400, "User record does not exist"));
                if(believer > 0) return Ok(new ApiResponse(200, "Users record updated"));
            }
            catch (System.Exception ex)
            {
               _logger.LogError("EditUser",ex);
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update  user at this time"));
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] BelieversDto model)
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
                if(!string.IsNullOrEmpty(model.SetDateOfBirth)) model.DateOfBirth = DateTime.ParseExact(model.SetDateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                if(!string.IsNullOrEmpty(model.SetWeddingAnniversary)) model.WeddingAnniversary = DateTime.ParseExact(model.SetWeddingAnniversary, "yyyy-MM-dd", CultureInfo.InvariantCulture);;
                var believer = await _unitOfWork.BelieverRepository.CreateUser(model);
                if(believer == -1) return BadRequest(new ApiResponse(400, "User record already exist"));
                if(believer > 0) return Ok(new ApiResponse(200, "User created"));
            }
            catch (System.Exception ex)
            {
               _logger.LogError("AddUser",ex);
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to create user at this time"));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBeliever(int id)
        {
            try{
                
                bool isDeleted = await _unitOfWork.BelieverRepository.DeleteMember(id);
                if(isDeleted){
                    return Ok(new ApiResponse(200,"Believers record deleted"));
                }else{
                    return BadRequest(new ApiResponse(400,"An error occured"));
                }
            }catch(Exception ex){
                return BadRequest(new ApiResponse(400,"An error occured"));
            }
            

        }

        public async Task<ActionResult> BelieverForm()
        {
              ViewBag.PageName = $"New Believer";
              ViewBag.States = await _unitOfWork.SettingsRepository.GetStates();
            return View();
        }

        public async Task<ActionResult> Believers(string _memberId)
        {
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
                ViewBag.DeptDetails = await _unitOfWork.MemberRepository.GetMemberDetails(_memberId);
            }
            return View(member);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}