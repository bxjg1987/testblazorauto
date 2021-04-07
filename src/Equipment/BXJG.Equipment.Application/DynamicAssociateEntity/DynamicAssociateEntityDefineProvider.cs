using BXJG.DynamicAssociateEntity;
using BXJG.Equipment.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment.DynamicAssociateEntity
{
    public class DynamicAssociateEntityDefineProvider : IDynamicAssociateEntityDefineProvider
    {
        public IEnumerable<DynamicAssociateEntityDefine> GetDefines(DynamicAssociateEntityDefineProviderContext context)
        {
            return new DynamicAssociateEntityDefine[]
            {
                new DynamicAssociateEntityDefine
                {
                    Name="equipment",
                    DisplayName = "设备".BXJGEquipmentL(),
                    ServiceType= typeof(DynamicAssociateEntityEquipmentInfoService),
                    ServiceType2= typeof(DynamicAssociateEntityEquipmentInfoService),
                    Fields= new DynamicAssociateEntityDefineField[]
                    {
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="id".BXJGEquipmentL(),
                            IsKey=true,
                            Name="id"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="设备名称".BXJGEquipmentL(),
                            IsDisplayField = true,
                            Name = "name"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName = "单位".BXJGEquipmentL(),
                            IsDisplayField = true,
                            Name ="unit"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="规格".BXJGEquipmentL(),
                            IsDisplayField =true,
                            Name = "size"
                        }
                    }
                }
            };
        }
    }
}
