using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.File
{
    public class Consts
    {
        public const string UploadDir = "upload";
        public const string UploadTemp = "upload\\temp";

        public const string SettingKeyUploadGroup = "BXJGUtilsFileUploadGroup";
        public const string SettingKeyUploadSize = "BXJGUtilsFileUploadSize";
        public const int DefaultUploadMaxSize = 1024 * 5;
        public const string SettingKeyUploadType = "BXJGUtilsFileUploadType";
        public const string DefaultUploadTypes = "jpg,jpeg,gif,png";
    }
}
