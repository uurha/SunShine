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
using System.Linq;
using CorePlugin.Attributes.Base;
using CorePlugin.Extensions;
using UnityEngine;

namespace CorePlugin.Attributes.Validation
{
    /// <summary>
    /// Attribute validating whether Object in field or all objects in the list have desired component.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class HasComponentAttribute : FieldValidationAttribute
    {
        private readonly Type _requiredType;

        public HasComponentAttribute(Type requiredType, bool showError) : base(showError)
        {
            _requiredType = requiredType ?? throw new ArgumentNullException(nameof(requiredType));
        }

        public HasComponentAttribute(Type requiredType) : base(false)
        {
            _requiredType = requiredType ?? throw new ArgumentNullException(nameof(requiredType));
        }

        private protected override string ErrorText()
        {
            return $"should have component with type: <i>\"{_requiredType}\"</i>";
        }

        private protected override bool ValidState(object obj)
        {
            return ((GameObject)obj).GetComponents<MonoBehaviour>().Select(x => x.GetType()).Any(type => _requiredType.IsAssignableFrom(type));
        }
    }
}
