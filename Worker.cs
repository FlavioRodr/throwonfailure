using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace framework
{
    public class Worker : IHostedService
    {
        private IFakeService fakeSvc;

        public Worker(IFakeService fakeSvc)
        {
            this.fakeSvc = fakeSvc;
        }

        [ThrowOnFailed]
        public void method()
        {
            Response r = null;
            try
            {
                Console.WriteLine("Get Response will get called.");
                r = this.fakeSvc.Get();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception thrown.");
                Console.WriteLine($"Exception msg: {ex.Message}. Stacktrace: ${ex.StackTrace}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.method();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}