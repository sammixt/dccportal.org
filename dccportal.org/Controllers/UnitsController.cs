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
    public class UnitsController : Controller
    {
        private readonly ILogger<UnitsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnitsController(ILogger<UnitsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.PageName = $"Units";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Units()
        {
            var DataTableRequest = Request.GetDataTableRequestForm();
            var departments =  await _unitOfWork.UnitRepository.GetAllUnits(DataTableRequest);
            
            return Ok(departments);
        }
        //Get Units Per Dept
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnitPerDept()
        {
             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            var units = await _unitOfWork.UnitRepository.GetUnitPerDept(deptId);
           
            return units;
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
                var unit = await _unitOfWork.UnitRepository.CreateUnit(model);
                if(unit == -1) return BadRequest(new ApiResponse(400, "Unit name already exist for Department"));
                if(unit > 0) return Ok(new ApiResponse(200, "Unit created"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError("AddUnit",ex);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to create unit at this time"));
        }

        public async Task<ActionResult<UnitDto>> GetUnitDetail([FromQuery] UnitDto model)
        {
            var units = await _unitOfWork.UnitRepository.GetUnit(model.SetUnitIdString);
            return units;
        }

        [HttpPut]
        public async Task<ActionResult> EditUnit([FromBody] UnitDto model)
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
                var output = await _unitOfWork.UnitRepository.EditUnit(model);
                if(output == -1) return BadRequest(new ApiResponse(400, "Unit record does not exist"));
                if(output > 0) return Ok(new ApiResponse(200, "Unit record updated"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError("EditUnit",ex);
            }
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update unit at this time"));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUnit([FromQuery] UnitDto model)
        {
            try{
                
                bool isDeleted = await _unitOfWork.UnitRepository.DeleteUnit(model.SetUnitIdString);
                if(isDeleted){
                    return Ok(new ApiResponse(200,"Unit  deleted"));
                }else{
                    return BadRequest(new ApiResponse(400,"An error occured"));
                }
            }catch(Exception ex){
                _logger.LogError("DeleteUnit",ex);
                return BadRequest(new ApiResponse(400,"An error occured"));
            }
            

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}