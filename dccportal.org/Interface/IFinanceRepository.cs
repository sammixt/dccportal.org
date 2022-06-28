using dccportal.org.Dto;

namespace dccportal.org.Interface
{
    public interface IFinanceRepository
    {
        Task<int> AddPaymentType(DueDto dto);
        Task<int> CheckIfRecordExist(int deptId, int DueType = 0, int month = 0, int BelieverId = 0, string year = null);
        Task<bool> DeletePaymentType(string duesId);
        Task<List<DueDto>> Dues(int deptId);
        Task<int> EditPaymentType(DueDto dto);
        Task<decimal> GetAmountPerTransacton(int deptId, int DuesId);
        Task<string> GetDeptBalance(int deptId);
        Task<List<PaymentDto>> GetDuesPaymentRecordList(int deptId, string PaymentId = null, string PaymentType = null, string Month = null, string Year = null, string MemberId = null, DateTime? StartDate = null, DateTime? EndDate = null);
        Task<List<PaymentModelDto>> GetExpenditureList(int deptId, DateTime? StartDate = null, DateTime? EndDate = null);
        Task<List<MonthDto>> GetMonths();
        Task<string> GetWalletBalance(int deptId, string MemberId);
        Task<string> GetWalletBalance(int deptId, int MemberId);
        Task<int> InsertDuesPayment(int deptId, PaymentModelDto modelDto);
        Task<int> TopUpWallet(int deptId, WalletDto walletDto = null, PaymentModelDto Paymentmodel = null);
    }
}