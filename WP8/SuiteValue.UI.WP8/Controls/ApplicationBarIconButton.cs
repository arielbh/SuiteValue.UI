using System;
using System.Collections;
using System.Windows;

namespace SuiteValue.UI.WP8.Controls
{    
    public class ApplicationBarIconButton : ApplicationBarMenuItem, Microsoft.Phone.Shell.IApplicationBarIconButton
    {

        internal override void UpdateFromData()
        {
            base.UpdateFromData();
            IconUri = _data.IconUri;
        }

        #region Dependency Properties

        #region IconUri
        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconUri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register(
                "IconUri",
                typeof(Uri),
                typeof(ApplicationBarIconButton),
                new PropertyMetadata(default(Uri), (d, e) => ((ApplicationBarIconButton)d).IconUriChanged((Uri)e.NewValue)));

        private void IconUriChanged(Uri iconUri)
        {
            var button = SysAppBarMenuItem as Microsoft.Phone.Shell.IApplicationBarIconButton;
            button.IconUri = iconUri;
        }
        #endregion

        #endregion
    
        #region Overrides
        protected override IList AppBarItemsCollection
        {
            get { return AppBar.Buttons; }
        }

        protected override Microsoft.Phone.Shell.IApplicationBarMenuItem CreateApplicationBarMenuItem()
        {
            return new Microsoft.Phone.Shell.ApplicationBarIconButton();
        }        
        #endregion
    }
}
