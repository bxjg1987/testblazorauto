using BXJG.CMS.Localization;
using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.CMS.DynamicAssociateEntity
{
    public class DynamicAssociateEntityDefineProvider : IDynamicAssociateEntityDefineProvider
    {
        public IEnumerable<DynamicAssociateEntityDefine> GetDefines(DynamicAssociateEntityDefineProviderContext context)
        {
            return new DynamicAssociateEntityDefine[]
            {
                new DynamicAssociateEntityDefine
                {
                    Name="column",
                    DisplayName = "栏目".BXJGCMSL(),
                    ServiceType= typeof(DynamicAssociateEntityColumnService),
                    Fields= new DynamicAssociateEntityDefineField[]
                    {
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName="id".BXJGCMSL(),
                            IsKey=true,
                            Name="id"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="名称".BXJGCMSL(),
                            IsDisplayField = true,
                            Name = "displayName"
                        },
                        new DynamicAssociateEntityDefineField
                        {
                            DislayName ="内容类型".BXJGCMSL(),
                            IsDisplayField = true,
                            Name = "contentTypeDisplayName"
                        }
                    },
                    Child = new DynamicAssociateEntityDefine
                    {
                        Name="article",
                        DisplayName = "文章".BXJGCMSL(),
                        ServiceType= typeof(DynamicAssociateEntityArticleService),
                        Fields= new DynamicAssociateEntityDefineField[]
                        {
                            new DynamicAssociateEntityDefineField
                            {
                                DislayName="id".BXJGCMSL(),
                                IsKey=true,
                                Name="id"
                            },
                            new DynamicAssociateEntityDefineField
                            {
                                DislayName ="标题".BXJGCMSL(),
                                IsDisplayField = true,
                                Name = "title"
                            },
                            new DynamicAssociateEntityDefineField
                            {
                                DislayName ="已发布".BXJGCMSL(),
                                IsDisplayField = true,
                                Name = "published"
                            }
                        }
                    }
                }
            };
        }
    }
}
