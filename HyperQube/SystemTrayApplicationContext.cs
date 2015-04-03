using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HyperQube.Library;

namespace HyperQube
{
    [Export(typeof (ApplicationContext))]
    internal class SystemTrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _icon;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IBulletTrace _trace;

        [ImportingConstructor]
        public SystemTrayApplicationContext([ImportMany] IEnumerable<Lazy<IQube>> qubes,
                                            Lazy<ISubscriptionService> subscriptionService, IBulletTrace trace,
                                            ITooltipProvider tooltipProvider)
        {
            _trace = trace;

            var executingAssembly = Assembly.GetExecutingAssembly();
            using (var stream = executingAssembly.GetManifestResourceStream("HyperQube.HyperQube.ico"))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                var icon = new Icon(stream);
                _icon = new NotifyIcon
                        {
                            Icon = icon,
                            Visible = true
                        };
            }

            ((TooltipProvider) tooltipProvider).Icon = _icon;

            _icon.ContextMenu = BuildContextMenu(qubes.Select(x => x.Value).OrderBy(x => x.Title).ToList().AsReadOnly());
            _trace.Information("Finished constructing system tray");

            _subscriptionService = subscriptionService.Value;
        }

        private ContextMenu BuildContextMenu(IEnumerable<IQube> qubes)
        {
            _trace.Information("Building context menu.");

            var menuItems = new List<MenuItem>
                            {
                                new MenuItem("Settings", EditSettings),
                                new MenuItem("Exit", Exit)
                            };

            var items = qubes.Select(BuildMenuItem).Where(x => x != null).ToArray();

            if (items.Length > 0)
            {
                var pluginMenuItem = new MenuItem("Plugins", items.ToArray());
                menuItems.Insert(0, pluginMenuItem);
            }

            var contextMenu = new ContextMenu(menuItems.ToArray());
            return contextMenu;
        }

        private void EditSettings(object sender, EventArgs e)
        {
            _subscriptionService.EnableOrDisableQubes();
        }

        private static MenuItem BuildMenuItem(IQube qube)
        {
            var hyperQubeWithMenu = qube as IQubeMenuItem;

            if (hyperQubeWithMenu == null) return null; // No menu's

            var hyperQubeWithSubMenus = hyperQubeWithMenu as IQubeMenuItemWithSubMenus;
            if (hyperQubeWithSubMenus == null)
            {
                // only one level
                return new MenuItem(qube.Title, (sender, args) => hyperQubeWithMenu.OpenMenu());
            }

            var pluginMenu = new MenuItem(qube.Title);
            BuildSubMenus(hyperQubeWithMenu, pluginMenu);

            return pluginMenu;
        }

        private static void BuildSubMenus(IQubeMenuItem qubeWithMenu, Menu pluginMenu)
        {
            if (qubeWithMenu == null) return;

            var subMenu = new MenuItem(qubeWithMenu.MenuTitle, (sender, args) => qubeWithMenu.OpenMenu());
            pluginMenu.MenuItems.Add(subMenu);

            var pluginWithSubMenus = qubeWithMenu as IQubeMenuItemWithSubMenus;
            if (pluginWithSubMenus == null) return;

            foreach (var pluginMenuItemWithSubMenu in pluginWithSubMenus.SubMenuItems.OrderBy(x => x.MenuTitle))
                BuildSubMenus(pluginMenuItemWithSubMenu, subMenu);
        }

        private void Exit(object sender, EventArgs e)
        {
            _icon.Visible = false;
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_icon != null) _icon.Dispose();
                if (_subscriptionService != null) _subscriptionService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
