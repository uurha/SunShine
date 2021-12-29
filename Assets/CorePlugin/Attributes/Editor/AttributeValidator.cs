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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CorePlugin.Attributes.Base;
using CorePlugin.Editor.Helpers;
using CorePlugin.Logger;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.Attributes.Editor
{
    internal static class AttributeValidator
    {
        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic |
                                           BindingFlags.Static | BindingFlags.Instance |
                                           BindingFlags.DeclaredOnly;

        public static IEnumerable<FieldInfo> GetAllFields(Type t)
        {
            return t == null ? Enumerable.Empty<FieldInfo>() : t.GetFields(Flags).Concat(GetAllFields(t.BaseType));
        }

        public static IEnumerable<Attribute> GetAllAttributes(Type t)
        {
            return t == null
                       ? Enumerable.Empty<Attribute>()
                       : t.GetCustomAttributes(typeof(ClassValidationAttribute)).Concat(GetAllAttributes(t.BaseType));
        }

        public static IEnumerable<ErrorObjectPair> ErrorObjectPairs(GameObject o)
        {
            var behaviours = o.GetComponentsInChildren<MonoBehaviour>(true);
            var listErrors = new List<ErrorObjectPair>();

            foreach (var b in behaviours)
            {
                var fields = GetAllFields(b.GetType());

                foreach (var f in fields)
                {
                    var atts = f.GetCustomAttributes(typeof(FieldValidationAttribute), true);

                    foreach (var a in atts)
                    {
                        if (!(a is FieldValidationAttribute vatt) ||
                            vatt.Validate(f, b))
                            continue;
                        listErrors.Add(new ErrorObjectPair(vatt.ErrorMessage, b));
                        if (vatt.ShowError) ShowError(vatt.ErrorMessage, b);
                    }
                }
                var attributes = GetAllAttributes(b.GetType());

                foreach (var a in attributes)
                {
                    if (!(a is ClassValidationAttribute vatt) ||
                        vatt.Validate(b))
                        continue;
                    listErrors.Add(new ErrorObjectPair(vatt.ErrorMessage, b));
                    if (vatt.ShowError) ShowError(vatt.ErrorMessage, b);
                }
            }
            return listErrors;
        }

        public static IEnumerable<ErrorObjectPair> ErrorObjectPairs()
        {
            var behaviours = Object.FindObjectsOfType<MonoBehaviour>();
            var listErrors = new List<ErrorObjectPair>();

            foreach (var b in behaviours)
            {
                var fields = GetAllFields(b.GetType());

                foreach (var f in fields)
                {
                    var atts = f.GetCustomAttributes(typeof(FieldValidationAttribute), true);

                    foreach (var a in atts)
                    {
                        if (!(a is FieldValidationAttribute vatt) ||
                            vatt.Validate(f, b))
                            continue;
                        listErrors.Add(new ErrorObjectPair(vatt.ErrorMessage, b));
                        if (vatt.ShowError) ShowError(vatt.ErrorMessage, b);
                    }
                }
                var attributes = GetAllAttributes(b.GetType());

                foreach (var a in attributes)
                {
                    if (!(a is ClassValidationAttribute vatt) ||
                        vatt.Validate(b))
                        continue;
                    listErrors.Add(new ErrorObjectPair(vatt.ErrorMessage, b));
                    if (vatt.ShowError) ShowError(vatt.ErrorMessage, b);
                }
            }
            return listErrors;
        }

        public static void ShowError(string msg, Object o)
        {
            DebugLogger.LogError(msg.Replace("\n", " "), o);
        }
    }
}
