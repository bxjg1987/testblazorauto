using Abp.Configuration;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.Settings
{
    /// <summary>
    /// 设置定义和设置值的混合体
    /// </summary>
    public class SettingDto: SettingEditDto
    {
        //public string Name { get; set; }

        //public string Value { get; set; }
  

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A brief description for this setting.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Scopes of this setting.
        /// Default value: <see cref="SettingScopes.Application"/>.
        /// </summary>
        public SettingScopes Scopes { get; set; }

        /// <summary>
        /// Is this setting inherited from parent scopes.
        /// Default: True.
        /// </summary>
        public bool IsInherited { get; set; }

        /// <summary>
        /// Gets/sets group for this setting.
        /// </summary>
        public SettingDefinitionGroupDto Group { get; set; }

        /// <summary>
        /// Default value of the setting.
        /// </summary>
        public string DefaultValue { get; set; }

        ///// <summary>
        ///// Client visibility definition for the setting.
        ///// </summary>
        //public ISettingClientVisibilityProvider ClientVisibilityProvider { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this setting.
        /// </summary>
        public Dictionary<string,string> CustomData { get; set; }

        /// <summary>
        /// Is this setting stored as encrypted in the data source.
        /// Default: False.
        /// </summary>
        public bool IsEncrypted { get; set; }
    }
}
