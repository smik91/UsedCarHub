namespace CarAPI.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; } 
        public string ErrorMessage { get; set; } 
        public object Data { get; set; } 

        public ApiResponse(object data)
        {
            Success = true;
            Data = data;
        }
        public ApiResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }
    }
}
