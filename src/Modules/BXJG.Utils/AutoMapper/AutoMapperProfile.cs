using Abp.Notifications;
using AutoMapper;
using BXJG.Utils.Files;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Share.Files;
using System.Text.Json;

namespace BXJG.Utils.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           CreateMap<FileEntity,DownloadFileResult>();
        }
    }
}
