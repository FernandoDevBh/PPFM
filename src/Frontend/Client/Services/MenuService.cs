using Client.Components.Navigation.Menu.Models;
using Client.Components.Navigation.Menu.Services;

namespace Client.Services;

public class MenuService : IMenuService
{
    public List<MenuItem> GetMenus()
    {
        return new List<MenuItem>
        {
            CreateMenuItem("Dashboard", "DashboardFill"),
            CreateMenuItem("Pages", "AiOutlineFileText"),
            CreateMenuItem("Media", "FileImageFill"),
            CreateMenuItem("Projects", "LayoutTextSidebarReverse",new()
            {
                CreateMenuItem("Submenu 1"),
                CreateMenuItem("Submenu 2"),
                CreateMenuItem("Submenu 3"),
            }),
            CreateMenuItem("Analytics", "AiOutLineBarChart"),
            CreateMenuItem("Inbox", "AiOutlineMail"),
            CreateMenuItem("Profile", "BsPerson"),
            CreateMenuItem("Settings", "AiOutlineSetting"),
            CreateMenuItem("Logout", "AiOutlineLogout"),
        };
    }

    private MenuItem CreateMenuItem(string title, string? icon = null, List<MenuItem>? items = null) =>
        new()
        {
            Title = title,
            Icon = string.IsNullOrEmpty(icon) ? string.Empty : $"Client.Icons.{icon}, Client.Icons",
            SubMenuItems = items ?? new List<MenuItem>(),
            SubMenu = (items != null && items.Any())
        };           
}
