using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.OU;
using BXJG.Utils.Application.Share.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.User
{
    /// <summary>
    /// 作为下拉框选择用户的数据模型
    /// 共用的、不包含敏感数据的
    /// </summary>
    public class UserSelectDto : EntityDto<long>//, IUserForSelectDto =
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string? Name { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Display(Name = "邮箱地址")]
        public string? EmailAddress { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public virtual IEnumerable<RoleSelectDto>? Roles { get; set; }
        /// <summary>
        /// 角色
        /// json反序列化时要使用<see cref="OusConverter"/>
        /// [JsonConverter(typeof(OusConverter<OUSelectDto>))]
        /// </summary>
        [Display(Name = "角色")]
        public virtual string? RolesText => Roles == null ? string.Empty : string.Join(',', Roles.Select(x => x.DisplayText));
        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        public virtual IEnumerable<IGeneralTree> Ous { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        //public virtual string? OusText => string.Empty;
        public virtual string? OusText => Ous == null ? string.Empty : string.Join(',', Ous.Select(x => x.DisplayName));
    }
    public class OusConverter<TConcrete> : JsonConverter<IEnumerable< IGeneralTree>>
    //where TConcrete : IGeneralTree
    {
        public override IEnumerable<IGeneralTree> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<IEnumerable<TConcrete>>(ref reader, options) as IEnumerable<IGeneralTree>;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<IGeneralTree> value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }

}
