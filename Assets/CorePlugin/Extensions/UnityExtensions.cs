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
using System.Text.RegularExpressions;
using CorePlugin.Serializable.Interface;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.Extensions
{
    /// <summary>
    /// Extension class for default Unity classes
    /// </summary>
    public static class UnityExtensions
    {
        /// <summary>
        /// Checks whether the left item is null and doesn't equal right item
        /// </summary>
        /// <param name="lci">Left compare item</param>
        /// <param name="rci">Right compare item</param>
        /// <returns></returns>
        public static bool IsNotNullAndNotEqual(this IUnique lci, IUnique rci)
        {
            return lci?.Equals(rci) == false;
        }

        public static void Add<TKey, TValue>(this List<Named<TKey, TValue>> list, TKey key, TValue value)
        {
            list.Add(new Named<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Gets object name and converts into CamelCase
        /// </summary>
        /// <param name="input"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string PrettyObjectName(this Object input, params string[] remove)
        {
            if (remove == null) return input.name.PrettyCamelCase();
            foreach (var s in remove) input.name = input.name.Replace(s, string.Empty);
            return input.name.PrettyCamelCase();
        }

        public static string PrettyCamelCase(this string input)
        {
            return Regex.Replace(input.Replace("_", ""), "((?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]))", " $1").Trim();
        }

        public static string ToTitleCase(this string input)
        {
            return Regex.Replace(input, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
        }

        /// <summary>
        /// Removing range of items from list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> RemoveRange<T>(this List<T> list, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable) list.Remove(item);
            return list;
        }

        /// <summary>
        /// Puts the string into the Clipboard.
        /// </summary>
        public static void CopyToClipboard(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
        }

        /// <summary>
        /// Checks whether the left item is null and equals right item
        /// </summary>
        /// <param name="lci">Left compare item</param>
        /// <param name="rci">Right compare item</param>
        /// <returns></returns>
        public static bool IsNotNullAndEqual(this IUnique lci, IUnique rci)
        {
            return lci?.Equals(rci) == true;
        }

        /// <summary>
        /// Clearing list and destroying its items
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Clear<T>(ref List<T> list) where T : MonoBehaviour
        {
            foreach (var item in list) Object.Destroy(item.gameObject);
            list.Clear();
        }

        public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue)
            where T : IUnique
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            return source.Select(x => EqualityComparer<T>.Default.Equals(x, oldValue) ? newValue : x);
        }

        public static IList<T> ReplaceOrAdd<T>(this IList<T> source, IEnumerable<T> newValues) where T : IUnique
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var serializables = newValues as T[] ?? newValues.ToArray();

            for (var i = 0; i < serializables.Count(); i++)
            {
                var foundIndex = source.Replace(serializables[i]);
                if (foundIndex == -1) source.Add(serializables[i]);
            }
            return source;
        }

        public static int Replace<T>(this IList<T> source, T newValue) where T : IUnique
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var findItem = source.FirstOrDefault(x => x.Equals(newValue));
            var index = findItem == null ? -1 : source.IndexOf(findItem);
            if (index != -1) source[index] = newValue;
            return index;
        }

        /// <summary>
        /// Trying to find object on scene that was inherited from T
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectOfType<T>(out T result)
        {
            result = default;
            if (TryToFindObjectsOfType(out IEnumerable<T> bufferResults)) result = bufferResults.FirstOrDefault();
            return result != null;
        }

        /// <summary>
        /// Trying to find objects on scene that were inherited from T
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectsOfType<T>(out IEnumerable<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            return result.Any();
        }

        /// <summary>
        /// Trying to find objects on scene that were inherited from T and return a list of said objects
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryToFindObjectsOfType<T>(out IList<T> result)
        {
            result = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToList();
            return result != null;
        }
    }
}
