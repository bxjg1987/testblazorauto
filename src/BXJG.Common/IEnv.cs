using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common
{
    /// <summary>
    /// asp.net core 3.x 有个 IWebHostEnv...的环境变量
    /// 某些类库不希望引用asp.net core的库，只是想获取当前web环境的数据，比如WebRoot属性，因此定义这个接口
    /// 有asp.net core项目实现类 并单例注入
    /// 利用abp的 自定义约定注册器 会让调用方使用起来更简单
    /// </summary>
    public interface IEnv
    {
        public string Root { get; }
    }
}
