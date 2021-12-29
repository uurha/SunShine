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

using System.IO;
using CorePlugin.Core;
using CorePlugin.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Editor.MenuItemHelpers
{
    public static class CoreMenuItems
    {
        [MenuItem("Core/Create Core Manager")]
        private static void CreateCoreManager()
        {
            CreatePrefab<CoreManager>();
        }

        [MenuItem("Core/Show Selector Window")]
        private static void ShowSelectorWindow()
        {
            CoreSelectorWindow.Init();
        }

        private static string PrefabPath<T>()
        {
            return Path.Combine("Prefabs", typeof(T).Name);
        }

        private static void CreatePrefab<T>() where T : MonoBehaviour
        {
            var prefabPath = PrefabPath<T>();
            var componentOrGameObject = Resources.Load<T>(prefabPath);

            if (componentOrGameObject != null)
            {
                var objects = Object.FindObjectsOfType(typeof(T));

                if (objects.Length > 0)
                {
                    foreach (var o in objects) ShowError($"Should be only one {typeof(T).Name} in scene", o);
                    return;
                }
                if (!(PrefabUtility.InstantiatePrefab(componentOrGameObject) is T prefab)) return;
                prefab.name = componentOrGameObject.name;
                prefab.transform.SetAsLastSibling();
            }
            else
            {
                var c = Path.Combine("..", "Core", nameof(Resources), prefabPath);
                var message = $"Probably you move or rename {typeof(T).Name} prefab from initial path ({c}).";
                ShowError(message);
            }
        }

        private static void ShowError(string error, Object context = null)
        {
            EditorApplication.Beep();
            Debug.LogError(error, context);
        }
    }
}
