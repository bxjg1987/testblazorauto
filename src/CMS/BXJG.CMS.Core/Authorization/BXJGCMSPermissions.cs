using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Authorization
{
    public class BXJGCMSPermissions
    {
        public const string BXJGCMS = "BXJGCMS";

        #region 文章
        public const string Article = "BXJGCMSArticle";
        public const string ArticleCreate = "BXJGCMSArticleCreate";
        public const string ArticleUpdate = "BXJGCMSArticleUpdate";
        public const string ArticleDelete = "BXJGCMSArticleDelete";
        #endregion

        #region 栏目
        public const string Column = "BXJGCMSColumn";
        public const string ColumnCreate = "BXJGCMSColumnCreate";
        public const string ColumnUpdate = "BXJGCMSColumnUpdate";
        public const string ColumnDelete = "BXJGCMSColumnDelete";
        #endregion

        #region 广告
        //广告
        public const string BXJGCMSAd = "BXJGCMSAd";
        public const string BXJGCMSAdCreate = "BXJGCMSAdCreate";
        public const string BXJGCMSAdUpdate = "BXJGCMSAdUpdate";
        public const string BXJGCMSAdDelete = "BXJGCMSAdDelete";
        //广告控件
        public const string BXJGCMSAdControl = "BXJGCMSAdControl";
        public const string BXJGCMSAdControlCreate = "BXJGCMSAdControlCreate";
        public const string BXJGCMSAdControlUpdate = "BXJGCMSAdControlUpdate";
        public const string BXJGCMSAdControlDelete = "BXJGCMSAdControlDelete";
        //广告位
        public const string BXJGCMSAdPosition = "BXJGCMSAdPosition";
        public const string BXJGCMSAdPositionCreate = "BXJGCMSAdPositionCreate";
        public const string BXJGCMSAdPositionUpdate = "BXJGCMSAdPositionUpdate";
        public const string BXJGCMSAdPositionDelete = "BXJGCMSAdPositionDelete";
        #endregion
    }
}
