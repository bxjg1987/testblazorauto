using System.Collections.Generic;
using ZLJ.UI.Admin.Models;
using Microsoft.AspNetCore.Components;

namespace ZLJ.UI.Admin.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}