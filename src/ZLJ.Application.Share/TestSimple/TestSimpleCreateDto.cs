using Abp.Application.Services.Dto;
using System.ComponentModel;
using BXJG.Common.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ZLJ.Application.Share.TestSimple
{
    //修改模型字段最少、新增模型次之、查询模型最多，这样后续字段调整时只需要修改编辑模型即可；查询模型一定时包含新增和修改的字段的，所以用这种继承也没毛病
    //新增往往是提交所有数据，修改时可能部分数据就不允许修改了

    /// <summary>
    /// 各应用新增 普通数据测试 的数据模型
    ///</summary>
    public class TestSimpleCreateDto : TestSimpleEditDto
    {
    }
}