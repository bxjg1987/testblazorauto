using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Feedback
{
    public class FeedbackCondition :  IHaveKeywords
    {
        public string? Keywords { get; set; }
    }
}
