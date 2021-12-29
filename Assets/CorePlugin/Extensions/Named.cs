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
using UnityEngine;

namespace CorePlugin.Extensions
{
    /// <summary>
    /// Replacement for dictionary in Unity Inspector
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class Named<TKey, TValue>
    {
        [SerializeField] protected TKey key;
        [SerializeField] protected TValue value;

        public TKey Key
        {
            get => key;
            set => key = value;
        }

        public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        public Named(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public Named()
        {
        }

        public static implicit operator KeyValuePair<TKey, TValue>(Named<TKey, TValue> obj)
        {
            return new KeyValuePair<TKey, TValue>(obj.Key, obj.Value);
        }
    }

    /// <summary>
    /// More complex list for dictionary
    /// </summary>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class Named<TName, TKey, TValue>
    {
        [SerializeField] protected TKey key;
        [SerializeField] protected TName name;
        [SerializeField] protected TValue value;

        public TName Name
        {
            get => name;
            set => name = value;
        }

        public TKey Key
        {
            get => key;
            set => key = value;
        }

        public TValue Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
