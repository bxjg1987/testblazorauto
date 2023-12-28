using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Utils.GeneralTree;
using BXJG.Common.Dto;
using ZLJ.App.Admin.Roles.Dto;
using ZLJ.App.Admin.Post.Dto;
using BXJG.Utils;
using BXJG.Utils.Application.Share;

namespace ZLJ.App.Admin.Post
{
    public interface IPostAppService :
      
         ICrudBaseAppService<PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto>
        
    {
        //Task<ListResultDto<PermissionDto>> GetAllPermissions();

        //Task<GetPostForEditOutput> GetPostForEdit(EntityDto input);

        //Task<IList<PostDto>> GetPostsAsync(GetPostsInput input);
        //Task<IEnumerable<int>> DeleteBatchAsync(params int[] input);
    }
}
