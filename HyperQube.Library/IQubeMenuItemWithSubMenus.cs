using System.Collections.Generic;

namespace HyperQube.Library
{
    public interface IQubeMenuItemWithSubMenus : IQubeMenuItem
    {
        IEnumerable<IQubeMenuItem> SubMenuItems { get; }
    }
}
