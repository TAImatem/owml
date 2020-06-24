using System;
using UnityEngine;

namespace OWML.Common.Menus
{
    public interface IModInput<T> : IModInputGeneral
    {
        event Action<T> OnChange;
        T Value { get; set; }
    }
}