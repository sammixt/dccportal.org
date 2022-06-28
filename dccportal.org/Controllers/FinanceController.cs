using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Interface;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dccportal.org.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class FinanceController : Controller
    {
        private readonly ILogger<FinanceController> _logger;
         private readonly IUnitOfWork _unitOfWork;

        public FinanceController(ILogger<FinanceController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Setup()
        {
            //dept Id
            ViewBag.PageName = "Payment Setup";
            List<DueDto> dueDtos = new List<DueDto>();
             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            try{
                dueDtos = await _unitOfWork.FinanceRepository.Dues(deptId);
            }catch(Exception ex){
                   _logger.LogError("Payment Setup",ex);
            }
            return View(dueDtos);
        }


        [HttpPost]
        public async Task<ActionResult> AddPaymentType([FromBody] DueDto model)
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
                model.CreationDate = DateTime.Today;
                model.DeptId = dept;
                var unit = await _unitOfWork.FinanceRepository.AddPaymentType(model);
                if(unit == -1) return BadRequest(new ApiResponse(400, "Payment type already exist for Department"));
                if(unit > 0) return Ok(new ApiResponse(200, "Payment type created"));
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to add payment at this time"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError("AddPaymentType",ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
             
        }

        [HttpPut]
        public async Task<ActionResult> EditPaymentType([FromBody] DueDto model)
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
                var output = await _unitOfWork.FinanceRepository.EditPaymentType(model);
                if(output == -1) return BadRequest(new ApiResponse(400, "Payment record does not exist"));
                if(output > 0) return Ok(new ApiResponse(200, "payment record updated"));
            }
            catch (System.Exception ex)
            {
               _logger.LogError("EditPaymentType",ex);
            }
           
            return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to update payment at this time"));
            
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePaymentType([FromQuery] DueDto model)
        {
            try{             
                bool isDeleted = await _unitOfWork.FinanceRepository.DeletePaymentType(model.SetDuesIdString);
                if(isDeleted){
                    return Ok(new ApiResponse(200,"Payment type  deleted"));
                }else{
                    return BadRequest(new ApiResponse(400,"An error occured"));
                }
            }catch(Exception ex){
                _logger.LogError("DeletePaymentType",ex);
                return BadRequest(new ApiResponse(400,"An error occured"));
            }
        }

        public IActionResult MembersInDept()
        {
            ViewBag.PageName = $"Perform transaction";
            ViewBag.SubCaption = $"Select member to perform transaction";
            return View();
        }

        public async Task<ActionResult> DuesEntry(string _memberId)
        {
            ViewBag.PageName = $"Perform transaction";

             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            if (String.IsNullOrEmpty(_memberId))
            {
                return RedirectToAction(nameof(MembersInDept));
            }
            var SingleUser = await _unitOfWork.BelieverRepository.GetMember(_memberId);
            string Year = DateTime.Now.Year.ToString();
            ViewBag.PaymentRecord = await _unitOfWork.FinanceRepository.GetDuesPaymentRecordList(deptId,PaymentType: Convert.ToString(2), Year: Year, MemberId: _memberId) ?? null;
            ViewBag.DuesList = await _unitOfWork.FinanceRepository.Dues(deptId);
            ViewBag.Month = await _unitOfWork.FinanceRepository.GetMonths();
            return View(SingleUser);
        }

        public async Task<ActionResult> Expense()
        {
            ViewBag.PageName = $"Expense Entries";
             //var principal = (ClaimsIdentity)User.Identity;
            //string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            //FinanceBLL._DeptId = Convert.ToInt32(DeptId);
             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            var paymentRecord = await _unitOfWork.FinanceRepository.GetExpenditureList(deptId);
            return View(paymentRecord);
        }

        [HttpPost]
        public async Task<ActionResult> InsertExpense([FromBody]PaymentModelDto model)
        {
              
            try
            {
                  var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
                model.Amount = -model.Amount;
                model.Year = DateTime.Now.Year.ToString();
                model.TransactionType = "Debit";
                if(!string.IsNullOrEmpty(model.PaymentDateString)) model.PaymentDate = DateTime.ParseExact(model.PaymentDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                int outcome = await _unitOfWork.FinanceRepository.InsertDuesPayment(deptId,model);
                if(outcome < 1) return BadRequest(new ApiResponse(400,"An error occured"));
                return Ok(new ApiResponse(200,"Expense Captured"));
            }
            catch(Exception ex){
                _logger.LogError("InsertExpense",ex);
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to complete request at this time"));
            }
        }

        public async Task<ActionResult> GetWalletBalance(string id)
        {
             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            string WalletBallance = await _unitOfWork.FinanceRepository.GetWalletBalance(deptId,id);
            return Ok(new {amount = WalletBallance });
        }

        public async Task<ActionResult> GetDeptBalance()
        {
             var principal = (ClaimsIdentity)User.Identity;
            string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            string WalletBallance = await _unitOfWork.FinanceRepository.GetDeptBalance(deptId);
            return Ok(new {amount = WalletBallance });
        }

         public async Task<ActionResult> getAmount(int DuesId)
        {
            //var principal = (ClaimsIdentity)User.Identity;
            //string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
             var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            var amount = await _unitOfWork.FinanceRepository.GetAmountPerTransacton(deptId,DuesId);
            return Ok(new {amount = amount });
        }

        [HttpPost]
        public async Task<ActionResult> InsertToUpWallet([FromBody]WalletDto model)
        {
            var principal = (ClaimsIdentity)User.Identity;
        string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
            int deptId = Convert.ToInt32(DeptId);
            //FinanceBLL._DeptId = Convert.ToInt32(DeptId);
            int outcome = await _unitOfWork.FinanceRepository.TopUpWallet(deptId,walletDto:model);
            if(outcome < 1) return BadRequest(new ApiResponse(400, "Unable to Top up wallet"));
            return Ok(new ApiResponse(200, "Top up successful"));
        }

        [HttpPost]
        public async Task<ActionResult> InsertDuesPayment([FromBody]PaymentModelDto model)
        {
            try
            {
                model.TransactionType = "Credit";
                model.PaymentSatus = "PAID";
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

                    var principal = (ClaimsIdentity)User.Identity;
                string DeptId = principal.FindFirst(ClaimTypes.Sid).Value;
                    int deptId = Convert.ToInt32(DeptId);
                int CheckRecs;
                if(!string.IsNullOrEmpty(model.PaymentDateString)) model.PaymentDate = DateTime.ParseExact(model.PaymentDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                string Years = DateTime.Now.Year.ToString();
                if(model.DuesId == 4){
                    CheckRecs = await _unitOfWork.FinanceRepository.CheckIfRecordExist(deptId: deptId, DueType: (int)model.DuesId, month: (int)model.Month, BelieverId: (int)model.MemberId, year: Years);
                }else{
                    CheckRecs = await _unitOfWork.FinanceRepository.CheckIfRecordExist(deptId: deptId,DueType: (int)model.DuesId,BelieverId: (int)model.MemberId);
                }

                if(CheckRecs == 0){

                    if(model.PaymentMethod == 1)
                    {
                        string WalletBallance = await _unitOfWork.FinanceRepository.GetWalletBalance(deptId,(int)model.MemberId);
                    decimal _WalletBalance = (!string.IsNullOrWhiteSpace(WalletBallance))?Convert.ToDecimal(WalletBallance):0;

                    if (_WalletBalance < model.Amount)
                        {
                            return BadRequest(new ApiResponse(400,"Insufficient Funds in Wallet"));
                        }
                        else
                        {
                            await _unitOfWork.FinanceRepository.TopUpWallet(deptId,Paymentmodel:model);
                        }
                    }
                    int outcome = await _unitOfWork.FinanceRepository.InsertDuesPayment(deptId,model);
                    if(outcome < 1) return BadRequest(new ApiResponse(400,"An error occured"));
                    return Ok(new ApiResponse(200,"Payment Successful"));
                    
                }else{
                    return Conflict(new ApiResponse(409,"Record already exist"));
                }
            }catch(Exception ex){
                _logger.LogError("InsertDuesPayment",ex);
                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to complete request at this time"));
            }
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}