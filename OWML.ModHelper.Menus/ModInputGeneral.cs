using System;
using OWML.Common.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public abstract class ModInputGeneral: IModInputGeneral
    {
        public MonoBehaviour Element { get; }

        private int _index;
        public int Index
        {
            get => Element.transform.parent == null ? _index : Element.transform.GetSiblingIndex();
            set
            {
                _index = value;
                Element.transform.SetSiblingIndex(value);
            }
        }

        private readonly Text _text;
        public string Title
        {
            get => _text.text;
            set => _text.text = value;
        }

        protected IModMenu Menu;

        protected ModInputGeneral(MonoBehaviour element, IModMenu menu)
        {
            Element = element;
            Menu = menu;
            _text = element.GetComponentInChildren<Text>();
        }


        public void Show()
        {
            Element.gameObject.SetActive(true);
        }

        public void Hide()
        {
            Element.gameObject.SetActive(false);
        }

        public virtual void Initialize(IModMenu menu)
        {
            Menu = menu;
        }

    }
}
