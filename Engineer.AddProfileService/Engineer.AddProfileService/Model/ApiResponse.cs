using System;

namespace Engineer.AddProfileService.Model
{
    public class ApiResponse
    {
        public DateTime TimeStamp { get; set; }
        public object Result { get; set; }
        public StatusResponse Status { get; set; }

        public ApiResponse()
        {
            TimeStamp = DateTime.UtcNow;
            Result = null;
            Status = new StatusResponse()
            {
                IsValid = true,
                Status = "SUCCESS",
                Message = string.Empty
            };
        }
    }

    public class StatusResponse
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public bool IsValid { get; set; }
    }
}
