using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dccportal.org.Models;
using dccportal.org.Dto;
using dccportal.org.Responses;
using dccportal.org.Interface;
using dccportal.org.Helper;
using dccportal.org.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace dccportal.org.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        var dept = await _unitOfWork.DepartmentRepository.GetAllDeptsAsync();
        return PartialView(dept);
    }

     [HttpPost]
    public async Task<ActionResult> CreateLogin([FromBody] UsersAccountDto model)
    {
            if(!ModelState.IsValid)
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
            if(!model.Password.Equals(model.ConfirmPassword)) return BadRequest(new ApiResponse(400, "Password and Confirm password mismatch"));
           
            var memberIdString = Encrypter.Decrypt(model.setBelieverIdString,Constants.PASSPHRASE);
                int memberIdInt = Convert.ToInt32(memberIdString);
                model.BelieverId = memberIdInt;
            bool userExist = await _unitOfWork.AccountRepository.CheckUserLoginAccountExist(model);
            if (userExist)
            {
                return Conflict(new ApiResponse(400, "User is already created in this Department"));
            }
            bool userNameExist = await _unitOfWork.AccountRepository.CheckIfUserNameExist(model);
            if (userNameExist)
            {
                return Conflict(new ApiResponse(400, "Username Already Exist"));
            }
                
            var create =  await _unitOfWork.AccountRepository.CreateUserLogin(model);
            if(create > 0) return Ok(new ApiResponse(200, "Account Created Successfully"));

            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to assign to department"));
    }
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Dashboard()
    {
        ViewBag.PageName = "Dashboard";
            // var principal = (ClaimsIdentity)User.Identity;
            //string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            ViewBag.Believer = await _unitOfWork.BelieverRepository.CountBelivers();
            ViewBag.Members = await _unitOfWork.MemberRepository.CountMembers();
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.CountDepts();
            ViewBag.Units =  await _unitOfWork.UnitRepository.CountUnit();

            return View();
    }

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeptDashboard()
    {
            ViewBag.PageName = "Dashboard";
            var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            ViewBag.MembersInDept = await _unitOfWork.MemberRepository.CountMembersInDept(deptId);
            ViewBag.UnitsInDept = await _unitOfWork.UnitRepository.CountUnitInDept(deptId);
            ViewBag.WalletBallance = await _unitOfWork.FinanceRepository.GetDeptBalance(deptId);

            return View();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DashboardDepts(){
         var DataTableRequest = Request.GetDataTableRequestForm();
         var departments =  await _unitOfWork.DepartmentRepository.GetAllDepartmentsDashboard(DataTableRequest);
        
        return Ok(departments);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DashboardUnits(){
        var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
         var DataTableRequest = Request.GetDataTableRequestForm();
         var units =  await _unitOfWork.UnitRepository.GetAllUnitsDashboard(DataTableRequest,deptId);
        
        return Ok(units);
    }

    [HttpPost]
        public async Task<ActionResult> Login([FromBody]UsersAccountDto model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Password)) return BadRequest(new ApiResponse(400, "Password was not supplied"));
                if (string.IsNullOrEmpty(model.UserName)) return BadRequest(new ApiResponse(400, "Username was not supplied"));

                var profile = await _unitOfWork.AccountRepository.AuthenticateUser(model);

                if(profile != null)
                {
                    var claims = new List<Claim>();
                if(profile.RoleId == 2)
                {
                    claims.Add(new Claim(ClaimTypes.Name, profile.Dept.DeptName));
                    claims.Add(new Claim(ClaimTypes.GivenName, profile.Dept.DeptName));
                    claims.Add(new Claim(ClaimTypes.Email, profile.Dept.DeptName));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Name, profile.UserName));
                    claims.Add(new Claim(ClaimTypes.GivenName, (profile.Believer.FirstName + " " + profile.Believer.LastName)));
                    claims.Add(new Claim(ClaimTypes.Email, Convert.ToString(profile.Believer.Email)));
                }
                
                claims.Add(new Claim(ClaimTypes.SerialNumber, Convert.ToString(profile.BelieverId)));
                
                claims.Add(new Claim(ClaimTypes.StateOrProvince, profile.Dept.DeptName));
                claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(profile.DeptId)));
                claims.Add(new Claim(ClaimTypes.Role, Convert.ToString(profile.Role.RoleName)));
                claims.Add(new Claim(ClaimTypes.Rsa, Convert.ToString(profile.RoleId)));

                //claims.Add(new Claim(ClaimTypes.Name, "scezeala"));
                //claims.Add(new Claim(ClaimTypes.GivenName, "Samuel Ezeala"));
                //claims.Add(new Claim(ClaimTypes.SerialNumber, Convert.ToString(1)));
                //claims.Add(new Claim(ClaimTypes.Email, "sezeala@gmail.com"));
                //claims.Add(new Claim(ClaimTypes.StateOrProvince, "IT"));
                //claims.Add(new Claim(ClaimTypes.Sid, Convert.ToString(1)));
                //claims.Add(new Claim(ClaimTypes.Role, Convert.ToString("Super Administrator")));
                //claims.Add(new Claim(ClaimTypes.Rsa, Convert.ToString(1)));
                var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                        IsPersistent = true,
                        AllowRefresh = true
                    };
                    //props.IsPersistent = model.RememberMe;
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
                    return Ok(new ApiResponse(200, "User Successfully Logged on"));
                }
                else
                {
                    return Ok(new ApiResponse(401, "Invalid User Name or Password"));
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Login",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

//[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public ActionResult RedirectToLocal()
        {
            if(User.IsInRole("Super Administrator"))   
                return RedirectToAction(nameof(Dashboard));
            else
                return RedirectToAction(nameof(DeptDashboard));
            
        }
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index));
        }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

