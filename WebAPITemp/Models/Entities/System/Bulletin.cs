using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPITemp.Models.Entities
{
    /// <summary>
    /// 布告欄
    /// </summary>
    [Table("Bulletin")]
    public class Bulletin
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 公告主題
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        /// <summary>
        /// 公告內容
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Article { get; set; }

        /// <summary>
        /// 顯示區域
        /// </summary>
        [Required]
        [Column(TypeName = "tinyint")]
        public int Area { get; set; }

        /// <summary>
        /// 上架時間
        /// </summary>
        [Required]
        public DateTime StartOn { get; set; }

        /// <summary>
        /// 下架時間
        /// </summary>
        [Required]
        public DateTime EndOn { get; set; }

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
