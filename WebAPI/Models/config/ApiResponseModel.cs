using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ApiResponseModel
    {
        public bool IsValid { get; set; }
        public string ErrorMsg { get; set; }
        public dynamic Data { get; set; }
        public ApiResponseModel Success(dynamic data)
        {
            return new ApiResponseModel()
            {
                IsValid = true,
                ErrorMsg = null,
                Data = data
            };
        }
        public ApiResponseModel Failed(string errorMsg)
        {
            return new ApiResponseModel()
            {
                IsValid = false,
                ErrorMsg = errorMsg,
                Data = null
            };
        }
    }
}
