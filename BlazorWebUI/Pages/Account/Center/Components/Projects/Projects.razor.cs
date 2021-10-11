using System.Collections.Generic;
using BlazorWebUI.Models;
using Microsoft.AspNetCore.Components;
using AntDesign;

namespace BlazorWebUI.Pages.Account.Center
{
    public partial class Projects
    {
        private readonly ListGridType _listGridType = new ListGridType
        {
            Gutter = 24,
            Column = 4
        };

        [Parameter]
        public IList<ListItemDataType> List { get; set; }
    }
}