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
using CorePlugin.Extensions;
using UnityEditor;

namespace CorePlugin.Attributes.EditorAddons
{
    /// <summary>
    /// Provide component marked by this attribute to <seealso cref="Editor.Windows.CoreSelectorWindow"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class CoreManagerElementAttribute : DisplayNameAttribute
    {
        public CoreManagerElementAttribute()
        {
        }

        public CoreManagerElementAttribute(string displayName) : base(displayName)
        {
        }
    }

    [Flags]
    public enum FieldType
    {
        None = 0,
        PlayMode = 1,
        EditorMode = 2,
        Both = PlayMode | EditorMode
    }

    /// <summary>
    /// Provide data from object field marked by this attribute to <seealso cref="Editor.Windows.CoreSelectorWindow"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional(EditorDefinition.UnityEditor)]
    public class CoreManagerElementsFieldAttribute : Attribute
    {
        private readonly FieldType _fieldType;

        public CoreManagerElementsFieldAttribute(FieldType type = FieldType.Both)
        {
            _fieldType = type;
        }

        public bool CheckFlag(FieldType type)
        {
            var checkFlag = _fieldType.HasFlag(type);
            return checkFlag;
        }
    }
}
