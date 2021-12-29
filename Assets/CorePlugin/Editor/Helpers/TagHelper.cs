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

using UnityEditor;
using UnityEngine;

namespace CorePlugin.Editor.Helpers
{
    public static class TagHelper
    {
        public static void AddTag(string tag)
        {
            if (GetTagsProperty(out var so, out var tagsProperty)) return;

            for (var i = 0; i < tagsProperty.arraySize; ++i)
                if (tagsProperty.GetArrayElementAtIndex(i).stringValue == tag)
                    return; // Tag already present, nothing to do.
            var index = tagsProperty.arraySize - 1;
            tagsProperty.InsertArrayElementAtIndex(index);
            tagsProperty.GetArrayElementAtIndex(index).stringValue = tag;
            so.ApplyModifiedProperties();
            so.Update();
        }

        internal static bool IsTagsExists(params string[] tags)
        {
            if (GetTagsProperty(out var _, out var tagsProperty)) return false;

            foreach (var tag in tags)
            {
                var isExists = false;

                for (var i = 0; i < tagsProperty.arraySize; i++)
                    if (tag.Equals(tagsProperty.GetArrayElementAtIndex(i).stringValue))
                        isExists = true;
                if (isExists) continue;
                Debug.LogError($"Tag: \"{tag}\" not defined");
                return false;
            }
            return true;
        }

        internal static bool IsTagsExistsInternal(params string[] tags)
        {
            if (GetTagsProperty(out var _, out var tagsProperty)) return false;

            foreach (var tag in tags)
            {
                var isExists = false;

                for (var i = 0; i < tagsProperty.arraySize; i++)
                    if (tag.Equals(tagsProperty.GetArrayElementAtIndex(i).stringValue))
                        isExists = true;
                if (isExists) continue;
                return false;
            }
            return true;
        }

        private static bool GetTagsProperty(out SerializedObject so, out SerializedProperty tagsProperty)
        {
            tagsProperty = null;
            so = null;
            var asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");

            if (asset == null ||
                asset.Length <= 0)
                return true;
            so = new SerializedObject(asset[0]);
            tagsProperty = so.FindProperty("tags");
            return false;
        }
    }
}
