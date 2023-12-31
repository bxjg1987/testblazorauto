
using BXJG.Utils.Application.Share;

namespace ZLJ.Application.Share.Post
{
    public interface IPostAppService : ICrudBaseAppService<PostDto, int, PagedAndSortedResultRequest<PagedPostResultRequestDto>, CreatePostDto, PostEditDto>
        
    {
        //Task<ListResultDto<PermissionDto>> GetAllPermissions();

        //Task<GetPostForEditOutput> GetPostForEdit(EntityDto input);

        //Task<IList<PostDto>> GetPostsAsync(GetPostsInput input);
        //Task<IEnumerable<int>> DeleteBatchAsync(params int[] input);
    }
}
