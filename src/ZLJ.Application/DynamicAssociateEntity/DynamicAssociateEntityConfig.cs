using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;

namespace ZLJ.DynamicAssociateEntity
{
    public class DynamicAssociateEntityConfig
    {
        public static IEnumerable<DynamicAssociateEntityDefine> GetDefines() => new DynamicAssociateEntityDefine[]
            {
                new DynamicAssociateEntityDefine
                {
                    Name="Product",
                    DisplayName = "商品".ZLJLI(),
                    ServiceType= typeof(DynamicAssociateProductService),
                    Fields = new DynamicAssociateEntityDefineField[]
                    {
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="id".ZLJLI(),
                            IsDisplayField=true,
                            IsKey=true,
                            Name="Id"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="商品名称".ZLJLI(),
                            IsDisplayField=true,
                            Name="Name"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="单位".ZLJLI(),
                            IsDisplayField=true,
                            Name="Unit"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="规格".ZLJLI(),
                            IsDisplayField=true,
                            Name="Size"
                        }
                    }
                },
                new DynamicAssociateEntityDefine
                {
                    Name="Equipment",
                    DisplayName = "设备".ZLJLI(),
                    ServiceType= typeof(DynamicAssociateEquipmentService),
                    Fields= new DynamicAssociateEntityDefineField[]
                    {
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="id".ZLJLI(),
                            IsDisplayField=true,
                            IsKey=true,
                            Name="Id"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="设备名称".ZLJLI(),
                            IsDisplayField = true,
                            Name = "Name"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName = "单位".ZLJLI(),
                            IsDisplayField = true,
                            Name ="Unit"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="规格".ZLJLI(),
                            IsDisplayField =true,
                            Name = "Size"
                        }
                    }
                }
            };
    }
}
