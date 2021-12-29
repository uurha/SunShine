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

using System.Collections;
using CorePlugin.Singletons;
using UnityEngine;

namespace CorePlugin.Dispatchers
{
    public class StaticCoroutineDispatcher : StaticObjectSingleton<StaticCoroutineDispatcher>
    {

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Start coroutine on CoroutineDispatcher
        /// </summary>
        /// <param name="coroutine"></param>
        /// <returns></returns>
        public static Coroutine StartStaticCoroutine(IEnumerator coroutine)
        {
            return GetInstance().StartCoroutine(coroutine);
        }
    }
}
