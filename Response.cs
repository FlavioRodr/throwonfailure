using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace framework
{
    public enum MessageType
    {

        SystemError,
        Validation,
        BusinessError,
        Informational,
        Warning
    }

    public class Message
    {
        public MessageType Type { get; set; }

        public string MsgText { get; set; }
    }

    public class Response
    {
        private readonly MessageType[] errorMsgTypes = new MessageType[] {
                   MessageType.SystemError,
                   MessageType.BusinessError,
                   MessageType.Validation
               };

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

        private List<Message> messages = new List<Message>();
        public ReadOnlyCollection<Message> Messages
        {
            get
            {
                if (this.Messages is null)
                {
                    this.messages = new List<Message>();
                }

                return this.messages.AsReadOnly();
            }
            set
            {
                this.messages = value.ToList();
            }
        }

        public Response()
        {

        }

        [JsonConstructorAttribute]
        public Response(bool operationSuccess, List<Message> messages)
        {
            this.operationSuccess = operationSuccess;
            this.messages = messages;


            if (!this.operationSuccess && this.IsThrowException)
            {
                var msg = this.messages.FirstOrDefault();
                Exception ex = BuildException(msg);
                throw ex;
            }
        }

        /// <summary>
        /// Adds messages and throws an exception if ThrowOnFailedAttribute was applied
        /// <param name="msg"></param>
        public void AddMessage(params Message[] msg)
        {
            this.messages.AddRange(msg);
            var errorMsg = msg.FirstOrDefault(msg => this.errorMsgTypes.Contains(msg.Type));

            if (errorMsg is not null && this.IsThrowException) 
            {
                this.OperationSuccess = false;
                Exception ex = BuildException(errorMsg);
                throw ex;
            }
        }
        
        /// <summary>
        /// Whether ThrowOnFailedAttribute was applied higher in the call stack
        /// </summary>
        private bool IsThrowException
        {
            get
            {
                var stack = new StackTrace();

                var isThrowException = stack.GetFrames().AsEnumerable().Any(f =>
                {
                    ThrowOnFailedAttribute? attr = f.GetMethod().GetCustomAttributes(typeof(ThrowOnFailedAttribute), true).FirstOrDefault() as ThrowOnFailedAttribute;
                    return attr is not null;
                });

                return isThrowException;
            }
        }

        private static Exception BuildException(Message? msg) 
        {
            Exception ex = new Exception();
            if (msg is not null)
            {
                ex = new Exception(msg.MsgText);
            }
            return ex;
        }
    }
}