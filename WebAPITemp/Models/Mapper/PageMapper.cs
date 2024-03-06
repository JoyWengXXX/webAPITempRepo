using AutoMapper;
using WebAPITemp.Models.DTOs;
using WebAPITemp.Models.Mapper.interfaces;

namespace WebAPITemp.Models.Mapper
{
    public class PageMapper : IPageMapper
    {
        public MapperConfiguration ToMenuPageViewModel()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<PageDTO, MenuPage>()
                                                .ForMember(x => x.pageID, y => y.MapFrom(o => o.pageID))
                                                .ForMember(x => x.level, y => y.MapFrom(o => o.level))
                                                .ForMember(x => x.pageName, y => y.MapFrom(o => o.pageName))
                                                .ForMember(x => x.parentPageID, y => y.MapFrom(o => o.parentPageID))
                                                .ForMember(x => x.subPages, y => y.Ignore())
                                                .ForMember(x => x.otherLinks, y => y.Ignore()));
        }
    }
}
