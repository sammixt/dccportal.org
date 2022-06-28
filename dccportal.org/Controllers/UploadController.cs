using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dccportal.org.Dto;
using dccportal.org.Interface;
using dccportal.org.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace dccportal.org.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment webHostEnvironment;

        public UploadController(ILogger<UploadController> logger, IWebHostEnvironment hostEnvironment,
         IUnitOfWork unitOfWork)
        {
            _logger = logger;
            webHostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.PageName = $"Upload Members";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadUser(UploadUserDto users)
        {
            if (users.ExcelFile == null || users.ExcelFile.Length <= 0)  
            {  
                 return BadRequest(new ApiResponse(400,"file cannot be empty"));
            }  

            if (!Path.GetExtension(users.ExcelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))  
            {  
                return BadRequest(new ApiResponse(400,"file cannot be empty"));
            } 

            var status =  await  ProcessUploadedFile(users);
            if(status != null){
                return Ok(status);
            }
           return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse(500, "Unable to upload user at this time"));
        }

        private async Task<UploadedUserStatus> ProcessUploadedFile(UploadUserDto model)
        {
            UploadedUserStatus uploadedUserStatus = new UploadedUserStatus();
            if (model.ExcelFile != null)
            {
                String uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ExcelFile.FileName;
                string filePath = Path.Combine(GetPathAndFilename(), uniqueFileName);
                //filePath = filePath.Replace("\\/", "\\");
                try
                {
                    var usersList = new List<ExcelList>();
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                       await model.ExcelFile.CopyToAsync(fileStream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(fileStream))
                        {
                        var currentSheet = package.Workbook.Worksheets;
                            //var workSheet = currentSheet[model.DeptId.ToString()];
                        var workSheet = currentSheet.FirstOrDefault();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        //ErrorLog.log(">>>>>>>>>> About to Read Excel File <<<<<<<<<<<");
                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                            var user = new ExcelList();
                            user.FirstName = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString() : "";
                            user.LastName = workSheet.Cells[rowIterator, 2].Value != null ? workSheet.Cells[rowIterator, 2].Value.ToString() : "";
                            user.Sex = workSheet.Cells[rowIterator, 3].Value != null ? getGender(workSheet.Cells[rowIterator, 3].Value.ToString())  : "";
                            user.DateOfBirth = workSheet.Cells[rowIterator, 4].Value != null ? GetDateFromString(workSheet.Cells[rowIterator, 4].Value.ToString()) : null; ;
                            user.PhoneNumber = workSheet.Cells[rowIterator, 5].Value != null ? workSheet.Cells[rowIterator, 5].Value.ToString() : "";
                            user.HomeAddressOne = workSheet.Cells[rowIterator, 6].Value != null ? workSheet.Cells[rowIterator, 6].Value.ToString() : "";
                            user.HomeAddressTwo = workSheet.Cells[rowIterator, 7].Value != null ? workSheet.Cells[rowIterator, 7].Value.ToString() : "";
                            user.City = workSheet.Cells[rowIterator, 8].Value != null ? workSheet.Cells[rowIterator, 8].Value.ToString() : "";
                            user.StateName = workSheet.Cells[rowIterator, 9].Value != null ? workSheet.Cells[rowIterator, 9].Value.ToString() : "";
                            user.Country = workSheet.Cells[rowIterator, 10].Value != null ? workSheet.Cells[rowIterator, 10].Value.ToString() : "";
                            user.MaritalStatus = workSheet.Cells[rowIterator, 11].Value != null ? workSheet.Cells[rowIterator, 11].Value.ToString() : "";
                            user.WeddingAnniversary = workSheet.Cells[rowIterator, 12].Value != null ? GetDateFromString(workSheet.Cells[rowIterator, 12].Value.ToString()) : null;
                            user.FacebookName = workSheet.Cells[rowIterator, 13].Value != null ? workSheet.Cells[rowIterator, 13].Value.ToString() : "";
                            user.InstagramHandle = workSheet.Cells[rowIterator, 14].Value != null ? workSheet.Cells[rowIterator, 14].Value.ToString() : "";
                            user.TwitterHandle = workSheet.Cells[rowIterator, 15].Value != null ? workSheet.Cells[rowIterator, 15].Value.ToString() : "";
                            //user.AltPhoneNumber = workSheet.Cells[rowIterator, 17].Value != null ? workSheet.Cells[rowIterator, 17].Value.ToString() : "";
                            user.Email = workSheet.Cells[rowIterator, 16].Value != null ? workSheet.Cells[rowIterator, 16].Value.ToString() : "";


                            usersList.Add(user);
                            }
                        }
                    }
                    var addedUser =  await AddUserToDept(usersList,model);
                    if(addedUser.Count() > 0){
                        var failed = addedUser.Where(x => x.Status.Equals("FAILED")).ToList();
                        var success = addedUser.Where(x => x.Status.Equals("SUCCESS")).ToList();
                        uploadedUserStatus.Failed = failed;
                        uploadedUserStatus.Successful = success;
                        return uploadedUserStatus;
                    }else{
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ;
                }

            }
            return null;
            //return uniqueFileName;
        }

        private async Task<List<UploadedUser>> AddUserToDept(List<ExcelList> usersList, UploadUserDto uploadDto)
        {
             List<UploadedUser> uploadedUsers = new List<UploadedUser>();
                try
                    {
                    //ErrorLog.log(">>>>>>>>>> About to Commit to Database Upload <<<<<<<<<<<");
                    foreach (var a in usersList)
                        {
                            UploadedUser uploadedUser = new UploadedUser();
                            BelieversDto bv = new BelieversDto();
                            bv.FirstName = a.FirstName;
                            bv.LastName = a.LastName;
                            bv.Sex = a.Sex;
                            bv.DateOfBirth = a.DateOfBirth;
                            bv.PhoneNumber = a.PhoneNumber;
                            bv.HomeAddressOne = a.HomeAddressOne;
                            bv.HomeAddressTwo = a.HomeAddressTwo;
                            bv.City = a.City;
                            bv.StateName = a.StateName;
                            bv.Country = a.Country;
                            bv.MaritalStatus = a.MaritalStatus;
                            bv.WeddingAnniversary = a.WeddingAnniversary;
                            bv.FacebookName = a.FacebookName;
                            bv.InstagramHandle = a.InstagramHandle;
                            bv.TwitterHandle = a.TwitterHandle;
                            bv.AltPhoneNumber = a.AltPhoneNumber;
                            bv.Email = a.Email;
                            uploadedUser.FirstName = a.FirstName;
                            uploadedUser.LastName = a.LastName;
                            uploadedUser.Gender = a.Sex;
                             var believer = await _unitOfWork.BelieverRepository.CreateUserViaUpload(bv);
                             if(believer.Item1 || (!believer.Item1 && believer.Item2 != -2)){
                              var addedToDept =  await _unitOfWork.MemberRepository.addUserToDept(believer.Item2, uploadDto.DeptId);
                              if(addedToDept > 0){
                                  uploadedUser.Status = "SUCCESS";
                              }else if(addedToDept == -1){
                                  uploadedUser.Status = "FAILED";
                                  uploadedUser.Message = "User already exist";
                              }else{
                                  uploadedUser.Status = "FAILED";
                                  uploadedUser.Message = "Could not add to Department";
                              }
                             } else{
                                  uploadedUser.Status = "FAILED";
                                  uploadedUser.Message = "Unable to add user";
                             }
                           
                           uploadedUsers.Add(uploadedUser);
                        }
                    }
                catch (Exception ex)
                {
                    throw;
                }
                return uploadedUsers;
    }

        private string GetPathAndFilename()
        {
            return @$"{this.webHostEnvironment.WebRootPath}/Doc/";
        }

        private static DateTime? GetDateFromString(string InputDate)
        {
            Nullable<DateTime> MyNullableDate = null;
            if (InputDate != null || InputDate != "")
            {

                // We must have a double to convert the OA date to a real date.
                double d;// = double.Parse(InputDate);
                if (double.TryParse(InputDate, out d))
                    {
                    DateTime? conv = DateTime.FromOADate(d);
                    return conv.Value.Date;
                    }
                else
                    {
                    return MyNullableDate;
                    }
                // Get the converted date from the OLE automation date.
                

                // Write to console.
                
                //string day = null;
                //string month = null;
                //string year = null;
                //string fullDate = null;
                //year = InputDate.Substring(0, 4);
                //month = InputDate.Substring(4, 2);
                //day = InputDate.Substring(6, 2);
                //fullDate = month + "/" + day + "/" + year;
                //DateTime? date = DateTime.ParseExact(fullDate, "MM/dd/yyyy", null);

               
            }
            else
            {
                return MyNullableDate;
            }
        }

        private static string getGender(string prefix){
            if(string.IsNullOrEmpty(prefix)) return null;
            if (prefix.ToLower().Equals("m") || prefix.ToLower().Equals("male"))
                return "Male";
            if (prefix.ToLower().Equals("f") || prefix.ToLower().Equals("female"))
                return "Female";
            else
                return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}