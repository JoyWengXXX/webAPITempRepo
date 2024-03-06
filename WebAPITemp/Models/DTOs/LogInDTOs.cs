namespace WebAPITemp.Models.DTOs
{
    public class LogInReturnDTO 
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int userID { get; set; }
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string userAccount { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 角色編號
        /// </summary>
        public int roleID { get; set; }
        /// <summary>
        /// 角色編號
        /// </summary>
        public string roleName { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string logInToken { get; set; }
    }

    /// <summary>
    /// 登入資訊
    /// </summary>
    public class LoginQueryDTO
    {
        /// <summary>
        /// 帳號
        /// </summary>
        public string userID { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string password { get; set; }
    }

    public class RefreshTokenDTO
    {
        /// <summary>
        /// Token
        /// </summary>
        /// <value></value>
        public string token { get; set; }
        /// <summary>
        /// 使用者ID
        /// </summary>
        /// <value></value>
        public int userID { get; set; }
        /// <summary>
        /// 使用者帳號
        /// </summary>
        /// <value></value>
        public string userAccount { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        /// <value></value>
        public int roleID { get; set; }
        /// <summary>
        /// Token過期時間
        /// </summary>
        /// <value></value>
        public DateTime expireTime { get; set; }
    }
}
