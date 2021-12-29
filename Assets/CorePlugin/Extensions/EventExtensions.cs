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
using System.Linq;

namespace CorePlugin.Extensions
{
    public static class EventExtensions
    {
        [Obsolete("Use <i>EventExtensions.Subscribe</i> or <i>EventExtensions.Unsubscribe</i>", true)]
        public static T Combine<T>(this Delegate[] subscribers) where T : Delegate
        {
            return subscribers.OfType<T>()
                              .Aggregate<T, T>(null, (current, dDelegate) => (T)Delegate.Combine(current, dDelegate));
        }

        /// <summary>
        /// Aggregating delegates
        /// </summary>
        /// <param name="dDelegate"></param>
        /// <param name="aggregateMethod"></param>
        /// <param name="delegates"></param>
        /// <typeparam name="T"></typeparam>
        public static void Aggregate<T>(ref T dDelegate, Func<Delegate, Delegate, Delegate> aggregateMethod, params Delegate[] delegates)
            where T : Delegate
        {
            if (delegates == null) throw new ArgumentNullException($"{nameof(delegates)} should be null");
            if (delegates.Length <= 0) throw new ArgumentOutOfRangeException($"{nameof(delegates)}.Length should not be zero");
            dDelegate = delegates.OfType<T>().Aggregate(dDelegate, (current, delegate1) => (T)aggregateMethod.Invoke(current, delegate1));
        }

        /// <summary>
        /// Subscribing delegates
        /// </summary>
        /// <param name="dDelegate"></param>
        /// <param name="delegates"></param>
        /// <typeparam name="T"></typeparam>
        public static void Subscribe<T>(ref T dDelegate, params Delegate[] delegates) where T : Delegate
        {
            Aggregate(ref dDelegate, Delegate.Combine, delegates);
        }

        /// <summary>
        /// Unsubscribes delegates
        /// </summary>
        /// <param name="dDelegate"></param>
        /// <param name="delegates"></param>
        /// <typeparam name="T"></typeparam>
        public static void Unsubscribe<T>(ref T dDelegate, params Delegate[] delegates) where T : Delegate
        {
            Aggregate(ref dDelegate, Delegate.Remove, delegates);
        }

        /// <summary>
        /// Unsubscribes all delegates
        /// </summary>
        /// <param name="dDelegate"></param>
        /// <param name="delegates"></param>
        /// <typeparam name="T"></typeparam>
        public static void UnsubscribeAll<T>(ref T dDelegate, params Delegate[] delegates) where T : Delegate
        {
            Aggregate(ref dDelegate, Delegate.RemoveAll, delegates);
        }
    }
}
