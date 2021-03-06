﻿using System;
using UnityEngine;

namespace OWML.Common
{
    public interface IModEvents
    {
        Action<MonoBehaviour, Events> OnEvent { get; set; }

        void Subscribe<T>(Events ev) where T : MonoBehaviour;

        [Obsolete("Use Subscribe instead")]
        void AddEvent<T>(Events ev) where T : MonoBehaviour;
    }
}
