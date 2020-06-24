using System;
using OWML.Common.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace OWML.ModHelper.Menus
{
    public abstract class ModInput<T> : ModInputGeneral, IModInput<T>
    {
        public event Action<T> OnChange;
        public abstract T Value { get; set; }

        protected ModInput(MonoBehaviour element, IModMenu menu):base(element, menu)
        {
        }

        protected void InvokeOnChange(T value)
        {
            OnChange?.Invoke(value);
        }
    }
}
