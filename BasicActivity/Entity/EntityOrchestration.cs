using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasicActivity.Entity
{
    public static class EntityOrchestration
    {
        [FunctionName("InitializeLoan")]
        public static void InitializeLoan(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<object>() as dynamic;
            var account = input.account.Value as string;
            var amount = (decimal)input.amount.Value;
            var loan = context.CreateEntityProxy<ILoan>(account);
            loan.InitializeLoan(amount);
        }

        [FunctionName("AddTransaction")]
        public static void AddTransaction(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<object>() as dynamic;
            var account = input.account.Value as string;
            var amount = (decimal)input.amount.Value;
            var loan = context.CreateEntityProxy<ILoan>(account);
            loan.AddTransaction(new LoanTransaction {  Amount=amount, TypeOfTransaction = TransactionType.Repay,TransactionTimeStamp= context.CurrentUtcDateTime});
        }
        [FunctionName("ListTransaction")]
        public static Task<List<LoanTransaction>> ListTransaction(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<object>() as dynamic;
            var account = input.account.Value as string;
            var loan = context.CreateEntityProxy<ILoan>(account);
            return loan.GetTransactionHistory();
        }


        [FunctionName("GetBalance")]
        public static async Task<decimal> GetBalance(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var input = context.GetInput<object>() as dynamic;
            var account = input.account.Value as string;
            var loan = context.CreateEntityProxy<ILoan>(account);
            return await loan.GetBalance();
        }


    }
}