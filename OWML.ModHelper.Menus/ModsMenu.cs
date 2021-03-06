﻿using System.Collections.Generic;
using System.Linq;
using OWML.Common;
using OWML.Common.Menus;
using OWML.ModHelper.Events;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public class ModsMenu : ModPopupMenu, IModsMenu
    {
        private const string ModsButtonTitle = "MODS";
        private const string OwmlButtonTitle = "OWML";

        private readonly IModMenus _menus;
        private readonly List<IModConfigMenu> _modConfigMenus;

        private Transform _modMenuTemplate;
        private IModButton _modButtonTemplate;
        private readonly IModInputHandler _inputHandler;

        public ModsMenu(IModConsole console, IModMenus menus, IModInputHandler inputHandler) : base(console)
        {
            _menus = menus;
            _modConfigMenus = new List<IModConfigMenu>();
            _inputHandler = inputHandler;
        }

        public void AddMod(IModData modData, IModBehaviour mod)
        {
            _modConfigMenus.Add(new ModConfigMenu(OwmlConsole, modData, mod));
        }

        public IModConfigMenu GetModMenu(IModBehaviour modBehaviour)
        {
            OwmlConsole.WriteLine("Registering " + modBehaviour.ModHelper.Manifest.UniqueName);
            var modConfigMenu = _modConfigMenus.FirstOrDefault(x => x.Mod == modBehaviour);
            if (modConfigMenu == null)
            {
                OwmlConsole.WriteLine($"Error: {modBehaviour.ModHelper.Manifest.UniqueName} isn't added.");
                return null;
            }
            return modConfigMenu;
        }

        public void Initialize(IModOWMenu mainMenu)
        {
            if (_modMenuTemplate == null)
            {
                CreateModMenuTemplate(mainMenu);
            }
            
            var modsButton = mainMenu.OptionsButton.Duplicate(ModsButtonTitle);
            var optionsMenu = mainMenu.OptionsMenu;
            var modsMenu = CreateModsMenu(optionsMenu);
            modsButton.OnClick += () => modsMenu.Open();
            Menu = mainMenu.Menu;

            InitConfigMenu(_menus.OwmlMenu, optionsMenu);
            var owmlButton = modsButton.Duplicate(OwmlButtonTitle);
            owmlButton.OnClick += () => _menus.OwmlMenu.Open();
        }

        private void CreateModMenuTemplate(IModOWMenu mainMenu)
        {
            var remapControlsButton = mainMenu.OptionsMenu.InputTab.GetTitleButton("UIElement-RemapControls");
            var buttonTemplate = Object.Instantiate(remapControlsButton.Button);
            buttonTemplate.gameObject.AddComponent<DontDestroyOnLoad>();
            _modButtonTemplate = new ModTitleButton(buttonTemplate, mainMenu);
            _modButtonTemplate.Button.enabled = false;

            var submitActionMenu = remapControlsButton.Button.GetComponent<SubmitActionMenu>();
            var rebindingMenu = submitActionMenu.GetValue<Menu>("_menuToOpen");
            var rebindingCanvas = rebindingMenu.transform.parent;
            _modMenuTemplate = Object.Instantiate(rebindingCanvas);
            _modMenuTemplate.gameObject.AddComponent<DontDestroyOnLoad>();
        }

        private IModPopupMenu CreateModsMenu(IModTabbedMenu options)
        {
            var modsTab = options.InputTab.Copy("MODS");
            modsTab.BaseButtons.ForEach(x => x.Hide());
            modsTab.Menu.GetComponentsInChildren<Selectable>(true).ToList().ForEach(x => x.gameObject.SetActive(false));
            modsTab.Menu.GetValue<TooltipDisplay>("_tooltipDisplay").GetComponent<Text>().color = Color.clear;
            options.AddTab(modsTab);
            var modMenuTemplate = _modMenuTemplate.GetComponentInChildren<Menu>(true);
            var modMenuCopy = Object.Instantiate(modMenuTemplate, _modMenuTemplate.transform);
            var modInputCombinationElementTemplate = new ModInputCombinationElement(options.InputTab.ToggleInputs[0].Copy().Toggle,
                _menus.InputCombinationMenu, _menus.InputCombinationElementMenu, _inputHandler);
            _menus.InputCombinationMenu.Initialize(modMenuCopy, modInputCombinationElementTemplate);
            foreach (var modConfigMenu in _modConfigMenus)
            {
                var modButton = _modButtonTemplate.Copy(modConfigMenu.Manifest.Name);
                modButton.Button.enabled = true;
                InitConfigMenu(modConfigMenu, options);
                modButton.OnClick += modConfigMenu.Open;
                modsTab.AddButton(modButton);
            }
            modsTab.UpdateNavigation();
            modsTab.SelectFirst();
            return modsTab;
        }

        private void InitConfigMenu(IModConfigMenuBase modConfigMenu, IModTabbedMenu options)
        {
            var toggleTemplate = options.InputTab.ToggleInputs[0];
            var sliderTemplate = options.InputTab.SliderInputs[0];
            var modMenuTemplate = _modMenuTemplate.GetComponentInChildren<Menu>(true);
            var modMenuCopy = Object.Instantiate(modMenuTemplate, _modMenuTemplate.transform);
            var textInputTemplate = new ModTextInput(toggleTemplate.Copy().Toggle, modConfigMenu, _menus.InputMenu);
            textInputTemplate.Hide();
            var comboInputTemplate = new ModComboInput(toggleTemplate.Copy().Toggle, modConfigMenu, _menus.InputCombinationMenu, _inputHandler);
            comboInputTemplate.Hide();
            var numberInputTemplate = new ModNumberInput(toggleTemplate.Copy().Toggle, modConfigMenu, _menus.InputMenu);
            numberInputTemplate.Hide();
            modConfigMenu.Initialize(modMenuCopy, toggleTemplate, sliderTemplate, textInputTemplate, numberInputTemplate, comboInputTemplate);
        }

    }
}
