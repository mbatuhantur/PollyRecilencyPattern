using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.RecilencyPatterns.Helpers
{
    // 3 farklı senaryo
    // circiut braker
    // timeout
    // retry
    public static class HttpRecilencyHelper
    {
        public static IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(int retryCount, TimeSpan sleepDuration)
        {
            var response = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, x =>
            {
                return sleepDuration; // 2 saniyede bir isteği toplam 3 kez dene

            },onRetryAsync: async (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"İstek tekrar deneniyor ${outcome.Exception.Message}");
            });

            return response;
        }
    }

}
