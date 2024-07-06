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
        public const string UploadTemp= "temp";
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
    }
}