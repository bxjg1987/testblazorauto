using Abp.Application.Services.Dto;
using Abp.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share
{
    /// <summary>
    /// abp提供的crud接口未定义批量删除接口，这里定义了批量删除接口，它继承了abp的crud接口
    /// 其它批量操作不应该在基础抽象接口中定义，因为它们不是crud操作。
    /// 之所以加了泛型约束是因为abp提供的父接口要求约束
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
                                         in TDeleteInput> : BXJG.Utils.Application.Share.ICrudBaseAppService<TEntityDto,
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
        //这里可以定义此项目相关的、多应用共享的、公共的接口
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
                                                                                EntityDto<TPrimaryKey>>, BXJG.Utils.Application.Share.ICrudBaseAppService<TEntityDto,
                                                                                                                                                          TPrimaryKey,
                                                                                                                                                          TGetAllInput,
                                                                                                                                                          TCreateInput,
                                                                                                                                                          TUpdateInput>
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