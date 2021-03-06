﻿using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public class ModTabMenu : ModPopupMenu, IModTabMenu
    {
        private readonly IModTabbedMenu _optionsMenu;

        public TabButton TabButton { get; private set; }

        private Text _text;
        public new string Title
        {
            get => _text.text;
            set => _text.text = value;
        }

        public ModTabMenu(IModConsole console, IModTabbedMenu optionsMenu) : base(console)
        {
            _optionsMenu = optionsMenu;
        }

        public void Initialize(TabButton tabButton)
        {
            TabButton = tabButton;
            TabButton.OnTabSelect += t => SelectFirst();
            var menu = tabButton.GetValue<Menu>("_tabbedMenu");
            Initialize(menu);
            InvokeOnInit();
        }

        public override void Open()
        {
            if (!_optionsMenu.IsOpen)
            {
                _optionsMenu.Open();
            }
            _optionsMenu.Menu.Invoke("SelectTabButton", TabButton);
        }

        public new IModTabMenu Copy()
        {
            var tabButton = Object.Instantiate(TabButton, TabButton.transform.parent);
            Object.Destroy(tabButton.GetComponentInChildren<LocalizedText>(true));
            _text = tabButton.GetComponentInChildren<Text>(true);
            var menu = Object.Instantiate(Menu, Menu.transform.parent);
            tabButton.SetValue("_tabbedMenu", menu);
            var modMenu = new ModTabMenu(OwmlConsole, _optionsMenu);
            modMenu.Initialize(tabButton);
            return modMenu;
        }

        public new IModTabMenu Copy(string title)
        {
            var copy = Copy();
            Title = title;
            return copy;
        }

    }
}
