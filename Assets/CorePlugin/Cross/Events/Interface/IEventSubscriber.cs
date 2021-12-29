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

namespace CorePlugin.Cross.Events.Interface
{

    /// <summary>
    /// Interface for subscribers.
    /// <code>
    /// <br/>public IEnumerable<Delegate/> GetSubscribers()
    /// <br/>{
    /// <br/>    var list = new Delegate[] {(CrossEventTypes.DelegateClass) MyMethod, (CrossEventTypes.DelegateClass2) MyMethod2};
    /// <br/>    return list;
    /// <br/>}
    /// </code>
    /// </summary>
    /// <seealso cref="CorePlugin.Cross.Events.EventTypes"/>
    public interface IEventSubscriber
    {
        /// <summary>
        /// Returns IEnumerable with all methods which need to be subscribed.
        /// </summary>
        /// <returns></returns>
        public Delegate[] GetSubscribers();
    }
}
