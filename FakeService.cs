using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace framework
{
    public class FakeService : IFakeService
    {

        public FakeService() { }

        public Response? Get()
        {
            var json = "{ operationSuccess: false, messages: ['a', 'b']}";
            var r = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(json);
            return r;
        }
    }
}