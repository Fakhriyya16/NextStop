﻿
namespace Service.Helpers
{
    public class AccountManagementResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<string>? Errors { get; set; }
    }
}
