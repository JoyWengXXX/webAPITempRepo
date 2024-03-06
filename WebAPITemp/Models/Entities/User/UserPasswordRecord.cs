using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 使用者密碼紀錄
    /// </summary>
    [Table("UserPasswordRecord")]
    public class UserPasswordRecord
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Key]
        [Required]
        public int UserID { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        [Key]
        [Required]
        [StringLength(64)]
        public string Password { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
        [Key]
        [Required]
        public bool IsEnable { get; set; }
        /// <summary>
        /// 建立者
        /// </summary>
        [Required]
        public int CreateBy { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        public DateTime CreateOn { get; set; }
    }
}
