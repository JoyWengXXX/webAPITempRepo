using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITemp.Models.DTOs;
using WebAPITemp.Services.Interfaces;

namespace WebAPITemp.Controllers
{
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        /// <summary>
        /// 取得權限下的選單
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPageMenus")]
        [Authorize]
        [ProducesResponseType(typeof(List<MenuPageWithSectionDTO>), 200)]
        public async Task<IActionResult> GetUserMenus()
        {
            int role = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "role").Value);
            var result = await _pageService.GetMenuPages(role);
            return Ok(result);
        }
    }
}
