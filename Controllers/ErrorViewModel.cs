namespace OrderManager.Controllers
{
    internal class ErrorViewModel(string errorMessage)
    {
        public required string RequestId { get; set; }
        private readonly string _errorMessage = errorMessage;

        public async Task<string> GetErrorMessageAsync()
        {
            // Simulate an asynchronous operation
            await Task.Delay(10);
            return _errorMessage;
        }

        public async Task<bool> ShowRequestIdAsync()
        {
            // Simulate an asynchronous operation
            await Task.Delay(10);
            return !string.IsNullOrEmpty(RequestId);
        }

        public async Task<string> ToStringAsync()
        {
            // Simulate an asynchronous operation
            await Task.Delay(10);
            return await GetErrorMessageAsync();
        }
    }
}