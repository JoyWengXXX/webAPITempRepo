using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 系統頁面分類
    /// </summary>
    [Table("PageSection")]
    public class PageSection
    {
        /// <summary>
        /// 功能頁分類編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "smallint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageSectionID { get; set; }

        /// <summary>
        /// 功能頁分類名稱
        /// </summary>
        [Required]
        [StringLength(30)]
        public string PageSectionName { get; set; }

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
