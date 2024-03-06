using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 角色資料
    /// </summary>
    [Table("Role")]
    public class Role
    {
        /// <summary>
        /// 角色權限編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }
        /// <summary>
        /// 角色權限名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string RoleName { get; set; }
        /// <summary>
        /// 是否為最高權限
        /// </summary>
        [Required]
        public bool IsHightestAdministrator { get; set; }
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
