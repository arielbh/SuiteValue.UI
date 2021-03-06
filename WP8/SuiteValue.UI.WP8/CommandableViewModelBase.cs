﻿using System.Collections.Generic;
using Microsoft.Phone.Shell;
using SuiteValue.UI.WP8.Controls;
using ApplicationBarIconButton = SuiteValue.UI.WP8.Controls.ApplicationBarIconButton;
using ApplicationBarMenuItem = SuiteValue.UI.WP8.Controls.ApplicationBarMenuItem;

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

        private ApplicationBarMode _appBarMode;

        public ApplicationBarMode AppBarMode
        {
            get { return _appBarMode; }
            set
            {
                if (value != _appBarMode)
                {
                    _appBarMode = value;
                    OnPropertyChanged(() => AppBarMode);
                }
            }
        }

        private AppBarData[] _appbarCommands;

        public AppBarData[] AppbarCommands
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

        private AppBarData[] _appbarMenuItems;

        public AppBarData[] AppbarMenuItems
        {
            get { return _appbarMenuItems; }
            set
            {
                if (value != _appbarMenuItems)
                {
                    _appbarMenuItems = value;
                    OnPropertyChanged(() => AppbarMenuItems);
                    if (AppbarMenuItems != null && AppbarMenuItems.Length > 0)
                    {
                        ShowAppBar = true;
                    }
                }
            }
        }  

        public virtual void Activate()
        {
            
        }

        public virtual void Deactivate()
        {
            
        }
    }
}