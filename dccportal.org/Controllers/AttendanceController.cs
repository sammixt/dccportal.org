using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Helper;
using dccportal.org.Interface;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dccportal.org.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AttendanceController : Controller
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IUnitOfWork _unitOfWork ;

        public AttendanceController(ILogger<AttendanceController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index(string _idstr)
        {
            ViewBag.PageName = $"Attendance";
            WorkerAttendanceOutput output = new WorkerAttendanceOutput();
            if(!string.IsNullOrEmpty(_idstr)){
                output = await _unitOfWork.AttendanceRepository.GetAttendanceInfo(_idstr);
                ViewBag.PageName = $"Attendance for {output.GetDate}";
            }
            return View(output);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateDate([FromBody] WorkerAttendanceDto model)
        {
            try
            {
                var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int dept = Convert.ToInt32(DeptId);
           
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
            model.Department = dept;
            if(model.DepartmentGroup.Equals("ALL"))
                model.DepartmentGroup = null;
            if(string.IsNullOrEmpty(model.SetAttendanceDate)) return BadRequest(new ApiResponse(400, "Unrecognised date format"));
            model.Date = DateTime.ParseExact(model.SetAttendanceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
           var hasAttendance = await _unitOfWork.AttendanceRepository.CheckAttendanceRecordExist(model.Date,dept,model.DepartmentGroup);
            if(hasAttendance) return BadRequest(new ApiResponse(400, $"Attendance already exist for {model.Date.ToShortDateString}"));
            
            var setUpAttendance = await _unitOfWork.AttendanceRepository.GetAndInsertUserToAttendanceTable(model);
            if(setUpAttendance == -1) return BadRequest(new ApiResponse(400, "No member found"));
            if(setUpAttendance == -2) return BadRequest(new ApiResponse(400, "An error occurred"));
            string idString = Encrypter.Encrypt(Convert.ToString(setUpAttendance),Constants.PASSPHRASE);
            if(setUpAttendance > 0) return Ok(new ApiResponse(200, idString));

            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update  user at this time"));
            }catch(Exception e){
                _logger.LogError("ValidateDate",e);
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update  user at this time"));
            }
            
        }

         [HttpPost]
        public async Task<ActionResult> SearchDate([FromBody] WorkerAttendanceDto model)
        {
            try
            {
                var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int dept = Convert.ToInt32(DeptId);
           
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
            model.Department = dept;
            if(model.DepartmentGroup.Equals("ALL"))
                model.DepartmentGroup = null;
            if(string.IsNullOrEmpty(model.SetAttendanceDate)) return BadRequest(new ApiResponse(400, "Unrecognised date format"));
            model.Date = DateTime.ParseExact(model.SetAttendanceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
           var hasAttendance = await _unitOfWork.AttendanceRepository.SearchAttendanceRecordExist(model.Date,dept,model.DepartmentGroup);
            if(hasAttendance == 0) return BadRequest(new ApiResponse(400, $"Attendance record does not exist for {model.Date.ToShortDateString}"));
            
            
            string idString = Encrypter.Encrypt(Convert.ToString(hasAttendance),Constants.PASSPHRASE);
            return Ok(new ApiResponse(200, idString));
            }
            catch (System.Exception e)
            {
                _logger.LogError("SearchDate",e);
                 return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to fetch  attendance at this time"));
            }
        }

        public async Task<ActionResult> UpdateStatus([FromBody] AttendanceStatusUpdate dto)
        {
            try
            {
               await _unitOfWork.AttendanceRepository.UpdateStatus(dto);
               return Ok(new ApiResponse(200,"Updated"));
            }catch(Exception ex)
            {
                _logger.LogError("UpdateStatus",ex);
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update  user at this time"));
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}