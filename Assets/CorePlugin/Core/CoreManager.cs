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
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Attributes.Headers;
using CorePlugin.Attributes.Validation;
using CorePlugin.Core.Interface;
using CorePlugin.Logger;
using CorePlugin.ReferenceDistribution;
using UnityEngine;

namespace CorePlugin.Core
{
    /// <summary>
    /// Manager for initialization of sub manager in the scene.
    /// <seealso cref="CorePlugin.ReferenceDistribution.ReferenceDistributor"/>
    /// <seealso cref="CorePlugin.Core.Interface.ICore"/>
    /// </summary>
    [OneAndOnly]
    public class CoreManager : MonoBehaviour
    {
        [ReferencesHeader]
        [SerializeField] [NotNull]
        private ReferenceDistributor referenceDistributor;

        [PrefabHeader]
        [SerializeField] [PrefabRequired] [HasComponent(typeof(ICore))]
        [CoreManagerElementsField]
        private List<GameObject> managers;

        private void Awake()
        {
            //Instantiate all managers.
            InitializeManagers();
            referenceDistributor.Initialize();
        }

        private void Start()
        {
            EventInitializer.InitializeSubscriptions();
            EventInitializer.InvokeBase();
        }

        /// <summary>
        /// Create and initialize managers from the list.
        /// </summary>
        private void InitializeManagers()
        {
            foreach (var o in managers.Select(m => Instantiate(m, transform)))
            {
                DebugLogger.Log($"Create manager: {o.name}");
                if (!o.TryGetComponent(out ICore manager)) continue;
                manager.InitializeElements();
            }
        }
    }
}
