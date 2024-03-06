using CommonLibrary.Dapper.Interfaces;
using Dapper;
using System.Data;
using WebAPITemp.DBContexts.Dapper;
using WebAPITemp.Models.DTOs;
using WebAPITemp.Models.Mapper.interfaces;
using WebAPITemp.Services.Interfaces;

namespace WebAPITemp.Services
{
    public class PageService : IPageService
    {
        private readonly IDbConnection _dbConnection;
        private readonly IPageMapper _pageMapper;

        public PageService(IRepository<ProjectDBContext, object> baseDapperDefault, IPageMapper pageMapper)
        {
            _dbConnection = baseDapperDefault.CreateConnection();
            _pageMapper = pageMapper;
        }

        /// <summary>
        /// 取回使用者系統選單
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<MenuPageWithSectionDTO>> GetMenuPages(int input)
        {
            var queryResult = await _dbConnection.QueryAsync<PageDTO>(
                        @"WITH RecursivePage AS
                            (
	                            SELECT 
                                    PG.[PageID], 
                                    PG.[PageName], 
                                    PG.[ParentPageID], 
                                    PG.[SectionID], 
                                    0 AS Level
	                            FROM [Page] PG
	                            WHERE [ParentPageID] = 0

	                            UNION ALL

	                            SELECT 
                                    PG.[PageID], 
                                    PG.[PageName], 
                                    PG.[ParentPageID], 
                                    PG.[SectionID], 
                                    RP.[Level] + 1
	                            FROM [Page] PG
	                            INNER JOIN RecursivePage RP ON PG.[ParentPageID] = RP.[PageID]
                            )
                        SELECT 
                            R.*, 
                            PS.[PageSectionName]
                        FROM RecursivePage R
                        LEFT JOIN 
                            [Permission] PM ON R.[PageID] = PM.[PageID]
                        LEFT JOIN 
                            [PageSection] PS ON R.[SectionID] = PS.[PageSectionID]
                        WHERE 
                            PM.[RoleID] = @input AND [IsEnable] = 1
                        ORDER BY [Level];", 
                        new { input });
            List<MenuPageWithSectionDTO> menuWithSection = new List<MenuPageWithSectionDTO>();
            var mapper = _pageMapper.ToMenuPageViewModel().CreateMapper();
            foreach (var section in queryResult.GroupBy(x => x.pageSectionName))
            {
                List<MenuPage> menuTree = new List<MenuPage>();
                foreach (var page in section)
                {
                    MenuPage mappedPage = mapper.Map<MenuPage>(page);
                    if (page.parentPageID == 0)
                    {
                        // 如果是最上層的頁面，直接加入
                        menuTree.Add(mappedPage);
                    }
                    else
                    {
                        // 尋找父節點並將該頁面加入其SubPages
                        FindAndAddToParent(mapper.Map<MenuPage>(page), menuTree);
                    }
                }
                menuWithSection.Add(new MenuPageWithSectionDTO() { pageSectionName = section.Key, menuPages = menuTree });
            }

            return menuWithSection;
        }

        /// <summary>
        /// 建立樹狀結構Menu
        /// </summary>
        /// <param name="page"></param>
        /// <param name="nodes"></param>
        private void FindAndAddToParent(MenuPage page, List<MenuPage> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.pageID == page.parentPageID)
                {
                    if (node.subPages == null)
                    {
                        node.subPages = new List<MenuPage>();
                    }
                    node.subPages.Add(page);
                    return;
                }
                else if (node.subPages != null)
                {
                    // 遞迴尋找父節點
                    FindAndAddToParent(page, node.subPages);
                }
            }
        }
    }
}
