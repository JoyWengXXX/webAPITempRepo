using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static WebAPITemp.Models.Enums;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 使用者資料
    /// </summary>
    [Table("User")]
    public class User
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        /// <summary>
        /// 使用者帳號(使用EMAIL)
        /// </summary>
        [Required]
        [StringLength(254)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 角色編號
        /// Role.RoleID
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 使用語言
        /// </summary>
        [Required]
        [Column(TypeName = "tinyint")]
        public UsingLanguage Language { get; set; }

        /// <summary>
        /// 使用者時區
        /// </summary>
        [Column(TypeName = "smallint")]
        public int? TimeZone { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Column(TypeName = "tinyint")]
        public UserStatus Status { get; set; }

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
