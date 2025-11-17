


using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Dtos;
using ZLJ.Application.Common.Share;

namespace ZLJ.Application.Share.Post
{
    public interface IPostAppService : Common.Share.ICrudBaseAppService<PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, PostCreateDto, PostEditDto>

    {
        //Task<ListResultDto<PermissionDto>> GetAllPermissions();

        //Task<GetPostForEditOutput> GetPostForEdit(EntityDto input);

        //Task<IList<PostDto>> GetPostsAsync(GetPostsInput input);
        //Task<IEnumerable<int>> DeleteBatchAsync(params int[] input);
    }
}
