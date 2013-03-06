using System.Collections.Generic;
using SuiteValue.UI.WP8.Controls;

namespace SuiteValue.UI.WP8
{
    public class CommandableViewModelBase : AsyncViewModelBase
    {
        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                if (value != _header)
                {
                    _header = value;
                    OnPropertyChanged(() => Header);
                }
            }
        }

        private bool _showAppBar;

        public bool ShowAppBar
        {
            get { return _showAppBar; }
            set
            {
                if (value != _showAppBar)
                {
                    _showAppBar = value;
                    OnPropertyChanged(() => ShowAppBar);
                }
            }
        }

        private ApplicationBarIconButton[] _appbarCommands;

        public ApplicationBarIconButton[] AppbarCommands
        {
            get { return _appbarCommands; }
            set
            {
                if (value != _appbarCommands)
                {
                    _appbarCommands = value;
                    OnPropertyChanged(() => AppbarCommands);
                    if (AppbarCommands != null && AppbarCommands.Length > 0)
                    {
                        ShowAppBar = true;
                    }
                }
            }
        }

        private List<ApplicationBarMenuItem> _appbarMenuItems;

        public List<ApplicationBarMenuItem> AppbarMenuItems
        {
            get { return _appbarMenuItems; }
            set
            {
                if (value != _appbarMenuItems)
                {
                    _appbarMenuItems = value;
                    OnPropertyChanged(() => AppbarMenuItems);
                    if (AppbarMenuItems != null && AppbarMenuItems.Count > 0)
                    {
                        ShowAppBar = true;
                    }
                }
            }
        }  

        public virtual void Activate()
        {
            
        }
    }
}