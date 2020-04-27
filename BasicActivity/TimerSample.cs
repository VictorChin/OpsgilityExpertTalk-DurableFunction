using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace BasicActivity
{

    //This sample demonstrate:
    //1. How to use context.CreateTimer to start durable timer
    //2. How to Chain multile functions and execute them sequentially.
    public static class TimerSample
    {
        [FunctionName("TimerSample")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            var cts = new CancellationTokenSource().Token;
           
            outputs.Add(await context.CallActivityAsync<string>("TimerSample_Hello", "Tokyo"));
            outputs.Add($"Tokyo Finished at {context.CurrentUtcDateTime.ToLongTimeString()}");
            Task delayTimer = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(30),cts);
            outputs.Add($"30 second timer start at {context.CurrentUtcDateTime.ToLongTimeString()}");

            await delayTimer;

            outputs.Add($"Timer Expired, Seattle about to start at {context.CurrentUtcDateTime.ToLongTimeString()}");
            outputs.Add(await context.CallActivityAsync<string>("TimerSample_Hello", "Seattle"));
            outputs.Add($"Seattle Finished at {context.CurrentUtcDateTime.ToLongTimeString()}");

            outputs.Add($"London about to start at {context.CurrentUtcDateTime.ToLongTimeString()}");
            outputs.Add(await context.CallActivityAsync<string>("TimerSample_Hello", "London"));
            outputs.Add($"London Finished at {context.CurrentUtcDateTime.ToLongTimeString()}");
            
            return outputs;
        }

        [FunctionName("TimerSample_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

       
    }
}