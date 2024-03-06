using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 系統
    /// </summary>
    [Table("System")]
    public class System
    {
        /// <summary>
        /// 系統編號
        /// </summary>
        [Key]
        [Required]
        [Column(TypeName = "tinyint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemID { get; set; }
        /// <summary>
        /// 系統名稱
        /// </summary>
        [Required]
        [StringLength(20)]
        public string SystemName { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(50)]
        public string? Memo { get; set; }
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
