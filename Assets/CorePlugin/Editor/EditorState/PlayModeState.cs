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

using System.Collections.Generic;
using System.Linq;
using CorePlugin.Editor.Extensions;
using CorePlugin.Editor.Helpers;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Editor.EditorState
{
    /// <summary>
    /// Class responsible for pre play mode validation checks
    /// </summary>

    //TODO: Find better name for internal class
    [InitializeOnLoad]
    internal static class PlayModeState
    {
        static PlayModeState()
        {
            EditorApplication.playModeStateChanged -= LogPlayModeState;
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            var pairs = AttributeExtensions.ErrorObjectPairs(); //TODO: validate sub prefabs on opened scene
            #if SCENE_MANAGMENT_ASSET
            if (!SceneLoaderSettingsValidator.Validate(out var settings))
            {
                var error = new ErrorObjectPair(SceneLoaderSettingsValidator.ReturnErrorText(settings), null);
                pairs = pairs.Concat(new[] {error});
            }
            #endif
            var errorObjectPairs = pairs as ErrorObjectPair[] ?? pairs.ToArray();
            if (IsPlayModeAvailable(errorObjectPairs)) return;
            EditorApplication.Beep();
            EditorApplication.ExitPlaymode();
            foreach (var errorObjectPair in errorObjectPairs) Debug.LogError(errorObjectPair.Key.Replace("\n", ""), errorObjectPair.Value);
        }

        private static bool IsPlayModeAvailable(IEnumerable<ErrorObjectPair> pairs)
        {
            return !pairs.Any();
        }
    }
}
