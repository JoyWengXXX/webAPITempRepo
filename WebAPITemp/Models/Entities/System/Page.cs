using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 系統頁面
    /// </summary>
    [Table("Page")]
    public class Page
    {
        /// <summary>
        /// 功能頁編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageID { get; set; }
        /// <summary>
        /// 所屬系統編號
        /// System.SystemID
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "tinyint")]
        public int SystemID { get; set; }
        /// <summary>
        /// 功能頁名稱
        /// </summary>
        [Required]
        [StringLength(30)]
        public string PageName { get; set; }
        /// <summary>
        /// 上層功能頁編號
        /// </summary>
        [Required]
        [Column(TypeName = "smallint")]
        public int ParentPageID { get; set; }
        /// <summary>
        /// 頁面角色分類
        /// </summary>
        [StringLength(10)]
        public string? PageFor { get; set; }
        /// <summary>
        /// 功能頁分類編號
        /// </summary>
        [Column(TypeName = "smallint")]
        public int? SectionID { get; set; }
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
