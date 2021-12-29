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
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CorePlugin.Editor.EditorState
{
    /// <summary>
    /// Class responsible for pre build validation checks
    /// </summary>

    //TODO: Find better name for internal class
    [InitializeOnLoad]
    internal static class BuildState
    {
        static BuildState()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(CheckSceneObjects);
        }

        private static void CheckSceneObjects(BuildPlayerOptions buildPlayerOptions)
        {
            var pairs = Enumerable.Empty<ErrorObjectPair>();
            pairs = pairs.Concat(CheckSceneObjects());
            pairs = pairs.Concat(CheckPrefabs());
            #if SCENE_MANAGMENT_ASSET
            if (!SceneLoaderSettingsValidator.Validate(out var settings))
            {
                var error = new ErrorObjectPair(SceneLoaderSettingsValidator.ReturnErrorText(settings), null);
                pairs = pairs.Concat(new[] {error});
            }
            #endif
            var errors = pairs as ErrorObjectPair[] ?? pairs.ToArray();

            if (!errors.Any())
            {
                BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
                return;
            }
            EditorApplication.Beep();
            foreach (var error in errors) Debug.LogException(new BuildPlayerWindow.BuildMethodException(error.Key));
        }

        private static IEnumerable<ErrorObjectPair> CheckPrefabs()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();

            var objs =
                allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject)
                         .Zip(allAssets, (o, s) => new { obj = o, path = s });
            var errors = Enumerable.Empty<ErrorObjectPair>();

            return objs.Where(x => x.obj != null)
                       .Aggregate(errors, (current, value) =>
                                              current.Concat(AttributeExtensions.ErrorObjectPairs(value.obj)
                                                                                .Select(x =>
                                                                                        {
                                                                                            x.Key += $"\n<b>Prefab path:</b> <i>\"{value.path}\"</i>";
                                                                                            return x;
                                                                                        })));
        }

        private static IEnumerable<ErrorObjectPair> CheckSceneObjects()
        {
            var errors = Enumerable.Empty<ErrorObjectPair>();
            var openScene = SceneManager.GetActiveScene().path;

            if (EditorBuildSettings.scenes.Length > 0)
                foreach (var s in EditorBuildSettings.scenes)
                {
                    if (!s.enabled) continue;
                    var scene = EditorSceneManager.OpenScene(s.path);

                    errors = scene.GetRootGameObjects()
                                  .Aggregate(errors, (current, gameObject) =>
                                                         current.Concat(AttributeExtensions.ErrorObjectPairs(gameObject)
                                                                                           .Select(x =>
                                                                                                   {
                                                                                                       x.Key +=
                                                                                                           $"\n<b>Scene path</b>: <i>\"{s.path}\"</i>";
                                                                                                       return x;
                                                                                                   })));
                }
            else
                errors = SceneManager.GetActiveScene().GetRootGameObjects()
                                     .Aggregate(errors,
                                                (current, rootGameObject) => current.Concat(AttributeExtensions.ErrorObjectPairs(rootGameObject)));
            EditorSceneManager.OpenScene(openScene);
            return errors;
        }
    }
}
