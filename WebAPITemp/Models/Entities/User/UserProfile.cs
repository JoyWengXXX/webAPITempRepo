using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static WebAPITemp.Models.Enums;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 使用者簡介
    /// </summary>
    [Table("UserProfile")]
    public class UserProfile
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Key]
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// 聯絡地址
        /// </summary>
        [StringLength(100)]
        public string? Address { get; set; }

        /// <summary>
        /// 使用者姓名(英文)
        /// </summary>
        [StringLength(100)]
        public string? ENName { get; set; }

        /// <summary>
        /// 使用者姓名(中文)
        /// </summary>
        [StringLength(100)]
        public string? CNName { get; set; }

        /// <summary>
        /// 使用者性別
        /// </summary>
        [Column(TypeName = "tinyint")]
        public Gender? Gender { get; set; }

        /// <summary>
        /// 國籍
        /// </summary>
        [StringLength(30)]
        public string? Nationality { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        [StringLength(20)]
        public string? ContactNumber { get; set; }

        /// <summary>
        /// 其他連絡電話
        /// </summary>
        [StringLength(20)]
        public string? OtherNumber { get; set; }

        /// <summary>
        /// 電子信箱
        /// </summary>
        [StringLength(254)]
        public string? ContactEmail { get; set; }

        /// <summary>
        /// 其他電子信箱
        /// </summary>
        [StringLength(254)]
        public string? OtherEmail { get; set; }

        /// <summary>
        /// 相關證照
        /// </summary>
        [StringLength(255)]
        public string? Certifications { get; set; }

        /// <summary>
        /// 學歷
        /// </summary>
        [StringLength(255)]
        public string? Education { get; set; }

        /// <summary>
        /// 簡介
        /// </summary>
        [StringLength(2000)]
        public string? Introduction { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(200)]
        public string? Note { get; set; }

        /// <summary>
        /// 審查狀態
        /// </summary>
        [Required]
        [Column(TypeName = "tinyint")]
        public ReviewStatus ReviewStatus { get; set; }

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
        /// <summary>
        /// 更新者
        /// </summary>
        public int? UpdateBy { get; set; }
        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdateOn { get; set; }
    }
}
