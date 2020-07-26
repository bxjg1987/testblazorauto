using AutoMapper;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.BaseInfo
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            #region 商城字典
            CreateMap<AdministrativeEditDto, AdministrativeEntity>();
            CreateMap<AdministrativeEntity, AdministrativeDto>();
            CreateMap<AdministrativeEntity, AdministrativeTreeNodeDto>().EntityToComboTree();
            CreateMap<AdministrativeEntity, AdministrativeCombboxDto>().EntityToCombobox();
            #endregion

        }
    }
}
