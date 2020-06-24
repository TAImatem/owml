using System;
using UnityEngine;

namespace OWML.Common.Menus
{
    public interface IModInputGeneral
    {
        MonoBehaviour Element { get; }
        string Title { get; set; }
        int Index { get; set; }
        void Show();
        void Hide();
        void Initialize(IModMenu menu);
    }
}