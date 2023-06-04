namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statuscode,string? message=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStatusCode(statuscode);

        }

        private string? GetDefaultMessageForStatusCode(int statuccode)
        {
            return statuccode switch
            {
                400 => "Bad Request",
                401 => "UnAuthorized",
                404 => "Resource NotFound",
                500 => "Errors Ead To Anger",
                _ => null

            };
        }
    }
}
