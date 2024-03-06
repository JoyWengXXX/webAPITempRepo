using Microsoft.AspNetCore.Mvc.Filters;
using WebAPITemp.Middleware;
using WebAPITemp.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using static WebAPITemp.Models.Enums;

namespace WebAPITemp.Filters
{
    /// <summary>
    /// 檢查API動作是否有符合設定角色權限
    /// </summary>
    public class ActionRoleFilter : Attribute, IAsyncActionFilter
    {
        private readonly ActionRole[] _roles;

        public ActionRoleFilter(ActionRole[] roles)
        {
            _roles = roles;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 取得 HttpContext
            var HttpContext = context.HttpContext;
            // 檢查是否存在 Authorization 標頭
            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                // 取得 Authorization 標頭的值
                var AuthorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
                // 檢查是否以 "Bearer " 開頭，表示是 JWT Token
                if (AuthorizationHeader.StartsWith("Bearer "))
                {
                    // 提取 Token
                    var Token = AuthorizationHeader.Substring(7);
                    // 解析 Token
                    var TokenHandler = new JwtSecurityTokenHandler();
                    var JWTToken = TokenHandler.ReadJwtToken(Token);
                    // 取得所需的資訊
                    int UserID = Convert.ToInt32(JWTToken.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
                    int Role = Convert.ToInt32(JWTToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value);
                    if (_roles.Contains((ActionRole)Role))
                    {
                        await next();
                    }
                    else
                    {
                        throw new AppException("權限不足");
                    }
                }
            }
        }
    }
}
