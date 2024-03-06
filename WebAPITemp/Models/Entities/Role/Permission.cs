using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 權限資料
    /// </summary>
    [Table("Permission")]
    public class Permission
    {
        /// <summary>
        /// 角色權限編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "smallint")]
        public int RoleID { get; set; }
        /// <summary>
        /// 功能頁編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "smallint")]
        public int PageID { get; set; }
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
