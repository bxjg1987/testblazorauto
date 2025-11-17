using System.Collections.Immutable;
using System.Net.Http;
using ZLJ.Application.Share.Roles;

namespace ZLJ.Admin.CoreRCL.Share
{
    // 若后面还有类似需求，可以考虑在rcl中定义一个抽象组件，然后这里的组件继承它

    public class TreePermission : Tree<FlatPermissionDto>
    {
        //FlatPermissionDto[] _datas = Array.Empty<FlatPermissionDto>();
        /// <summary>
        /// 专门给肉夹馍aop用的，你不该调用这个
        /// </summary>
        [Inject]
        public IServiceProvider Services { get; set; }
        [Inject]
        public IHttpClientFactory HttpClientFactory { get; set; }
        protected virtual HttpClient HttpClient => HttpClientFactory.CreateHttpClientAdmin();
      
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var dic = parameters.ToDictionary();
            if (!dic.ContainsKey(nameof(Checkable)))
                Checkable = true;

            await base.SetParametersAsync(parameters);

            //var dic = parameters.ToDictionary();
            //if(!dic.ContainsKey(nameof(Checkable)))
            //    Checkable= true;
        }

        //string? searchKey;
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            //if (DataSource == null)
            //    DataSource = _datas;

            if (TitleExpression == null)
                TitleExpression = x => x.DataItem.DisplayName;

            //KeyExpression
            if (KeyExpression == null)
                KeyExpression = node => node.DataItem.Name;

            if (ChildrenExpression == null)
                ChildrenExpression = x => x.DataItem.Children;


            
            //Checkable = true;

            // SearchExpression
            //if (SearchExpression == null)
            //    SearchExpression = x => searchKey.IsNullOrWhiteSpaceBXJG() || x.DataItem.Name.Contains(searchKey) || x.DataItem.DisplayName.Contains(searchKey);

            if (IsLeafExpression == null)
                IsLeafExpression = x => !x.DataItem.Children.Any();

        }
        //async Task NodeLoadDelayAsync(TreeEventArgs<FlatPermissionDto> args)
        //{
        //    var dto = args.Node.DataItem;
        //    var dataItem = (await HttpClient.Post<List<FlatPermissionDto>>("Permission/GetAllPermissions", ps: new { parentName= dto.Name })).ToList();
        //    //args.Node.DataItem.Children?.Clear();
        //    dto.Children = dataItem;
        //    //var dataItem = ((Data)args.Node.DataItem);
        //    //dataItem.Childs.Clear();
        //    //dataItem.Childs.AddRange(new List<Data>() { new Data($"{dataItem.Title}-1"), new Data($"{dataItem.Title}-2") });
        //}
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            DataSource = (await this.HttpClient.Post<List<FlatPermissionDto>>("Permission/GetAllPermissions", ps: null)).ToArray();
            foreach (var item in DataSource)
            {
                item.Children = DataSource.Where(x => x.ParentName == item.Name).ToImmutableList();
            }
            DataSource = DataSource.Where(x=>x.ParentName==null).ToArray();
            //base.check
           // await Task.Delay(1000);
            //InvokeAsync(StateHasChanged);
        }
    }
}
