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
using CorePlugin.Attributes.EditorAddons;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.Extensions
{
    public static class ReflectionExtensions
    {
        #if UNITY_EDITOR
        public const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic |
                                          BindingFlags.Static | BindingFlags.Instance |
                                          BindingFlags.DeclaredOnly;

        public static IEnumerable<KeyValuePair<FieldInfo, IEnumerable<T>>> GetFieldsAttributes<T>(this Type t)
            where T : Attribute
        {
            return t == null
                       ? Enumerable.Empty<KeyValuePair<FieldInfo, IEnumerable<T>>>()
                       : t.GetFields(Flags)
                          .ToDictionary(key => key, value => value.GetCustomAttributes(true).OfType<T>())
                          .Concat(GetFieldsAttributes<T>(t.BaseType));
        }

        public static IEnumerable<T> GetFields<T>(this object obj)
        {
            return obj.GetType().GetFields(Flags).Where(x => x.FieldType == typeof(T)).Select(x => (T)x.GetValue(obj));
        }

        public static T GetField<T>(this object obj, string name)
        {
            return (T)obj.GetType().GetField(name, Flags)?.GetValue(obj);
        }

        public static void SetValue(this object obj, string name, object value)
        {
            obj.GetType().GetField(name, Flags)?.SetValue(obj, value);
        }

        public static string PrettyMemberName(this MemberInfo input)
        {
            return input.Name.PrettyCamelCase();
        }

        public static Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>>
            GetSortedMethodAttributes(this Type type)
        {
            var methodButtonsAttributes =
                new Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>>();

            foreach (var pair in type.GetMethodsAttributes<EditorButtonAttribute>())
            {
                foreach (var attribute in pair.Value)
                    if (methodButtonsAttributes.ContainsKey(attribute.CaptureGroup))
                    {
                        var list = methodButtonsAttributes[attribute.CaptureGroup];
                        list = list.Append(new KeyValuePair<MethodInfo, EditorButtonAttribute>(pair.Key, attribute));
                        methodButtonsAttributes[attribute.CaptureGroup] = list.OrderBy(x => x.Value.Priority);
                    }
                    else
                    {
                        methodButtonsAttributes.Add(attribute.CaptureGroup,
                                                    new List<KeyValuePair<MethodInfo, EditorButtonAttribute>>
                                                    {
                                                        new KeyValuePair<MethodInfo,
                                                            EditorButtonAttribute>(pair.Key, attribute)
                                                    });
                    }
            }
            return methodButtonsAttributes.OrderBy(x => x.Key).ToDictionary(x => x.Key, y => y.Value);
        }

        public static IEnumerable<object> GetEnumerableOfType<T>(this T type)
        {
            if (!typeof(T).IsInterface &&
                !typeof(T).IsClass)
                throw new ArgumentException($"{type} should be class or interface");

            var objects = Assembly.GetAssembly(typeof(T)).GetTypes()
                                  .Where(myType => myType.IsClass && !myType.IsAbstract && !myType.IsInterface &&
                                                   myType.IsSubclassOf(typeof(T)))
                                  .Select(Activator.CreateInstance).ToList();
            return objects;
        }

        public static IEnumerable<Object> GetComponentsWithAttribute<T>(this GameObject gameObject) where T : Attribute
        {
            return gameObject.GetComponents<Component>().Where(x => GetBaseTypes(x.GetType()).Any(t => t.GetCustomAttribute<T>(false) != null));
        }

        public static IEnumerable<Object> GetComponentsWithAttribute<T>(this MonoBehaviour monoBehaviour) where T : Attribute
        {
            return monoBehaviour.GetComponents<Component>().Where(x => GetBaseTypes(x.GetType()).Any(t => t.GetCustomAttribute<T>(false) != null));
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            if (type == null) return Enumerable.Empty<Type>();
            var empty = Enumerable.Empty<Type>();
            return empty.Append(type).Concat(type.GetInterfaces()).Concat(GetBaseTypes(type.BaseType));
        }

        public static object GetFieldWithAttribute<T>(this Type type, Object instance) where T : Attribute
        {
            if (type == null) return null;
            var fieldInfos = type.GetFields(Flags);

            var field = fieldInfos.FirstOrDefault(x => x.GetCustomAttribute<T>() != null)?.GetValue(instance) ??
                        type.BaseType.GetFieldWithAttribute<T>(instance);
            return field;
        }

        public static bool IsPrefab(this Object obj)
        {
            return PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab &&
                   PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab;
        }

        public static bool IsSceneObject(this Object obj)
        {
            return PrefabUtility.GetPrefabInstanceStatus(obj) != PrefabInstanceStatus.NotAPrefab &&
                   PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.NotAPrefab;
        }

        public static IEnumerable<KeyValuePair<MethodInfo, IEnumerable<T>>> GetMethodsAttributes<T>(this Type t)
            where T : Attribute
        {
            return t == null
                       ? Enumerable.Empty<KeyValuePair<MethodInfo, IEnumerable<T>>>()
                       : t.GetMethods(Flags).Where(x => x.GetCustomAttributes<T>().Any())
                          .ToDictionary(key => key, value => value.GetCustomAttributes<T>(true))
                          .Concat(GetMethodsAttributes<T>(t.BaseType));
        }

        public static IEnumerable<T> GetClassAttributes<T>(this Type t) where T : Attribute
        {
            return t == null
                       ? Enumerable.Empty<T>()
                       : t.GetCustomAttributes().OfType<T>().Concat(GetClassAttributes<T>(t.BaseType));
        }
        #endif
    }
}
