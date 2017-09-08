using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enceladus.Api.Models
{
    class Message
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }

        public Message()
        { }

        public Message(Exception ex)
        {
            Type = MessageType.Error;
            Text = "Exception:  " + ex.Message;
            if (ex.InnerException != null)
                Text += Environment.NewLine + "   Inner Exception:  " + ex.InnerException;
        }

        public Message(ModelStateDictionary modelState)
        {
            Type = MessageType.Error;
            this.Text = ModelStateHelpers.AggregateErrors(modelState);
        }
        public Message(MessageType type, string text)
        {
            this.Type = type;
            this.Text = text;
        }
    }

    enum MessageType
    {
        Info,
        Error,
        Success
    }
}
