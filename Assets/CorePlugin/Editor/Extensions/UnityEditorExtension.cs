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

using System.Linq;
using System.Reflection;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Extensions;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Editor.Extensions
{
    /// <summary>
    /// Extensions for Unity Editor classes
    /// </summary>
    public static class UnityEditorExtension
    {
        /// <summary>
        /// Override for default Inspector HelpBox with RTF text
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static void HelpBox(string message, MessageType type)
        {
            var style = new GUIStyle(EditorStyles.helpBox) { richText = true, fontSize = 11 };
            HelpBox(message, type, style);
        }

        /// <summary>
        /// Gets object name and converts into CamelCase. If input has attribute DisplayNameAttribute then displays name from attribute
        /// </summary>
        /// <param name="input"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string PrettyEditorObjectName(this Object input, params string[] remove)
        {
            var attr = input.GetType().GetCustomAttribute<DisplayNameAttribute>();
            var name = input.name;
            if (attr != null) name = attr.GetDisplayName(name);
            if (remove == null) return name.PrettyCamelCase();
            name = remove.Aggregate(name, (current, s) => current.Replace(s, string.Empty));
            return name.PrettyCamelCase();
        }

        public static string PrettyTypeName(this Object input, params string[] remove)
        {
            var attr = input.GetType().GetCustomAttribute<DisplayNameAttribute>();
            var name = input.GetType().Name;
            if (attr != null) name = attr.GetDisplayName(name);
            if (remove == null) return name.PrettyCamelCase();
            name = remove.Aggregate(name, (current, s) => current.Replace(s, string.Empty));
            return name.PrettyCamelCase();
        }

        public static int SelectionGrid(int selected, string[] texts, int xCount, GUIStyle style,
                                        params GUILayoutOption[] options)
        {
            var bufferSelected = selected;
            GUILayout.BeginVertical();
            var count = 0;
            var isHorizontal = false;

            for (var index = 0; index < texts.Length; index++)
            {
                var text = texts[index];

                if (count == 0)
                {
                    GUILayout.BeginHorizontal();
                    isHorizontal = true;
                }
                count++;
                if (GUILayout.Toggle(bufferSelected == index, text, new GUIStyle(style), options)) bufferSelected = index;

                if (count == xCount)
                {
                    GUILayout.EndHorizontal();
                    count = 0;
                    isHorizontal = false;
                }
            }
            if (isHorizontal) GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            return bufferSelected;
        }

        /// <summary>
        /// Override for default Inspector HelpBox with style
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="style"></param>
        public static void HelpBox(string message, MessageType type, GUIStyle style)
        {
            var icon = IconName(type);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(GUIContent.none, EditorGUIUtility.TrTextContentWithIcon(message, icon), style);
        }

        /// <summary>
        /// Getting Icon Name from Unity Inspector
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string IconName(MessageType type)
        {
            var icon = type switch
                       {
                           MessageType.Info => "console.infoicon",
                           MessageType.Warning => "console.warnicon",
                           MessageType.Error => "console.erroricon",
                           _ => ""
                       };
            return icon;
        }
    }
}
