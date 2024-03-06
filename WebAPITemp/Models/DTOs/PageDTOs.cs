
namespace WebAPITemp.Models.DTOs
{
    public class MenuPageWithSectionDTO
    {
        /// <summary>
        /// 分類頁名稱
        /// </summary>
        public string pageSectionName { get; set; }
        /// <summary>
        /// 功能頁清單
        /// </summary>
        public List<MenuPage> menuPages { get; set; } = new List<MenuPage>();
    }
    public class MenuPage
    {
        /// <summary>
        /// 頁面標號
        /// </summary>
        public int pageID { get; set; }
        /// <summary>
        /// 頁面名稱
        /// </summary>
        public string pageName { get; set; }
        /// <summary>
        /// 上層功能頁編號
        /// </summary>
        public int parentPageID { get; set; }
        /// <summary>
        /// 頁面層數
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 子頁面清單
        /// </summary>
        public List<MenuPage> subPages { get; set; }
        /// <summary>
        /// 外部連結清單
        /// </summary>
        public List<OtherLink> otherLinks { get; set; }
    }
    public class OtherLink
    {
        /// <summary>
        /// 外部連結名稱
        /// </summary>
        public string linkName { get; set; }

        ///<summary>
        /// 外部連結網址
        /// </summary>
        public string linkUrl { get; set; }
    }

    public class PageDTO
    {
        /// <summary>
        /// 頁面標號
        /// </summary>
        public int pageID { get; set; }
        /// <summary>
        /// 頁面名稱
        /// </summary>
        public string pageName { get; set; }
        /// <summary>
        /// 上層功能頁編號
        /// </summary>
        public int parentPageID { get; set; }
        /// <summary>
        /// 頁面層數
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 分類頁名稱
        /// </summary>
        public string pageSectionName { get; set; }
    }
}
