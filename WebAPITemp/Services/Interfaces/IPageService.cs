using WebAPITemp.Models.DTOs;

namespace WebAPITemp.Services.Interfaces
{
    public interface IPageService
    {
        Task<List<MenuPageWithSectionDTO>> GetMenuPages(int RoleID);
    }
}
