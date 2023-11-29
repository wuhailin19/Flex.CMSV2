using AutoMapper;
using Flex.Core.Extensions;
using Flex.Domain.Dtos.Menu;
using Flex.Domain.Entities;

namespace Flex.Domain.AutoMapper
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<SysMenu, MenuDto>()
                .ForMember(a => a.title, b => b.MapFrom(c => c.Name))
                .ForMember(a => a.id, b => b.MapFrom(c => c.Id))
                .ForMember(a => a.icode, b => b.MapFrom(c => c.Icode))
                .ForMember(a => a.parentid, b => b.MapFrom(c => c.ParentID))
                .ForMember(a => a.linkurl, b => b.MapFrom(c => c.LinkUrl))
                .ForMember(a => a.ismenu, b => b.MapFrom(c => c.isMenu))
                .ForMember(a => a.status, b => b.MapFrom(c => c.ShowStatus))
                .ForMember(a => a.isaspx, b => b.MapFrom(c => c.IsControllerUrl))
                .ForMember(a => a.@checked, b=>b.Ignore())
                .ForMember(a => a.className, b => b.MapFrom(c =>
                         (c.Icode.IsNullOrEmpty() || c.FontSort == "fontClass" || c.FontSort.IsNullOrEmpty())
                         ? "layui-icon"
                         : c.FontSort));
            CreateMap<SysMenu, MenuColumnDto>()
                .ForMember(a => a.Status, b => b.MapFrom(c => c.ShowStatus))
                .ForMember(a => a.FontSort, b => b.MapFrom(c =>
                         (c.Icode.IsNullOrEmpty() || c.FontSort == "fontClass" || c.FontSort.IsNullOrEmpty())
                         ? "layui-icon"
                         : c.FontSort));
        }
    }
}
