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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CorePlugin.Attributes.EditorAddons;
using CorePlugin.Editor.Extensions;
using CorePlugin.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.Editor.Windows
{
    internal static class SelectorWindowExtensions
    {
        internal static readonly string[] StringsToRemove = { "(Clone)", "Core" };

        private static IEnumerable<Object> GetCoreElementsRecursively(this Object instance)
        {
            var fields = GetObjectsInField(instance.GetType(), instance).ToArray();
            var empty = Enumerable.Empty<Object>();
            if (!fields.Any()) return empty;

            foreach (var field in fields)
                switch (field)
                {
                    case IEnumerable enumerable:
                    {
                        var list = enumerable.Cast<GameObject>().ToList();
                        if (!list.Any()) continue;

                        var elements =
                            list.SelectMany(x => x.GetComponentsWithAttribute<CoreManagerElementAttribute>());
                        if (EditorApplication.isPlaying) elements = elements.Select(x => Object.FindObjectOfType(x.GetType()));
                        var objects = elements.ToList();

                        var aggregate = objects.Aggregate(objects,
                                                          (current, element) =>
                                                              current.Concat(GetCoreElementsRecursively(element))
                                                                     .ToList());
                        empty = empty.Concat(aggregate);
                        break;
                    }
                    case Object obj:
                    {
                        if (obj is MonoBehaviour gameObject)
                        {
                            var elements = gameObject.GetComponentsWithAttribute<CoreManagerElementAttribute>();
                            if (EditorApplication.isPlaying) elements = elements.Select(x => Object.FindObjectOfType(x.GetType()));
                            var objects = elements.ToList();

                            var aggregate = objects.Aggregate(objects,
                                                              (current, element) =>
                                                                  current.Concat(GetCoreElementsRecursively(element))
                                                                         .ToList());
                            empty = empty.Concat(aggregate);
                        }
                        break;
                    }
                }
            return empty;
        }

        private static IEnumerable<object> GetObjectsInField(this Type type, Object instance)
        {
            if (type == null) return Enumerable.Empty<object>();
            var fieldInfos = type.GetFields(ReflectionExtensions.Flags);

            var fields = fieldInfos.Where(x =>
                                          {
                                              var fieldAttribute =
                                                  x.GetCustomAttribute<CoreManagerElementsFieldAttribute>();

                                              return fieldAttribute != null &&
                                                     fieldAttribute.CheckFlag(EditorApplication.isPlaying
                                                                                  ? FieldType.PlayMode
                                                                                  : FieldType.EditorMode);
                                          }
                                         ).Select(x => x.GetValue(instance))
                                   .Concat(GetObjectsInField(type.BaseType, instance));
            return fields;
        }

        internal static IEnumerable<NamedGroup> ElementGathering(this Type type, Object instance)
        {
            var fields = GetObjectsInField(type, instance).ToArray();
            var empty = Enumerable.Empty<NamedGroup>();
            if (!fields.Any()) return empty;

            foreach (var field in fields)
                switch (field)
                {
                    case IEnumerable enumerable:
                    {
                        var list = enumerable.Cast<GameObject>().ToList();
                        if (!list.Any()) continue;

                        var elements =
                            list.SelectMany(x => x.GetComponentsWithAttribute<CoreManagerElementAttribute>());
                        if (EditorApplication.isPlaying) elements = elements.Select(x => Object.FindObjectOfType(x.GetType()));

                        empty = elements.Aggregate(empty,
                                                   (current, element) =>
                                                       current.Append(new NamedGroup(element,
                                                                                     GetCoreElementsRecursively(element))));
                        break;
                    }
                    case Object obj:
                    {
                        if (obj is GameObject gameObject)
                        {
                            var elements =
                                gameObject.GetComponentsWithAttribute<CoreManagerElementAttribute>();
                            if (EditorApplication.isPlaying) elements = elements.Select(x => Object.FindObjectOfType(x.GetType()));

                            empty = elements.Aggregate(empty,
                                                       (current, element) =>
                                                           current.Append(new NamedGroup(element,
                                                                                         GetCoreElementsRecursively(element))));
                        }
                        break;
                    }
                }
            return empty;
        }

        internal class NamedObject : Named<Object, string>
        {
            public NamedObject(Object obj) : base(obj, obj.PrettyTypeName(StringsToRemove))
            {
            }

            public string Name => value;
            public Object Object => key;
        }

        internal class NamedGroup : Named<NamedObject, List<NamedObject>>
        {
            public NamedGroup(Object namedObject, IEnumerable<Object> list) : base(new NamedObject(namedObject),
                                                                                   list.Select(item => new NamedObject(item)).ToList())
            {
            }

            public Object Object => key.Object;
            public NamedObject NamedObject => key;
            public List<NamedObject> NamedObjects => value;
        }
    }
}
