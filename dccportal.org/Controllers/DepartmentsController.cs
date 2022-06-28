using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dccportal.org.Extensions;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace dccportal.org.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DepartmentsController : Controller
    {
        private readonly ILogger<DepartmentsController> _logger;
         private readonly IUnitOfWork _unitOfWork;
        public DepartmentsController(ILogger<DepartmentsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.PageName = $"Departments";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Departments()
        {
            try
            {
                 var DataTableRequest = Request.GetDataTableRequestForm();
                 var departments =  await _unitOfWork.DepartmentRepository.GetAllDepartments(DataTableRequest);
            
                return Ok(departments);
            }
            catch (System.Exception ex)
            {
                 _logger.LogError("Departments",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment([FromBody] DepartmentDto model)
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
                var believer = await _unitOfWork.DepartmentRepository.CreateDepartment(model);
                if(believer == -1) return BadRequest(new ApiResponse(400, "Department record already exist"));
                if(believer > 0) return Ok(new ApiResponse(200, "Department created"));

                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to create department at this time"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError("AddDepartment",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<ActionResult> EditDept([FromBody] DepartmentDto model)
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
                var output = await _unitOfWork.DepartmentRepository.EditDept(model);
                if(output == -1) return BadRequest(new ApiResponse(400, "Department record does not exist"));
                if(output > 0) return Ok(new ApiResponse(200, "Department record updated"));

                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update department at this time"));
            }
            catch (System.Exception ex)
            {
                  _logger.LogError("EditDept",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteDepartment([FromQuery] DepartmentDto model)
        {
            try{
                
                bool isDeleted = await _unitOfWork.DepartmentRepository.DeleteDepartment(model.SetDeptIdString);
                if(isDeleted){
                    return Ok(new ApiResponse(200,"Department  deleted"));
                }else{
                    return BadRequest(new ApiResponse(400,"An error occured"));
                }
            }catch(Exception ex){
                  _logger.LogError("DeleteDepartment",ex);
                return BadRequest(new ApiResponse(400,"An error occured"));
            }
            

        }
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartment()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllDeptsAsync();
           
            return departments;
        }

        public async Task<ActionResult<DepartmentDto>> GetDepartmentDetail([FromQuery] DepartmentDto model)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetDepartment(model.SetDeptIdString);
            return departments;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}