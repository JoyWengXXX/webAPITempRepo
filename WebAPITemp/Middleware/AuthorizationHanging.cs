using Microsoft.AspNetCore.Mvc;
using WebAPITemp.Models.Entities;
using Newtonsoft.Json;
using WebAPITemp.DBContexts.Dapper;
using System.Linq.Expressions;
using CommonLibrary.Dapper.Interfaces;

namespace WebAPITemp.Middleware
{
    /// <summary>
    /// 處理Exception發生時的步驟
    /// 掛在MiddleWare中
    /// </summary>
    public class AuthorizationHandler : ControllerBase
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationHandler> _logger;
        private readonly IRepository<ProjectDBContext, LogInRecord> _baseDapper;

        public AuthorizationHandler(RequestDelegate next, ILogger<AuthorizationHandler> logger, IRepository<ProjectDBContext, LogInRecord> baseDapperDefault)
        {
            _next = next;
            _logger = logger;
            _baseDapper = baseDapperDefault;
        }

        public async Task Invoke(HttpContext context)
        {
            // 取得Request的Header中的JWT TOKEN
            string Token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(Token))
            {
                Expression<Func<LogInRecord, object>> selected = e => new { e.Token, e.IsEnable };
                Expression<Func<LogInRecord, bool>> whereConditions = e => e.Token == Token;
                LogInRecord? logInRecord = await _baseDapper.GetExistedData(selected, whereConditions);
                var response = context.Response;
                response.ContentType = "application/json";
                ObjectResult result;
                if (logInRecord != null && logInRecord.IsEnable)
                {
                    await _next(context);
                }
                else
                {
                    result = Unauthorized("Token失效");
                    response.StatusCode = 401;
                    await response.WriteAsync(JsonConvert.SerializeObject(result.Value));
                }
            }
            else
            {
                await _next(context);
                return;
            }
        }
    }
}
