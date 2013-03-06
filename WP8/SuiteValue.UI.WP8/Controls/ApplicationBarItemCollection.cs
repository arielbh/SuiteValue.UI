using System.Windows;
using Microsoft.Phone.Shell;
using ApplicationBarMenuItem = SuiteValue.UI.WP8.Controls.ApplicationBarMenuItem;

namespace SuiteValue.UI.WP8.Controls
{
    public abstract class ApplicationBarItemCollection<T> : DependencyObjectCollection<T> where T : ApplicationBarMenuItem
    {        
        internal void Attach(object dataContext, IApplicationBar sysAppBar)
        {
            foreach (var item in this)
            {
                item.DataContext = dataContext;
                item.Attach(sysAppBar);
            }
        }

        internal void Dettach(IApplicationBar sysAppBar)
        {
            foreach (var item in this)
            {
                item.Dettach(sysAppBar);
                item.DataContext = null;
            }
        }        
    }

    public class ApplicationBarMenuItemCollection : ApplicationBarItemCollection<ApplicationBarMenuItem>
    {
    }

    public class ApplicationBarIconButtonCollection : ApplicationBarItemCollection<ApplicationBarIconButton>
    {
    }
}
