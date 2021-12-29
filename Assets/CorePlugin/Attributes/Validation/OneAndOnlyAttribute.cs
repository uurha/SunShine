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
using Object = UnityEngine.Object;

namespace CorePlugin.Attributes.Validation
{
    /// <summary>
    /// Attribute validating whether there is only one copy of this class in the scene.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class OneAndOnlyAttribute : ClassValidationAttribute
    {
        private Type _type;

        public OneAndOnlyAttribute(bool showError) : base(showError)
        {
        }

        public OneAndOnlyAttribute() : base(false)
        {
        }

        public override bool Validate(Object instance)
        {
            bool isValid;

            try
            {
                isValid = !ValidState(instance);
                _error = ErrorText();
            }
            catch (Exception e)
            {
                _error = e.Message;
                isValid = false;
            }
            return isValid;
        }

        private protected override bool ValidState(object obj)
        {
            var mono = Object.FindObjectsOfType<MonoBehaviour>();
            _type = obj.GetType();
            var count = mono.Count(type => obj.GetType() == type.GetType());
            return count > 1;
        }

        private protected override string ErrorText()
        {
            return $"Should be only one instance of <i>{_type}</i> in scene";
        }
    }
}
