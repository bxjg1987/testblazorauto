using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ZLJ.Application.Common.Share.Post;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZLJ.RCL.Components.Post
{
    public class CheckGroupPost : CheckboxGroup<string>
    {
        /// <summary>
        /// 专门给肉夹馍aop用的，你不该调用这个
        /// </summary>
        [Inject]
        public IServiceProvider Services { get; set; }
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }

        protected virtual HttpClient HttpClient => HttpClientFactory.CreateHttpClientCommon();
        [CascadingParameter]
        [Parameter]
        public IEnumerable<PostProviderDto> Posts { get; set; } = new List<PostProviderDto>();

        //Posts是外部传入的，但内部可能自己从api加载
        IEnumerable<PostProviderDto> posts;
        protected override async Task OnInitializedAsync()
        {
            //不用用OnSetParamterAsync，搞不懂为啥使用它会反复加载数据好多次

            await base.OnInitializedAsync();

            //若已传入可选角色
            if (Posts != null && Posts.Any() && (posts == null || !Posts.SequenceEqual(posts)))
            {
                posts = Posts;
                InitNames();
            }
            else if (posts == null)
            {

                await InitDataSource();

            }
        }


        async Task InitDataSource()
        {
            var r = await HttpClient.GetAllProvider<PostProviderDto>( new { }, null);
            Posts = posts = r.Items;
            InitNames();
        }

        /// <summary>
        /// 绑定的角色为空时，是否自动勾选默认角色
        /// 通常新增时设置为true
        /// </summary>
        [Parameter]
        public bool SelectDefaultIfEmpty { get; set; } = false;

        void InitNames()
        {
            if (Value == null || !Value.Any())
            {

                Options = posts.Select(x => new CheckboxOption<string> { Checked = SelectDefaultIfEmpty && x.IsSelected, Label = x.DisplayText, Value = x.Name }).ToArray();
                if (SelectDefaultIfEmpty)
                {
                    Value = posts?.Where(d => d.IsSelected).Select(data => data.Name).ToArray();
                    ValueChanged.InvokeAsync(Value);
                }
            }
            else
            {
                Options = posts.Select(x => new CheckboxOption<string> { Checked = Value.Contains(x.Name), Label = x.DisplayText, Value = x.Name }).ToArray();
            }
        }
    }
}
