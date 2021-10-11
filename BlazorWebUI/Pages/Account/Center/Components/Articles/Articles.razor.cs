using System.Collections.Generic;
using BlazorWebUI.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorWebUI.Pages.Account.Center
{
    public partial class Articles
    {
        [Parameter] public IList<ListItemDataType> List { get; set; }
    }
}