#region license

// Copyright 2021 - 2021 Arcueid Elizabeth D'athemon
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using CorePlugin.Logger;
using UnityEngine;

namespace CorePlugin.Singletons
{
    /// <summary>
    /// Base for static objects singletons.
    /// Strongly recommended to use singletons as little as possible.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticObjectSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private protected static T _instance;

        public static bool IsInitialised => _instance != null;

        private static void CreateInstance(out T instance)
        {
            instance = new GameObject(typeof(T).Name).AddComponent<T>();
        }

        private protected static T GetInstance()
        {
            if (IsInitialised) return _instance;
            CreateInstance(out _instance);
            return _instance;
        }

        private protected static void Initialize()
        {
            if (IsInitialised) return;
            CreateInstance(out _instance);
        }

        protected virtual void OnDestroy()
        {
            DebugLogger.Log("OnDestroy: " + typeof(T));
            if (_instance == this) _instance = null;
        }
    }
}
