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
using System.Runtime.CompilerServices;
using CorePlugin.Attributes.Validation;
using CorePlugin.Core;
using CorePlugin.Extensions;
using CorePlugin.Logger;
using CorePlugin.ReferenceDistribution.Interface;
using UnityEngine;

namespace CorePlugin.ReferenceDistribution
{
    /// <summary>
    /// Class responsible for reference distribution inside one scene.
    /// <remarks> Strongly recommended to use <see cref="CorePlugin.Cross.Events"/> and <see cref="CorePlugin.Cross.Events.Interface"/> instead of direct reference serialization.</remarks>
    /// </summary>
    [RequireComponent(typeof(CoreManager))]
    [OneAndOnly]
    public class ReferenceDistributor : MonoBehaviour
    {
        private IEnumerable<IDistributingReference> _distributingReferences;
        private bool _isInitialized;
        private static ReferenceDistributor _instance;
        private static readonly string[] WarningCallers = { "Awake", "OnEnable" };

        public static bool TryGetReference<T>(out T reference, [CallerMemberName] string callerName = "")
            where T : IDistributingReference
        {
            ValidateCaller(callerName);
            reference = !_instance._isInitialized ? default : _instance._distributingReferences.OfType<T>().FirstOrDefault();
            return reference != null;
        }

        /// <summary>
        /// Getting reference by type from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetReference<T>([CallerMemberName] string callerName = "") where T : IDistributingReference
        {
            ValidateCaller(callerName);
            return !_instance._isInitialized ? default : _instance._distributingReferences.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Initializing distribution references
        /// </summary>
        public void Initialize()
        {
            _isInitialized = UnityExtensions.TryToFindObjectsOfType(out _distributingReferences);
            _instance = this;
        }

        private void OnDisable()
        {
            _instance = null;
        }

        private static void ValidateCaller(string callerName)
        {
            if (WarningCallers.Contains(callerName))
                DebugLogger.LogError($"It's not safe to call {nameof(ReferenceDistributor)} from {nameof(UnityEngine)}.{callerName}",
                                     _instance);
        }
    }
}
