using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace framework
{
    public class FakeService : IFakeService
    {

        public FakeService() { }

        public Response GetsFaultyResponseFromApi()
        {
            var json = "{ operationSuccess: false, messages: ['a', 'b']}";
            var r = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(json);
            return r;
        }

        public Response GetsFaltyResponse()
        {
            var errorResponse = new Response();

            if (true)  // pretend custom logic 
            {
                errorResponse.AddMessage(new Message { Type = MessageType.BusinessError, MsgText = "Some custom logic happened." });
            }

            return errorResponse;
        }
    }
}