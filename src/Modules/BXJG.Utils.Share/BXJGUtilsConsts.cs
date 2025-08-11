using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share
{
    public partial class BXJGUtilsConsts
    {
        public const string ExcludeEntities = "__excludeEntities";
        public const string LocalizationSourceName = "BXJGUtils";
        public const int MaxDisplayNameLength = 128;
        public const int MaxDepth = 16;
        public const int CodeUnitLength = 5;
        public const int MaxCodeLength = 95;
        public const string EntityIsNotActive = "实体已禁用";
        /// <summary>
        /// 通用的扩展数据长度
        /// </summary>
        public const int ExtDataMaxLength = 4000;

        static BXJGUtilsConsts()
        {
            //  UploadTemp = @$"upload{Path.DirectorySeparatorChar}temp";
            UploadTempUrl = @$"upload/temp";
        }
        public const string UploadDir = "upload";

        /// <summary>
        /// upload\temp
        /// </summary>
        public const string UploadTemp = "temp";
        public static readonly string UploadTempUrl;

        #region 全局配置
        /// <summary>
        /// 全局配置
        /// </summary>
        public const string QuanjuPeizhi = nameof(QuanjuPeizhi);
        /// <summary>
        /// 服务器跟url
        /// </summary>
        public const string FuwuqiGen = nameof(FuwuqiGen);
        /// <summary>
        /// 上传根目录
        /// </summary>
        public const string Shangchuangen = nameof(Shangchuangen);
        #endregion

        public const string SettingKeyUploadGroup = "BXJGUtilsFileUploadGroup";
        public const string SettingKeyUploadSize = "BXJGUtilsFileUploadSize";
        public const int DefaultUploadMaxSize = 1024 * 5;
        public const string SettingKeyUploadType = "BXJGUtilsFileUploadType";
        //public const string DefaultUploadTypes = "jpg,jpeg,gif,png,doc,docx,rar,xlsx,xls,pdf";

        #region 验证码
        public const string SettingKeyCaptchaGroup = "CaptchaOptions";
        public const string CaptchaOptions = nameof(CaptchaOptions);
        public const string CaptchaOptions_ImageOption = "CaptchaOptions:ImageOption";

        public const string CaptchaOptions_IgnoreCase = "CaptchaOptions:IgnoreCase";
        public const string CaptchaOptions_CaptchaType = "CaptchaOptions:CaptchaType";
        public const string CaptchaOptions_ImageOption_FontFamily = "CaptchaOptions:ImageOption:FontFamily";
        public const string CaptchaOptions_ImageOption_Animation = "CaptchaOptions:ImageOption:Animation";
        //Actionj
        #endregion

        #region 文件
        public const int FileNameMaxLength = 36;
        public const int FileRealNameMaxLength = 100;
        public const int FileExtMaxLength = 20;
        public const int FileRelativePathMaxLength = 500;
        public const int FileThumbnailRelativePathMaxLength = 500;
        public const int FileContentTypeMaxLength = 50;
        #endregion

        #region EntityFileMaxLength
        public const int EntityFileEntityTypeMaxLength = 100;
        public const int EntityFileEntityIdMaxLength = 60;
        public const int EntityFileFileUrlMaxLength = 500;
        public const int EntityFilePropertyNameMaxLength = 100;
        public const int EntityFileExtMaxLength = 4000;
        #endregion
        //public const int ExtFieldldMaxLength = 200;
        //public const int ExtField2dMaxLength = 400;
        #region Tag
        public const int TagNameMaxLength = 100;
        public const int TagDisplayNameMaxLength = 100;
        #endregion

        #region 附件
        public const int AttachmentEntityTypeMaxLength = 100;
        public const int AttachmentEntityIdMaxLength = 100;
        public const int AttachmentEntityPropertyNameMaxLength = 100;
        #endregion

        #region 附件权限
        public const int AttachmentPermissionNameMaxLength = 100;
        #endregion

        /// <summary>
        /// 通过isettingmanager修改配置时，应触发此事件，以便通知配置提供程序刷新
        /// </summary>
        public const string OnAbpApplicationSettingsChanged = "OnAbpApplicationSettingsChanged";

        /// <summary>
        /// 前端获取AbpUserConfigurationDto作为全局状态
        /// 里面任何一个状态变化时，都触发此事件
        /// 事件处理程序中，通过全局的延迟覆盖执行器推送信号到所有在线用户
        /// 在线用户得到信号后，发起查询，而后更新本地缓存的状态。
        /// 这个对应整个应用级别的配置更新
        /// </summary>
        public const string OnApplicationStateChanged = "OnApplicationStateChanged";
        /// <summary>
        /// 租户级别的状态更新
        /// </summary>
        public const string OnTenantStateChanged = "OnTenantStateChanged";
        /// <summary>
        /// 用户级别的状态更新
        /// </summary>
        public const string OnUserStateChanged = "OnUserStateChanged";
    }
}