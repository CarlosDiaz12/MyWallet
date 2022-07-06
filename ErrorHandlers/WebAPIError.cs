namespace MyWallet.ErrorHandlers
{
    public class WebAPIError
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
        public string Detail { get; set; }
        public WebAPIError(string message)
        {
            Message = message;
            IsError = true;
        }
    }
}
