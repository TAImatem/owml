﻿using System;
using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Events;
using Object = UnityEngine.Object;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public class ModPopupMenu : ModMenu, IModPopupMenu
    {
        public Action OnOpen { get; set; }
        public Action OnClose { get; set; }

        public bool IsOpen { get; private set; }

        private Text _title;
        public string Title
        {
            get => _title.text;
            set => _title.text = value;
        }

        public ModPopupMenu(IModConsole console) : base(console) { }

        public override void Initialize(Menu menu, LayoutGroup layoutGroup)
        {
            base.Initialize(menu, layoutGroup);
            _title = Menu.GetComponentInChildren<Text>(true);
            var localizedText = _title.GetComponent<LocalizedText>();
            if (localizedText != null)
            {
                Title = UITextLibrary.GetString(localizedText.GetValue<UITextType>("_textID"));
                Object.Destroy(localizedText);
            }
            Menu.OnActivateMenu += OnActivateMenu;
            Menu.OnDeactivateMenu += OnDeactivateMenu;
        }

        private void OnDeactivateMenu()
        {
            IsOpen = false;
            OnClose?.Invoke();
        }

        private void OnActivateMenu()
        {
            IsOpen = true;
            OnOpen?.Invoke();
        }

        public virtual void Open()
        {
            if (Menu == null)
            {
                OwmlConsole.WriteLine("Warning: can't open menu, it doesn't exist.");
                return;
            }
            SelectFirst();
            Menu.EnableMenu(true);
        }

        public void Close()
        {
            if (Menu == null)
            {
                OwmlConsole.WriteLine("Warning: can't close menu, it doesn't exist.");
                return;
            }
            Menu.EnableMenu(false);
        }

        public void Toggle()
        {
            if (IsOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }

        public IModPopupMenu Copy()
        {
            if (Menu == null)
            {
                OwmlConsole.WriteLine("Warning: can't copy menu, it doesn't exist.");
                return null;
            }
            var menu = Object.Instantiate(Menu, Menu.transform.parent);
            var modMenu = new ModPopupMenu(OwmlConsole);
            modMenu.Initialize(menu);
            return modMenu;
        }

        public IModPopupMenu Copy(string title)
        {
            var copy = Copy();
            copy.Title = title;
            return copy;
        }

        [Obsolete("Use Copy instead")]
        public IModPopupMenu CreateCopy(string title)
        {
            if (Menu == null)
            {
                OwmlConsole.WriteLine("Warning: can't copy menu, it doesn't exist.");
                return null;
            }
            var menu = Copy();
            menu.Title = title;
            return menu;
        }

    }
}
