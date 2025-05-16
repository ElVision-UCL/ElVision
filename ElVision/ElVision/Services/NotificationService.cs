using ElVisionLibrary.Models.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElVision.Services
{
    public interface INotificationService
    {
        event Action? OnNotificationAdded;
        SynchronizedCollection<NotificationModel> Notifications { get; }
        void Add(NotificationModel notification);
        void Remove(NotificationModel notification);
    }
    public class NotificationService : INotificationService
    {
        public event Action? OnNotificationAdded;

        public SynchronizedCollection<NotificationModel> Notifications { get; private set; } = [];

        public void Add(NotificationModel notification)
        {
            Notifications.Add(notification);
            OnNotificationAdded?.Invoke();

            if(notification.Type == NotificationType.Success)
            {
                Task.Delay(10000).ContinueWith(_ =>
                {
                    Remove(notification);
                    OnNotificationAdded?.Invoke();
                });
            }
        }

        public void AddUnexpectedError(string errorMessage)
        {
            Notifications.Add(new NotificationModel($"An unexpected error has occured: {errorMessage}", NotificationType.Error));
            OnNotificationAdded?.Invoke();
        }

        public void Remove(NotificationModel notification)
        {
            Notifications.Remove(notification);
            OnNotificationAdded?.Invoke();
        }
    }
}
