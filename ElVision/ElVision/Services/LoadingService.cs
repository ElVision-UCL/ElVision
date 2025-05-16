
namespace ElVision.Services
{
    public interface ILoadingService
    {
        event Action<bool, string?>? OnSpinnerChanged;
        bool IsVisible { get; }
        string? Message { get; }
        void Show(string? message = null, int millisecondsMessageDelay = 500);
        void Hide();
    }
    public class LoadingService : ILoadingService
    {
        public event Action<bool, string?>? OnSpinnerChanged;
        private CancellationTokenSource? delayCts;
        private bool isVisible;

        public bool IsVisible
        {
            get => isVisible;
            private set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnSpinnerChanged?.Invoke(isVisible, Message);
                }
            }
        }

        private string? message;
        public string? Message
        {
            get => message;
            private set
            {
                if (message != value)
                {
                    message = value;
                    OnSpinnerChanged?.Invoke(isVisible, message);
                }
            }
        }

        public void Show(string? message = null, int millisecondsMessageDelay = 500)
        {
            IsVisible = true;
            delayCts = new CancellationTokenSource();
            var token = delayCts.Token;

            Task.Delay(millisecondsMessageDelay, token).ContinueWith(t =>
            {
                if (!t.IsCanceled)
                {
                    Message = message;
                }
            }, token);
        }

        public void Hide()
        {
            delayCts?.Cancel();
            Message = null;
            IsVisible = false;
        }
    }
}
