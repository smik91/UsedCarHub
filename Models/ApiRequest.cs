namespace CarAPI.Models
{
    public class ApiRequest
    {
        public object Data { get; set; }

        public ApiRequest(object data)
        {
            Data = data;
        }
    }
}
