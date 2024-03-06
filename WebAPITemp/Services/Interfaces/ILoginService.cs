using WebAPITemp.Models.DTOs;

namespace WebAPITemp.Services.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// 取回登入者資訊
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public Task<LogInReturnDTO> GetLoginUser(string UserID, string Password);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public Task LogOutUser(int UserID, string Token);

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<string> RefreshJWTToken(RefreshTokenDTO input);
    }
}
