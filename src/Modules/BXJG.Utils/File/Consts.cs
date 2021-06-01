using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BXJG.Utils.File
{
    public class Consts
    {
        static Consts()
        {
            UploadTemp = @$"upload{Path.DirectorySeparatorChar}temp";
        }
        public const string UploadDir = "upload";
      
       /// <summary>
       /// upload\temp
       /// </summary>
        public static readonly string UploadTemp;

        public const string SettingKeyUploadGroup = "BXJGUtilsFileUploadGroup";
        public const string SettingKeyUploadSize = "BXJGUtilsFileUploadSize";
        public const int DefaultUploadMaxSize = 1024 * 5;
        public const string SettingKeyUploadType = "BXJGUtilsFileUploadType";
        public const string DefaultUploadTypes = "jpg,jpeg,gif,png,doc,docx,rar,xlsx,xls,pdf";
    }
}
