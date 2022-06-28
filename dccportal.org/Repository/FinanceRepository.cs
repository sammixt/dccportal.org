using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using dccportal.org.Dto;
using dccportal.org.Entities;
using dccportal.org.Helper;
using dccportal.org.Interface;
using Microsoft.EntityFrameworkCore;

namespace dccportal.org.Repository
{

    public class FinanceRepository : IFinanceRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public FinanceRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DueDto>> Dues(int deptId)
        {
            try
            {
                var dues = await _context.Dues.Where(x => x.DeptId == deptId)
                            .ProjectTo<DueDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();
                return dues;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<MonthDto>> GetMonths()
        {
            List<MonthDto> _Months = new List<MonthDto>();
            try
            {
                _Months = await _context.Months.OrderBy(m => m.MonthId)
                            .Select(x => new MonthDto() {
                                MonthId = x.MonthId,
                                Month = x.Month1
                            }).ToListAsync();
                return _Months;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
         public async Task<int> AddPaymentType(DueDto dto)
        {
           try{
                var payment = _mapper.Map<DueDto,Due>(dto);
                var paymentExist = await _context.Dues
                                        .Where(x => x.DuesName.ToLower() == dto.DuesName.ToLower())
                                        .Where(d => d.DeptId == dto.DeptId)
                                        .AnyAsync();
                
                if(paymentExist) return -1;
                _context.Dues.Add(payment);
                return await _context.SaveChangesAsync();
           }catch(Exception ex){
               throw;
           }
        }

        public async Task<int> EditPaymentType(DueDto dto)
        {
            try{
            var duesIdString = Encrypter.Decrypt(dto.SetDuesIdString,Constants.PASSPHRASE);
            int duesIdInt = Convert.ToInt32(duesIdString);
            dto.DuesId = duesIdInt;
            var due = _mapper.Map<DueDto,Due>(dto);
            var currentDue = await _context.Dues.FirstOrDefaultAsync(x => x.DuesId == duesIdInt);
            if(currentDue == null) return -1;

            currentDue.DuesName = due.DuesName ?? currentDue.DuesName;
            currentDue.DuesDesc = due.DuesDesc ?? currentDue.DuesDesc;
            currentDue.DuesType = due.DuesType ?? currentDue.DuesType;
            currentDue.Amount = due.Amount ;

            //currentUnit.DeptId = unit.DeptId;
            
            return await _context.SaveChangesAsync();
            }catch(Exception ex){
                throw;
            }
            
        }

         public async Task<bool> DeletePaymentType(string duesId)
        {
            bool output = false;
            try{
                var duesIdString = Encrypter.Decrypt(duesId,Constants.PASSPHRASE);
                int duesIdInt = Convert.ToInt32(duesIdString);
                var due = await _context.Dues.FirstOrDefaultAsync(x => x.DuesId == duesIdInt);
                if(due != null){
                    _context.Dues.Remove(due);
                  var complete = await _context.SaveChangesAsync();
                  output= true;
                }
            }catch(Exception ex){
                throw;
            }
            return output;
        }

        public async Task<List<PaymentDto>> GetDuesPaymentRecordList(int deptId, string PaymentId = null, string PaymentType = null, string Month = null, string Year = null, string MemberId = null, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<PaymentDto> _payment = new List<PaymentDto>();
            var memberIdString = Encrypter.Decrypt(MemberId,Constants.PASSPHRASE);
            int _MemberId = Convert.ToInt32(memberIdString);
            
            if (PaymentType != null && Year != null && MemberId != null)
            {
                int? _PaymentType = Convert.ToInt32(PaymentType);
                var query =  await _context.Payments.Where(m => m.MemberId == _MemberId && m.DeptId == deptId)
                .Include(m => m.MonthNavigation).DefaultIfEmpty()
                .Include(m => m.Dues).OrderBy(m => m.PaymentId).ToListAsync();
                if (query[0] != null)
                {
                    foreach (var item in query)
                    {
                        _payment.Add(new PaymentDto
                        {
                            DueType = item.Dues.DuesName,
                            Amount = (decimal)item.Amount,
                            TransactionType = item.TransactionType??null,
                            Month = item.MonthNavigation.Month1,
                            Year = item.Year??null,
                            PaymentDate = item.PaymentDate??null,
                            PaymentStatus = item.PaymentSatus

                        });
                    }
                    return _payment;
                }
                return null;

            }
            else if (StartDate != null && EndDate != null)
            {
                int? _PaymentType = Convert.ToInt32(PaymentType);
                var query = await _context.Payments.Where(m => m.MemberId == _MemberId && m.DeptId == deptId && m.DuesId == _PaymentType 
                            && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate))
                            .Include(m => m.MonthNavigation).DefaultIfEmpty()
                            .Include(m => m.Dues).OrderBy(m => m.PaymentId).ToListAsync();
                if (query[0] != null)
                {
                    foreach (var item in query)
                    {
                        _payment.Add(new PaymentDto
                        {
                            DueType = item.Dues.DuesName,
                            Amount = (decimal)item.Amount,
                            TransactionType = item.TransactionType,
                            Month = item.MonthNavigation.Month1,
                            PaymentDate = (DateTime)item.PaymentDate,
                            PaymentStatus = item.PaymentSatus
                        });
                    }
                    return _payment;
                }
                return null;
            }
            else
            {
                return null;
            }

        }

        public async Task<string> GetWalletBalance(int deptId, string MemberId)
        {
            var memberIdString = Encrypter.Decrypt(MemberId,Constants.PASSPHRASE);
            int _MemberId = Convert.ToInt32(memberIdString);
            
            decimal? SumTotal = await _context.Wallets.Where(m => m.MemberId == _MemberId && m.DeptId == deptId).SumAsync(m => m.Amount);
            return Convert.ToString(SumTotal);
        }

        public async Task<string> GetWalletBalance(int deptId, int MemberId)
        {
            decimal? SumTotal = await _context.Wallets.Where(m => m.MemberId == MemberId && m.DeptId == deptId).SumAsync(m => m.Amount);
            return Convert.ToString(SumTotal);
        }

        public async Task<string> GetDeptBalance(int deptId)
        {
            decimal? SumTotal = await _context.Payments.Where(m =>  m.DeptId == deptId).SumAsync(m => m.Amount);
            return Convert.ToString(SumTotal);
        }

        public async Task<int> TopUpWallet(int deptId, WalletDto walletDto = null,PaymentModelDto Paymentmodel = null)
        {
            Wallet Walletmodel = new Wallet();
            if (Paymentmodel != null)
            {
                Walletmodel = new Wallet();
                Walletmodel.MemberId = Paymentmodel.MemberId;
                Walletmodel.Amount = -Paymentmodel.Amount;
                Walletmodel.Description = Paymentmodel.Narration;
                Walletmodel.TransactionType = "Debit";
            }else{
                Walletmodel.Amount = walletDto.Amount;
                Walletmodel.Description = walletDto.Description;
                Walletmodel.MemberId = walletDto.MemberId;
            }
            Walletmodel.DeptId = deptId;
            Walletmodel.TransactionDate = DateTime.Now;
            
            _context.Wallets.Add(Walletmodel);
           int outcome = await _context.SaveChangesAsync();
            return outcome;
        }

        public async Task<decimal> GetAmountPerTransacton(int deptId,int DuesId)
        {
            decimal _Dues ;
            try
            {
                 _Dues = await _context.Dues.Where(m => m.DuesId == DuesId && m.DeptId == deptId)
                        .Select(m => m.Amount).FirstOrDefaultAsync();
                    return _Dues;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int>  CheckIfRecordExist(int deptId, int DueType = 0, int month = 0, int BelieverId = 0, string year = null)
        {
            int outcome;
           
            if (DueType == 4)
            {
                outcome = await _context.Payments
                        .Where(m => m.DuesId == DueType && m.Month == month && m.MemberId == BelieverId && m.Year == year && m.DeptId == deptId && m.PaymentSatus == "PAID")
                        .CountAsync();
            }
            else
            {
                outcome = await _context.Payments
                         .Where(m => m.DuesId == DueType && m.MemberId == BelieverId && m.DeptId == deptId)
                         .CountAsync();
            }
            return outcome;
        }

        public async Task<int>  InsertDuesPayment(int deptId,PaymentModelDto modelDto)
        {
            modelDto.Month = (modelDto.Month == 0) ? DateTime.Now.Month : modelDto.Month;
            modelDto.EntryDate = DateTime.Today;
            modelDto.DeptId = deptId;
            Payment model = _mapper.Map<PaymentModelDto, Payment>(modelDto);
            try
            {

                if (model.DuesId == 4)
                {
                    var query = await _context.Payments.Where(m => m.MemberId == model.MemberId 
                    && m.DeptId == deptId && m.Month == model.Month 
                    && m.Year == model.Year && m.DuesId == model.DuesId).FirstOrDefaultAsync();
                    if (query != null)
                    {
                       query.Amount =  model.Amount;
                        query.EntryDate = model.EntryDate;
                        query.PaymentDate = model.PaymentDate;
                        query.PaymentSatus = model.PaymentSatus;
                        query.TransactionType = model.TransactionType;
                      
                    }else{
                         _context.Payments.Add(model);
                    }

                }
                else
                {
                    _context.Payments.Add(model);
                   
                   
                }
                 return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
   
        public async Task<List<PaymentModelDto>>  GetExpenditureList(int deptId, DateTime? StartDate = null, DateTime? EndDate = null)
        {
            List<PaymentModelDto> _payment = new List<PaymentModelDto>();
           try
           {
                if (StartDate == null && EndDate == null)
                {
                    var query = await _context.Payments.Where(m => m.DeptId == deptId && m.TransactionType == "Debit").OrderBy(m => m.PaymentId).ToListAsync();
                    _payment = _mapper.Map<List<Payment>, List<PaymentModelDto>>(query);

                }
                else if (StartDate != null && EndDate != null)
                {

                    var query = await _context.Payments
                                .Where(m => m.DeptId == deptId && m.TransactionType == "Debit" && (m.PaymentDate >= StartDate && m.PaymentDate <= EndDate))
                                .OrderBy(m => m.PaymentId).ToListAsync();
                     _payment = _mapper.Map<List<Payment>, List<PaymentModelDto>>(query);
                }

                return _payment;
           }
           catch (System.Exception)
           {
               throw;
           }
        }
    }
}