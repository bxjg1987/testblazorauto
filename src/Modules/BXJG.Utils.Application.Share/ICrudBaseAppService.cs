using Abp.Application.Services.Dto;
using Abp.Application.Services;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share
{
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey,
                                         in TGetAllInput,
                                         in TCreateInput,
                                         in TUpdateInput,
                                         in TGetInput,
                                         in TDeleteInput> : IAsyncCrudAppService<TEntityDto,
                                                                                 TPrimaryKey,
                                                                                 TGetAllInput,
                                                                                 TCreateInput,
                                                                                 TUpdateInput,
                                                                                 TGetInput,
                                                                                 TDeleteInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        /// <summary>
        /// 批量删除，此操作是一个具体的操作（通过id删除实体），输入输出参数没必要泛型化，这样简单些。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<BatchOperationOutput<TPrimaryKey>> BatchDeleteAsync(BatchOperationInput<TPrimaryKey> input);

        // 其它批量操作不应该在基础抽象接口中定义。
    }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey,
                                         in TGetAllInput,
                                         in TCreateInput,
                                         in TUpdateInput,
                                         in TGetInput> : ICrudBaseAppService<TEntityDto,
                                                                             TPrimaryKey,
                                                                             TGetAllInput,
                                                                             TCreateInput,
                                                                             TUpdateInput,
                                                                             TGetInput,
                                                                             EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    { }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TUpdateInput"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey,
                                         in TGetAllInput,
                                         in TCreateInput,
                                         in TUpdateInput> : ICrudBaseAppService<TEntityDto,
                                                                                TPrimaryKey,
                                                                                TGetAllInput,
                                                                                TCreateInput,
                                                                                TUpdateInput,
                                                                                EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    { }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey,
                                         in TGetAllInput,
                                         in TCreateInput> : ICrudBaseAppService<TEntityDto,
                                                                                TPrimaryKey,
                                                                                TGetAllInput,
                                                                                TCreateInput,
                                                                                TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    { }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey,
                                         in TGetAllInput> : ICrudBaseAppService<TEntityDto,
                                                                                TPrimaryKey,
                                                                                TGetAllInput,
                                                                                TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    { }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface ICrudBaseAppService<TEntityDto,
                                         TPrimaryKey> : ICrudBaseAppService<TEntityDto,
                                                                            TPrimaryKey,
                                                                            PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
    { }
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    public interface ICrudBaseAppService<TEntityDto> : ICrudBaseAppService<TEntityDto, int>
        where TEntityDto : IEntityDto<int>
    { }
}