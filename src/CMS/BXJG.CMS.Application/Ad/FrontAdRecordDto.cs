using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Ad
{
    public class FrontAdPositionDto
    {
        public long     AdPositionId { get; set; }
    
        public int      AdPositionWidth { get; set; }
      
        public int      AdPositionHeight { get; set; }
     
        public List<FrontAdControlDto> Controls { get; set; }
    }

    public class FrontAdControlDto 
    {
        public long AdControlId { get; set; }
   
        public AdControlType AdControlType { get; set; }
    
        public dynamic ExtensionData { get; set; }
        
        public List<FrontAdDto> Ads { get; set; }
    }
    
    public class FrontAdDto 
    {
        public long RecordId { get; set; }
        
        public long AdId { get; set; }

        public string Title { get; set; }
        
        public AdType AdType { get; set; }
        
        public string Content { get; set; }
        
        public string Url { get; set; }
        
        public int SortIndex { get; set; }
    }
}
