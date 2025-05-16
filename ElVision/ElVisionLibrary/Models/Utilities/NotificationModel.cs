using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVisionLibrary.Models.Utilities
{
    public class NotificationModel
    {
        public string Message { get; set; }
        public NotificationType Type { get; set; }

        public NotificationModel(string message, NotificationType type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum NotificationType
    {
        Success,
        Error,
        Info,
        Warning
    }
}
