using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasicActivity
{
    public static class FanOut
    {
        [FunctionName("FanOut")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            List<Task> sites = new List<Task>();
            

            var responses = new List<Task<DurableHttpResponse>> {
               context.CallHttpAsync(HttpMethod.Get,new Uri("https://www.google.com")),
               context.CallHttpAsync(HttpMethod.Get,new Uri("https://www.amazon.com")),
               context.CallHttpAsync(HttpMethod.Get,new Uri("https://www.msn.com"))
            };
            await Task.WhenAll(responses.ToArray());
            foreach (var item in responses)
            {
                
                outputs.Add($"{item.Result.StatusCode}:{item.Result.Content.Length} bytes");
            }
           
            return outputs;
        }

       

    }
}