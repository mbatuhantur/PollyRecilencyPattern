using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.RecilenyPatterns.Helpers
{
  // 3 farklı senaryo circuit braker, timeout, retry
  public class HttpRecilencyHelper
  {
    private readonly ILogger<HttpRecilencyHelper> logger;


    public HttpRecilencyHelper(ILogger<HttpRecilencyHelper> logger)
    {
      this.logger = logger;
    }

    public IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(int retryCount, TimeSpan sleepDuration)
    {

      var response = HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(retryCount, x =>
      {

        return sleepDuration; // 2 saniyede bir isteği toplam 3 kez dene. 6 saniye ulaşamaz ise isteği deniyecek.

      }, onRetryAsync: async (outcome, timespan, retryCount, context) =>
      {
        this.logger.LogWarning($"İstek tekrar deneniyor {outcome.Result}");
      });


      return response;

    }

        public IAsyncPolicy<HttpResponseMessage> CreateTimeoutPolicy(TimeSpan timeout)
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(timeout, Timeout.TimeoutStrategy.Pessimistic, (context, timespan, task) =>
            {

                logger.LogInformation($"İstek timeout düştü");
                return Task.CompletedTask;
            });
        }

        public IAsyncPolicy<HttpResponseMessage> CreateCirciutBrakerPolicy(int errorCount, TimeSpan timeOfBreak)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError(). // 5xx hata kodları ve 408 hata kodu için
                Or<Exception>() // kendi e
                .CircuitBreakerAsync(errorCount, timeOfBreak, 
                onBreak: async (exception, duration) =>
                {
                    logger.LogInformation($"Hizmet kesinti kısmı");
                    await Task.CompletedTask;

                }, onReset: () =>
                {
                    logger.LogInformation("Hizmete devam et. Burada timeOfBreak kadar beklendi sonra hizmet kesintisi" +
                        "bitti. Circuit Braker açık konuma geçti. Taki bir daha hata olana kadar açık kalacak");
                });
        }

        //public IAsyncPolicy<HttpResponseMessage> CreateCircuitBreakerPolicy(int errorCount, TimeSpan timeOfBreak)
        //{
        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .Or<Exception>()
        //        .CircuitBreakerAsync(errorCount, timeOfBreak,
        //            onBreak: async (exception, duration) =>
        //            {
        //                logger.LogError($"Hizmet kesintiye kısmı "); // Daha detaylı loglama
        //                await Task.Delay(duration); // timeOfBreak süresince bekle
        //            },
        //            onReset: () =>
        //            {
        //                logger.LogInformation("Hizmete devam et. Burada timeOfBreak kadar beklendi sonra hizmet kesintisi" +
        //                "bitti. Circuit Braker açık konuma geçti. Taki bir daha hata olana kadar açık kalacak");
        //            });
        //}

    }

}
