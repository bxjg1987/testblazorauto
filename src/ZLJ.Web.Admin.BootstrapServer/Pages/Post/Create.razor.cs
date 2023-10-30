using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Admin.Post.Dto;

namespace ZLJ.Web.Admin.BootstrapServer.Pages.Post
{
    public partial class Create
    {
        [Parameter]
        public PostDto CreatedDto { get; set; }

        protected override void OnInitialized()
        {
            CreatedDto.Description = "xxxxxxxxxxxxxxx";
            base.OnInitialized();
        }
    }
}
