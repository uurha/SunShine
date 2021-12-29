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

namespace CorePlugin.Attributes.Validation
{
    /// <summary>
    /// Attribute validating whether field or element in the list equals null.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class NotNullAttribute : FieldValidationAttribute
    {
        public NotNullAttribute(bool showError) : base(showError)
        {
        }

        public NotNullAttribute() : base(false)
        {
        }

        private protected override bool ValidState(object obj)
        {
            return obj != null;
        }

        private protected override string ErrorText()
        {
            return "cannot be null";
        }
    }
}
