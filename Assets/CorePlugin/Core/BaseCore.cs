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
using UnityEngine;

namespace CorePlugin.Core
{
    /// <summary>
    /// Base implementation of IManager.
    /// </summary>
    public abstract class BaseCore : MonoBehaviour, ICore
    {
        [PrefabHeader]
        [SerializeField] [PrefabRequired] [CoreManagerElementsField]
        private protected List<GameObject> elements;

        public virtual void InitializeElements()
        {
            foreach (var o in elements.Select(m => Instantiate(m, transform))) DebugLogger.Log($"Create element: {o.name}");
        }
    }
}
