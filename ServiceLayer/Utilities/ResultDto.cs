using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Utilities
{
    public class ResultDto<T> where T : class
    {
        public ResultDto(bool result, string? message, T? data)
        {
            Message = message;
            Result = result;
            Data = data;
        }

        public bool Result { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
