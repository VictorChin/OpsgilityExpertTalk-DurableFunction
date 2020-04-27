using System.Collections.Generic;
using System.Threading.Tasks;

namespace BasicActivity
{
    public interface ILoan
    {
        void AddTransaction(LoanTransaction transaction);
        Task<decimal> GetBalance();
        Task<List<LoanTransaction>> GetTransactionHistory();
        void InitializeLoan(decimal loanAmount);
    }
}