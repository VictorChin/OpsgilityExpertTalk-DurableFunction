using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

using Newtonsoft.Json.Linq;

using System.Threading;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicActivity
{
    public static class ClientFunction
    {
        [FunctionName("ClientFunctionHttp")]
        public static async Task<HttpResponseMessage> ClientFunctionHttp(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "activities/{activity}")] HttpRequestMessage req,
            [DurableClient] IDurableClient starter,
            string activity,
            ILogger log)
        {
            object eventData = await req.Content.ReadAsAsync<object>();

            string instanceId = await starter.StartNewAsync(activity,eventData);

            return starter.CreateCheckStatusResponse(req,instanceId);
        }
        [FunctionName("EntityHttp")]
        public static async Task<IActionResult> EntityFunctionHttp(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "entity/{account}/{action}/{amount?}")] HttpRequest req,
           [DurableClient] IDurableClient starter,
           string account,
           string action,
           decimal? amount,
           ILogger log)
        {
            string instanceId;
            switch (action)
            {
                case "InitializeLoan":
                  instanceId = await starter.StartNewAsync(action, new { account = account, amount = amount });
                    break;
                case "AddTransaction":
                    instanceId = await starter.StartNewAsync(action, new { account = account, amount = amount });
                    break;
                case "GetBalance":
                  instanceId = await starter.StartNewAsync(action, new { account = account});
                    break;
                case "ListTransaction":
                    instanceId = await starter.StartNewAsync(action, new { account = account });
                    break;
                default:
                    throw new Exception("Invalid Action");
            }
            
            //"InitializeLoan"
            //string instanceId = await starter.StartNewAsync(action, new { account = account, amount = amount });
            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName("GetLoanBalance")]
        public static async Task<IActionResult> GetLoanBalance(
       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LoanBalance/{account}")] HttpRequest req,
        [DurableClient] IDurableEntityClient client,
        string account)
        {
            
            var entityId = new EntityId(nameof(Loan), account);
            var stateResponse = await client.ReadEntityStateAsync<Loan>(entityId);
            if (!stateResponse.EntityExists) {
                return new NotFoundObjectResult($"There is no loan associated with {account}");
            };

            var loan = stateResponse.EntityState as Loan;
            IActionResult result = new OkObjectResult(
                new
                {
                    account = account,
                    balance = loan.GetBalance().Result,
                    transactions = loan.GetTransactionHistory().Result
                }
                );
            return result;

        }

    }
}
