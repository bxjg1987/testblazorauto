using AutoMapper;
using BXJG.Equipment.EquipmentInfo;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment
{
    public class BXJGEquipmentMapProfile<TUser> : Profile
    {
        public BXJGEquipmentMapProfile()
        {
            #region 设备档案
            CreateMap<EquipmentInfoEntity, EquipmentInfoDto>();
            CreateMap<EquipmentInfoEditDto, EquipmentInfoEntity>();
            #endregion
        }
    }
}
