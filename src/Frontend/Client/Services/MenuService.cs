using Client.Icons;
using Microsoft.AspNetCore.Components;
using Client.Components.Navigation.Menu.Models;
using Client.Components.Navigation.Menu.Services;

namespace Client.Services;

public class MenuService : IMenuService
{
    public List<MenuItem> GetMenus()
    {
        return new List<MenuItem>
        {
            CreateMenuItem<DashboardFill>("Dashboard"),
            CreateMenuItem<AiOutlineFileText>("Pages"),
            CreateMenuItem<FileImageFill>("Media"),
            CreateMenuItem<LayoutTextSidebarReverse>("Projects", new()
            {
                CreateMenuItem("Submenu 1"),
                CreateMenuItem("Submenu 2"),
                CreateMenuItem("Submenu 3"),
            }),
            CreateMenuItem<AiOutLineBarChart>("Analytics"),
            CreateMenuItem<AiOutlineMail>("Inbox"),
            CreateMenuItem<BsPerson>("Profile"),
            CreateMenuItem<AiOutlineSetting>("Settings"),
            CreateMenuItem<AiOutlineLogout>("Logout"),
        };
    }

    private MenuItem CreateMenuItem(string title) =>
        new() { Title = title };
        
    
    private MenuItem CreateMenuItem<TComponent>(string title, List<MenuItem>? items = null) where TComponent : IComponent
    {
        return new()
        {
            Title = title,
            Icon = (builder) =>
            {
                builder.OpenComponent(0, typeof(TComponent));
                builder.CloseComponent();
            },
            SubMenu = items != null && items.Any(),
            SubMenuItems = items != null ? items : new()
        };
    }
}
