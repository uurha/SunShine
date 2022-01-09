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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CorePlugin.Core;
using CorePlugin.Editor.Extensions;
using CorePlugin.Extensions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.Editor.Windows
{
    internal class SelectorWindowAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            CoreSelectorWindow.ValidateCores();
        }
    }

    public class CoreSelectorWindow : EditorWindow
    {
        private List<SelectorWindowExtensions.NamedGroup> _cores = new List<SelectorWindowExtensions.NamedGroup>();
        private int _mainTab;
        private int _subTab = -1;

        private UnityEditor.Editor _embeddedInspector;
        private Object _displayObject;
        private const string Elements = "Elements";
        private const string EditScript = "Edit Script";

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            GetTabsIndexes();
        }

        private void GenerateMenu(Rect rect)
        {
            if (_displayObject == null) return;
            var menu = new GenericMenu();
            var showObjectContent = new GUIContent($"Show {_displayObject.PrettyEditorObjectName(SelectorWindowExtensions.StringsToRemove)} Object");

            menu.AddItem(showObjectContent, false,
                         () => ShowObject(_displayObject));
            var editScriptContent = new GUIContent(EditScript);
            menu.AddItem(editScriptContent, false, () => OpenScript(_displayObject));
            menu.DropDown(rect);
        }

        private void GetTabsIndexes()
        {
            _subTab = EditorPrefs.GetInt(nameof(CoreSelectorWindow) + nameof(_subTab), 0);
            _mainTab = EditorPrefs.GetInt(nameof(CoreSelectorWindow) + nameof(_mainTab), 0);
        }

        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<CoreSelectorWindow>(true, nameof(CoreSelectorWindow).PrettyCamelCase());
            window.Show();
            window.UpdateCores();
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            SetTabIndexes();
            if (_embeddedInspector != null) DestroyImmediate(_embeddedInspector);
        }

        private void OnGUI()
        {
            if (_cores.Count <= 0) return;
            var bufferMainTab = _mainTab;
            var columnCount = Mathf.RoundToInt(position.width / 200);

            _mainTab = UnityEditorExtension.SelectionGrid(_mainTab, _cores.Select(x => x.NamedObject.Name).ToArray(),
                                                          columnCount,
                                                          new GUIStyle(GUI.skin.button) { stretchWidth = true });

            if (_mainTab != bufferMainTab)
            {
                ClearEmbeddedInspector();
                _subTab = -1;
                SetTabIndexes();
            }
            if (_mainTab >= _cores.Count) return;
            var key = _cores[_mainTab];
            var bufferObject = key.Object;
            EditorGUILayout.Separator();

            if (key.NamedObjects.Count > 0)
                EditorGUILayout.LabelField(Elements,
                                           new GUIStyle(GUI.skin.label)
                                           {
                                               stretchWidth = true, alignment = TextAnchor.MiddleCenter,
                                               fontStyle = FontStyle.BoldAndItalic
                                           });

            var bufferSubTab = _subTab;
            _subTab = UnityEditorExtension.SelectionGrid(_subTab, key.NamedObjects.Select(x => x.Name).ToArray(),
                                                         columnCount,
                                                         new GUIStyle(GUI.skin.button) { stretchWidth = true });
            if (_subTab >= key.Value.Count) return;

            if (_subTab != bufferSubTab)
            {
                ClearEmbeddedInspector();
            }

            if (_subTab != -1)
            {
                bufferObject = key.Value[_subTab].Object;
                SetTabIndexes();
            }
            
            EditorGUILayout.Separator();

            EditorGUILayout.BeginFoldoutHeaderGroup(true, $"{bufferObject.PrettyTypeName()} (Script)", null,
                                                    GenerateMenu);
            EditorGUILayout.EndFoldoutHeaderGroup();
            RecycleInspector(bufferObject);
            _displayObject = bufferObject;
            _embeddedInspector.OnInspectorGUI();
        }

        private void ClearEmbeddedInspector()
        {
            if (_embeddedInspector == null) return;
            DestroyImmediate(_embeddedInspector);
            _embeddedInspector = null;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    _cores.Clear();
                    UpdateCores();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    _cores.Clear();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    _cores.Clear();
                    UpdateCores();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    _cores.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

        private static void OpenScript(Object displayObject)
        {
            var scriptPath = string.Empty;

            switch (displayObject)
            {
                case MonoBehaviour mono:
                {
                    var monoscript = MonoScript.FromMonoBehaviour(mono);
                    scriptPath = AssetDatabase.GetAssetPath(monoscript);
                    break;
                }
                case ScriptableObject scriptable:
                {
                    var monoscript = MonoScript.FromScriptableObject(scriptable);
                    scriptPath = AssetDatabase.GetAssetPath(monoscript);
                    break;
                }
                default:
                    Debug.LogException(new
                                           NotSupportedException($"<b><i>{displayObject.GetType().FullName}</i></b> should inherit from {nameof(MonoBehaviour)} or {nameof(ScriptableObject)}"));
                    break;
            }

            if (!InternalEditorUtility.OpenFileAtLineExternal(scriptPath, 1, 1))
                Debug.LogException(new FileNotFoundException("Something go wrong. Not possible to open file in IDE"));
        }

        private void RecycleInspector(Object target)
        {
            _embeddedInspector ??= UnityEditor.Editor.CreateEditor(target);
        }

        private void ResetIndexes()
        {
            if (_mainTab >= _cores.Count)
            {
                if (_mainTab != -1)
                    _mainTab = _cores.Count - 1;
                else
                    _mainTab = 0;
                _subTab = -1;
                SetTabIndexes();
                return;
            }
            var key = _cores[_mainTab];

            if (_subTab >= key.Value.Count)
            {
                if (_subTab != -1)
                    _subTab = key.Value.Count - 1;
                else
                    _subTab = -1;
            }
            SetTabIndexes();
        }

        private void SetTabIndexes()
        {
            EditorPrefs.SetInt(nameof(CoreSelectorWindow) + nameof(_subTab), _subTab);
            EditorPrefs.SetInt(nameof(CoreSelectorWindow) + nameof(_mainTab), _mainTab);
        }

        private static void ShowObject(Object displayObject)
        {
            Selection.SetActiveObjectWithContext(displayObject, displayObject);
        }

        private void UpdateCores()
        {
            if (UnityExtensions.TryToFindObjectOfType<CoreManager>(out var coreManager))
                _cores = typeof(CoreManager).ElementGathering(coreManager).ToList();
        }

        [DidReloadScripts]
        public static void ValidateCores()
        {
            if (!HasOpenInstances<CoreSelectorWindow>()) return;
            var window = GetWindow<CoreSelectorWindow>(true, nameof(CoreSelectorWindow).PrettyCamelCase());
            window.UpdateCores();
            window.ResetIndexes();
        }
    }
}
