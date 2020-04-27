using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BasicActivity
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register own factory
            builder.Services.AddSingleton<
                IDurableHttpMessageHandlerFactory,
                MyDurableHttpMessageHandlerFactory>();
        }
    }

    public class MyDurableHttpMessageHandlerFactory : IDurableHttpMessageHandlerFactory
    {
        public HttpMessageHandler CreateHttpMessageHandler()
        {
            // Disable TLS/SSL certificate validation (not recommended in production!)
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            };
        }
    }
}
