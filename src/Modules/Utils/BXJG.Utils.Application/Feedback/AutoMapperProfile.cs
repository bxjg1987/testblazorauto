using AutoMapper;
using BXJG.Utils.Application.Share.Feedback;
using BXJG.Utils.Application.Share.Files;
using BXJG.Utils.Feedback;
using BXJG.Utils.Files;
using BXJG.Utils.Share.Files;
using System.Collections.Generic;
using System.Text.Json;
namespace BXJG.Utils.Application.Feedback
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<FeedbackEntity, FeedbackDto>().ForMember(x => x.ExtensionData, x => x.Ignore());
            CreateMap<FeedbackEditDto, FeedbackEntity>();
        }
    }
}
