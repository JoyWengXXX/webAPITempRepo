using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 使用者登入紀錄
    /// </summary>
    [Table("LogInRecord")]
    public class LogInRecord
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "tinyint")]
        public int SystemID { get; set; }
        /// <summary>
        /// 系統編號
        /// </summary>
        [Key]
        [Required]
        public int UserID { get; set; }
        /// <summary>
        /// 系統編號
        /// </summary>
        [Key]
        [Required]
        [StringLength(1000)]
        public string Token { get; set; }
        /// <summary>
        /// 是否啟用
        /// </summary>
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
