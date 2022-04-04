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
        public void method1()
        {
            Response r;
            try
            {
                Console.WriteLine("GetsFaultyResponseFromApi will get called.");
                r = this.fakeSvc.GetsFaultyResponseFromApi();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception thrown.");
                Console.WriteLine($"Exception msg: {ex.Message}. Stacktrace: ${ex.StackTrace}");
            }
        }

        [ThrowOnFailed]
        public void method2() 
        {
            Response r = new Response();

            try
            {
                Console.WriteLine("GetsFaltyResponse will get called.");
                var intermediateResponse = this.fakeSvc.GetsFaltyResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown.");
                Console.WriteLine($"Exception msg: {ex.Message}. Stacktrace: ${ex.StackTrace}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //this.method1();
            this.method2();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}