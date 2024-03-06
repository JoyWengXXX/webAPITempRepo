using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAPITemp.Middleware
{
    /// <summary>
    /// 處理Exception發生時的步驟
    /// 掛在MiddleWare中
    /// </summary>
    public class ErrorHandler : ControllerBase
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ObjectResult result;
                //如果是自定義的錯誤，則回傳客製化的錯誤訊息
                if (error is AppException)
                {
                    result = BadRequest(error.Message);
                }
                else
                {
                    result = Problem("伺服器錯誤");
                    //記錄錯誤Log
                    _logger.LogError(error, error.Message);
                }
                await response.WriteAsync(JsonConvert.SerializeObject(result.Value));
            }
        }
    }


    //預設自定義Exception
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}
