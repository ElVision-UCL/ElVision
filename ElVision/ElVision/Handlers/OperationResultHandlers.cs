using ElVision.Services;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using ElVisionLibrary.Models.Utilities;
using ElVisionLibrary.Models.ElPris;

namespace ElVision.Handlers
{
    public static class OperationResultHandlers
    {
        public static T GetData<T>(OperationResult operationResult)
        {
            var operationResultData = operationResult as OperationResult<T>;
            return operationResultData.Data;
        }

        public static void HandleUnsuccesful(OperationResult operationResult, INotificationService notificationService)
        {
            if (operationResult.StatusCode == HttpStatusCode.BadRequest)
            {
                if (operationResult.ModelStateErrors is not null)
                {
                    string combinedErrors = string.Join(Environment.NewLine, operationResult.ModelStateErrors.SelectMany(kvp => kvp.Value.Select(error => $"{kvp.Key}: {error}")));
                    var combinedErrorMessage = operationResult.ErrorMessage + Environment.NewLine + combinedErrors;

                    notificationService.Add(new NotificationModel(combinedErrorMessage, NotificationType.Error));
                }
                else
                {
                    notificationService.Add(new NotificationModel(operationResult.ErrorMessage, NotificationType.Error));
                }
            }
            else if (operationResult.StatusCode == HttpStatusCode.InternalServerError)
            {
                if (operationResult.ProblemDetails is not null)
                {
                    notificationService.Add(new NotificationModel($"{operationResult.ProblemDetails.Title}\n{operationResult.ProblemDetails.Detail}", NotificationType.Error));
                }
                else
                {
                    notificationService.Add(new NotificationModel(operationResult.ErrorMessage, NotificationType.Error));
                }
            }
            else if (operationResult.StatusCode == HttpStatusCode.NotFound)
            {
                notificationService.Add(new NotificationModel(operationResult.ErrorMessage, NotificationType.Error));
            }
            else if (operationResult.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                notificationService.Add(new NotificationModel(operationResult.ErrorMessage, NotificationType.Error));
            }
            else
            {
                if (operationResult.ErrorMessage is not null)
                {
                    notificationService.Add(new NotificationModel(operationResult.ErrorMessage, NotificationType.Error));
                }
                else
                {
                    notificationService.Add(new NotificationModel("Not sure what happened there my friend, my bad :( forgive me!", NotificationType.Error));
                }
            }
        }
    }
}
