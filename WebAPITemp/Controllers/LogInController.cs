using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITemp.Models.DTOs;
using WebAPITemp.Services.Interfaces;

namespace WebAPITemp.Controllers
{
    public class LogInController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LogInController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        [HttpPost("Login"), AllowAnonymous]
        [ProducesResponseType(typeof(LogInReturnDTO), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> Login([FromBody] LoginQueryDTO Input)
        {
            LogInReturnDTO userInfo = await _loginService.GetLoginUser(Input.userID, Input.password);
            return Ok(userInfo);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [Authorize]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> Logout()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = Int32.Parse(User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            await _loginService.LogOutUser(userId, token);
            return Ok("已登出");
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "IgnoreExpiration")]
        [HttpPost("RefreshJWTToken")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 401)]
        public async Task<IActionResult> RefreshJWTToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            string userAccount = User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            int roleId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "role")?.Value);
            var expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(User.Claims.FirstOrDefault(x => x.Type == "exp").Value));
            RefreshTokenDTO input = new RefreshTokenDTO
            {
                token = token,
                userID = userId,
                userAccount = userAccount,
                roleID = roleId,
                expireTime = expiration.LocalDateTime
            };

            string newToken = await _loginService.RefreshJWTToken(input);
            return Ok(newToken);
        }
    }
}
