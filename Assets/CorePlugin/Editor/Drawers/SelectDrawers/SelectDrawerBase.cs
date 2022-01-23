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
using CorePlugin.Attributes.EditorAddons.SelectAttributes;
using UnityEditor;
using UnityEngine;

namespace CorePlugin.Editor.Drawers.SelectDrawers
{
    public abstract class SelectDrawerBase<T> : PropertyDrawer where T : SelectAttributeBase
    {
        private protected bool _initializeFold;

        private protected List<Type> _reflectionType;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference) return;
            var att = (T)attribute;
            LazyGetAllInheritedType(att.GetFieldType() ?? fieldInfo.FieldType);
            var popupPosition = GetPopupPosition(position);

            var typePopupNameArray =
                _reflectionType.Select(type => type == null ? "null" : $"{type.Name} : {type}").ToArray();

            var typeFullNameArray = _reflectionType
                                   .Select(type => type == null
                                                       ? ""
                                                       : $"{type.Assembly.ToString().Split(',')[0]} {type.FullName}")
                                   .ToArray();

            //Get the type of serialized object
            var currentTypeIndex = Array.IndexOf(typeFullNameArray, property.managedReferenceFullTypename);

            if (currentTypeIndex <= -1 || currentTypeIndex >= typeFullNameArray.Length)
            {
                currentTypeIndex = 0;
            }
            
            var currentObjectType = _reflectionType[currentTypeIndex];
            var selectedTypeIndex = EditorGUI.Popup(popupPosition, currentTypeIndex, typePopupNameArray);
            ValidateType(property, selectedTypeIndex, currentObjectType);

            if (!_initializeFold)
            {
                property.isExpanded = currentTypeIndex != 0;
                _initializeFold = true;
            }
            EditorGUI.PropertyField(position, property, label, true);
        }

        private protected virtual string GetManagedReferenceFullTypename(SerializedProperty property)
        {
            return property.managedReferenceFullTypename;
        }

        private protected abstract void ValidateType(SerializedProperty property, int selectedTypeIndex, Type currentObjectType);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private protected void LazyGetAllInheritedType(Type baseType)
        {
            if (_reflectionType != null) return;

            _reflectionType = AppDomain.CurrentDomain.GetAssemblies()
                                       .SelectMany(s => s.GetTypes())
                                       .Where(p => baseType.IsAssignableFrom(p) && (p.IsClass || p.IsValueType))
                                       .ToList();
            _reflectionType.Insert(0, null);
        }

        private protected Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }
    }
}
