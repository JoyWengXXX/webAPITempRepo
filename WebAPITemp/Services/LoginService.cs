using Dapper;
using System.Data;
using WebAPITemp.DBContexts.Dapper;
using WebAPITemp.Models.DTOs;
using WebAPITemp.Models.Entities;
using WebAPITemp.Middleware;
using CommonLibrary.Authorization;
using CommonLibrary.Dapper.Interfaces;
using WebAPITemp.Services.Interfaces;

namespace WebAPITemp.Services
{
    public class LoginService : ILoginService
    {
        private readonly JwtHelper _jwtHelpers;
        private readonly IDbConnection _dbConnection;
        private readonly IRepository<ProjectDBContext, LogInRecord> _baseDapper_LogInRecord;
        
        public LoginService(JwtHelper jwtHelpers, IRepository<ProjectDBContext, LogInRecord> baseDapperDefault)
        {
            _jwtHelpers = jwtHelpers;
            _dbConnection = baseDapperDefault.CreateConnection();
            _baseDapper_LogInRecord = baseDapperDefault;
        }

        /// <summary>
        /// 取回登入者資訊
        /// </summary>
        /// <param name="userAccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<LogInReturnDTO> GetLoginUser(string userAccount, string password)
        {
            LogInReturnDTO? result = await _dbConnection.QuerySingleOrDefaultAsync<LogInReturnDTO>(
                        @"SELECT 
                            U.[UserID],
                            U.[UserAccount],
                            PP.[ENName] AS userName,
                            R.[RoleID],
                            R.[RoleName]
                        FROM [dbo].[User] U
                        INNER JOIN 
                            [UserPasswordRecord] UP ON U.[UserID] = UP.[UserID] AND UP.[IsEnable] = 1
                        LEFT JOIN 
                            [Role] R ON U.[RoleID] = R.[RoleID] AND R.[IsEnable] = 1
                        LEFT JOIN 
                            [UserProfile] PP ON U.[UserID] = PP.[UserID]
                        WHERE 
                            U.[UserAccount] = @UserAccount AND UP.[Password] = @Password AND U.[Status] = 1", 
                        new { userAccount, password });
            if (result == null)
                throw new AppException("帳號或密碼錯誤");
            else
            {
                result.logInToken = _jwtHelpers.GenerateToken(result.userID, result.userAccount, result.roleID);
                await _baseDapper_LogInRecord.CreateData(new LogInRecord
                {
                    SystemID = 1,
                    UserID = result.userID,
                    Token = result.logInToken,
                    IsEnable = true,
                    CreateBy = result.userID,
                    CreateOn = DateTime.Now
                });
            }
            return result;
        }

        public async Task LogOutUser(int userID, string token)
        {
            await _baseDapper_LogInRecord.UpdateData(x => x.IsEnable == false,
                                                     x => x.UserID == userID && 
                                                          x.Token == token && 
                                                          x.IsEnable == true);
        }

        public async Task<string> RefreshJWTToken(RefreshTokenDTO input)
        {
            var existedToken = await _baseDapper_LogInRecord.GetExistedData(x => new { x.Token }, 
                                                                            x => x.UserID == input.userID && 
                                                                                 x.Token == input.token && 
                                                                                 x.IsEnable == true);
            if (existedToken == null)
                throw new AppException("Token不存在");
            await LogOutUser(input.userID, input.token);
            string logInToken = _jwtHelpers.GenerateToken(input.userID, input.userAccount, input.roleID);
            await _baseDapper_LogInRecord.CreateData(new LogInRecord
            {
                SystemID = 1,
                UserID = input.userID,
                Token = logInToken,
                IsEnable = true,
                CreateBy = input.userID,
                CreateOn = DateTime.Now
            });
            return logInToken;
        }
    }
}
