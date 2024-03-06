using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommonLibrary.Authorization
{
    /// <summary>
    /// 設定JWT驗證/授權
    /// 參考:https://ithelp.ithome.com.tw/articles/10299318
    /// </summary>
    public static class JWTAuthorizationSetting
    {
        //設定JWT Token的設定
        public static void JWTSetting(this WebApplicationBuilder builder)
        {
            //清除預設映射
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //註冊JwtHelper
            builder.Services.AddSingleton<JwtHelper>();
            //使用選項模式註冊
            builder.Services.Configure<JwtSettingsOptions>(
                builder.Configuration.GetSection("JwtSettings"));
            //設定認證方式
            builder.Services
              //使用bearer token方式認證並且token用jwt格式
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options => {
                  // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                  options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      // 可以讓[Authorize]判斷角色
                      NameClaimType = ClaimTypes.NameIdentifier,
                      RoleClaimType = ClaimTypes.Role,
                      // 預設會認證發行人
                      ValidateIssuer = true,
                      ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
                      // 不認證使用者
                      ValidateAudience = false,
                      // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                      ValidateIssuerSigningKey = true,
                      // 一般我們都會驗證 Token 的有效期間
                      ValidateLifetime = true,
                      // 簽章所使用的key
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
                  };
              });
            //設定認證方式(不驗證過期)
            builder.Services
              //使用bearer token方式認證並且token用jwt格式
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer("IgnoreExpiration", options => {
                  // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                  options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      // 可以讓[Authorize]判斷角色
                      NameClaimType = ClaimTypes.NameIdentifier,
                      RoleClaimType = ClaimTypes.Role,
                      // 預設會認證發行人
                      ValidateIssuer = true,
                      ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
                      // 不認證使用者
                      ValidateAudience = false,
                      // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                      ValidateIssuerSigningKey = true,
                      // 一般我們都會驗證 Token 的有效期間
                      ValidateLifetime = false,
                      // 簽章所使用的key
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
                  };
              });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("VerifyExpiration", policy =>
                {
                    policy.AuthenticationSchemes.Add("VerifyExpiration");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("IgnoreExpiration", policy =>
                {
                    policy.AuthenticationSchemes.Add("IgnoreExpiration");
                    policy.RequireAuthenticatedUser();
                });
            });
        }
    }

    public class JwtHelper
    {
        private readonly JwtSettingsOptions settings;

        public JwtHelper(IOptionsMonitor<JwtSettingsOptions> settings)
        {
            //注入appsetting的json
            this.settings = settings.CurrentValue;
        }

        /// <summary>
        /// 產生JWT Token
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GenerateToken(int userID, string userName, int roleID)
        {
            //發行人
            var issuer = settings.Issuer;
            //加密的key，拿來比對jwt-token沒有
            var signKey = settings.SignKey;
            //建立JWT - Token
            var token = JwtBuilder.Create()
                        //所採用的雜湊演算法
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        //加密key
                        .WithSecret(signKey)
                        //角色
                        .AddClaim(ClaimTypes.Role, roleID)
                        //JWT ID
                        .AddClaim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        //發行人
                        .AddClaim(JwtRegisteredClaimNames.Iss, issuer)
                        //使用對象名稱
                        .AddClaim(JwtRegisteredClaimNames.Sub, userID)
                        //過期時間
                        .AddClaim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddHours(settings.ExpireTimeInHour).ToUnixTimeSeconds())
                        //此時間以前是不可以使用
                        .AddClaim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //發行時間
                        .AddClaim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        //使用者全名
                        .AddClaim(JwtRegisteredClaimNames.Name, userName)
                        //使用者角色ID
                        .AddClaim("role", roleID)
                        //進行編碼
                        .Encode();
            return token;
        }
    }
    //將appsetting轉為強行別所使用
    public class JwtSettingsOptions
    {
        public string Issuer { get; set; } = "";
        public string SignKey { get; set; } = "";
        public int ExpireTimeInHour { get; set; } = 0;
    }
}
