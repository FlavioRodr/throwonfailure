using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;

namespace framework
{
    public class Response
    {
        private bool operationSuccess;
        public bool OperationSuccess
        {

            get
            {
                return this.operationSuccess;
            }
            set
            {
                this.operationSuccess = value;
            }
        }

        private List<string> messages;
        public List<string> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
            }
        }

        public Response()
        {

        }

        [JsonConstructorAttribute]
        public Response(bool operationSuccess, List<string> messages)
        {
            this.operationSuccess = operationSuccess;
            this.messages = messages;

            var stack = new StackTrace();

            if (!this.operationSuccess) 
            {
                var isThrowException = stack.GetFrames().AsEnumerable().Any(f =>
                {
                    var attr = f.GetMethod().GetCustomAttributes(typeof(ThrowOnFailedAttribute), true).FirstOrDefault() as ThrowOnFailedAttribute;

                    return attr is not null;
                });

                if (isThrowException)
                {
                    var msg = this.messages.FirstOrDefault();
                    msg = msg is not null ? msg : string.Empty;
                    var ex = new Exception(msg);
                    throw ex;
                }
            }
        }
    }
}