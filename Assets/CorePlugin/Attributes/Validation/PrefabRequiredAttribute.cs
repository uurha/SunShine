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
using System.Diagnostics;
using CorePlugin.Attributes.Base;
using CorePlugin.Extensions;
using Object = UnityEngine.Object;

namespace CorePlugin.Attributes.Validation
{
    /// <summary>
    /// Attribute validating whether the object or all items in list are prefabs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class PrefabRequiredAttribute : FieldValidationAttribute
    {
        public PrefabRequiredAttribute(bool showError) : base(showError)
        {
        }

        public PrefabRequiredAttribute() : base(false)
        {
        }

        private protected override bool ValidState(object obj)
        {
            #if UNITY_EDITOR
            return ((Object)obj).IsPrefab();
            #else
            return true;
            #endif
        }

        private protected override string ErrorText()
        {
            return "should not be instanced prefab";
        }
    }
}
