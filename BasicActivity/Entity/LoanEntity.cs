using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicActivity
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Loan : ILoan
    {
        public decimal loanAmount;


        public List<LoanTransaction> transactionHistory = new List<LoanTransaction>();

        public void InitializeLoan(decimal loanAmount)
        {
            this.loanAmount = loanAmount;
        }

        public void AddTransaction(LoanTransaction transaction)
        {
            transactionHistory.Add(transaction);
        }
        public Task<decimal> GetBalance()
        {
            decimal balance = loanAmount;
            foreach (var t in transactionHistory.OrderBy(t => t.TransactionTimeStamp))
            {
                switch (t.TypeOfTransaction)
                {
                    case TransactionType.Repay:
                        balance -= t.Amount;
                        break;
                    case TransactionType.Borrow:
                        balance += t.Amount;
                        break;

                }

            }
            return Task.FromResult(balance);
        }


        public Task<List<LoanTransaction>> GetTransactionHistory()
        {
            return Task.FromResult(this.transactionHistory);
        }



        [FunctionName(nameof(Loan))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
        {

            return ctx.DispatchAsync<Loan>();

        }

    }
}
