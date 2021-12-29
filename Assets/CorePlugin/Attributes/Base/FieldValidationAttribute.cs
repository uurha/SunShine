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
using System.Collections;
using System.Reflection;
using Object = UnityEngine.Object;

namespace CorePlugin.Attributes.Base
{
    /// <summary>
    /// Base attribute for field validation.
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class FieldValidationAttribute : ValidationAttribute
    {
        protected FieldValidationAttribute(bool showError) : base(showError)
        {
        }

        public virtual bool Validate(FieldInfo field, Object instance)
        {
            bool isValid;

            try
            {
                var value = field.GetValue(instance);

                if (value is IEnumerable list)
                {
                    isValid = true;
                    var index = -1;

                    foreach (var item in list)
                    {
                        index++;

                        if (!item.Equals(null) &&
                            ValidState(item))
                            continue;
                        isValid = false;

                        _error =
                            $"<b>Element[{index}]</b> {ErrorText()}\ninto Field: <i>{field.Name}</i>\non GameObject: {instance.name}";
                        break;
                    }
                }
                else
                {
                    _error =
                        $"Object <b>\"{((Object)value)?.name}\"</b> {ErrorText()}\ninto Field: <i>{field.Name}</i> on GameObject: {instance.name}";
                    isValid = value != null && ValidState(value);
                }
            }
            catch (Exception e)
            {
                _error = e.Message;
                isValid = false;
            }
            return isValid;
        }
    }
}
