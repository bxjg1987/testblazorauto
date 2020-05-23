using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    public class FrontAdPositionDto:EntityDto<long>
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public List<FrontAdControlDto> Controls { get; set; }
    }

    public class FrontAdControlDto : EntityDto<long>
    {
        //public long AdControlId { get; set; }

        public AdControlType AdControlType { get; set; }

        public dynamic ExtensionData { get; set; }

        public List<FrontAdRecordDto> Ads { get; set; }
    }

    public class FrontAdRecordDto
    {
        public long Id { get; set; }

        public long AdId { get; set; }

        public string AdTitle { get; set; }

        public AdType AdAdType { get; set; }

        public string AdContent { get; set; }

        public string AdUrl { get; set; }

        public int AdSortIndex { get; set; }
    }
}
