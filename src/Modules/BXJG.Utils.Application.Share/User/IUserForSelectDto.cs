using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /*
     * 当前项目是抽象，若用抽象类，就限制了子类必须定义我们定义的抽象
     * 但调用方的新增模型、查询模型 可能需要继承自己的编辑模型
     * 所以抽象层用接口，这样使用方更灵活
     */


    /// <summary>
    /// 下拉框可选的用户dto模型
    /// </summary>
    public interface IUserForSelectDto : IEntityDto<long>
    {
        //当前项目的抽象部分貌似不需要访问这些属性，所以顶部定义都无所谓

         //string EmailAddress { get; set; }

         //string PhoneNumber { get; set; }
         //string Name { get; set; }


         //string[] RoleNames { get; set; }
    }
}
