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
using CorePlugin.Cross.Events.Interface;
using CorePlugin.Extensions;

namespace CorePlugin.Core
{
    /// <summary>
    /// Class responsible for reference initialization
    /// <seealso cref="CorePlugin.Cross.Events.Interface.IEventHandler"/>
    /// <seealso cref="CorePlugin.Cross.Events.Interface.IEventSubscriber"/>
    /// </summary>
    public static class EventInitializer
    {
        private static IList<IEventHandler> _handlers; //TODO: Better handlers storage

        /// <summary>
        /// Initialising cross subscriptions for all handlers in the scene.
        /// </summary>
        public static void InitializeSubscriptions()
        {
            if (!UnityExtensions.TryToFindObjectsOfType(out _handlers) ||
                !UnityExtensions.TryToFindObjectsOfType(out IList<IEventSubscriber> crossEventSubscribers))
                return;
            foreach (var crossEventHandler in _handlers) crossEventHandler.Subscribe(SelectSubscribers(crossEventSubscribers));
        }

        /// <summary>
        /// Subscribing event subscriber after scene has Awoken to event handlers.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void Subscribe(IEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Subscribe(subscriber.GetSubscribers());
        }

        /// <summary>
        /// Unsubscribing event subscriber after scene has Awoken from event handlers.
        /// </summary>
        /// <param name="subscriber"></param>
        public static void Unsubscribe(IEventSubscriber subscriber)
        {
            foreach (var crossEventHandler in _handlers) crossEventHandler.Unsubscribe(subscriber.GetSubscribers());
        }

        /// <summary>
        /// Adding new handler after scene has Awoken to list of event handlers.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="subscriptionsNeeded">If false invokeNeeded will not be called</param>
        /// <param name="invokeNeeded"></param>
        public static void AddHandler(IEventHandler handler, bool subscriptionsNeeded = true, bool invokeNeeded = false)
        {
            _handlers.Add(handler);
            if (!subscriptionsNeeded) return;

            if (UnityExtensions.TryToFindObjectsOfType(out IList<IEventSubscriber> crossEventSubscribers))
                handler.Subscribe(SelectSubscribers(crossEventSubscribers));
            if (invokeNeeded) handler.InvokeEvents();
        }

        private static Delegate[] SelectSubscribers(IEnumerable<IEventSubscriber> crossEventSubscribers)
        {
            return crossEventSubscribers.SelectMany(x => x.GetSubscribers()).ToArray();
        }

        /// <summary>
        /// Removing event handler after scene has Awoken from list
        /// </summary>
        /// <param name="handler"></param>
        public static void RemoveHandler(IEventHandler handler)
        {
            _handlers.Remove(handler);
        }

        /// <summary>
        /// Invoking event on handlers.
        /// </summary>
        public static void InvokeBase()
        {
            if (!UnityExtensions.TryToFindObjectsOfType(out IList<IEventHandler> handlers)) return;
            foreach (var handler in handlers) handler.InvokeEvents();
        }
    }
}
