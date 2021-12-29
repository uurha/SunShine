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
using System.Threading;
using System.Threading.Tasks;
using CorePlugin.Singletons;
using UnityEngine;

namespace CorePlugin.Dispatchers
{
    public class MainThreadDispatcher : StaticObjectSingleton<MainThreadDispatcher>
    {
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
        private static readonly SemaphoreSlim ExecutionQueueLock = new SemaphoreSlim(1, 1);

        public static event Action OnDestroyEvent;

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

        private void Update()
        {
            ExecutionQueueLock.Wait();

            try
            {
                while (ExecutionQueue.Count > 0) ExecutionQueue.Dequeue().Invoke();
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }

        private static IEnumerator ActionWrapper(Action a)
        {
            a();
            yield return null;
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public static void Enqueue(IEnumerator action)
        {
            GetInstance().EnqueueInternal(action);
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        public static void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        /// <summary>
        /// Locks the queue and adds the Action to the queue, returning a Task which is completed when the action completes
        /// </summary>
        /// <param name="action">function that will be executed from the main thread.</param>
        /// <returns>A Task that can be awaited until the action completes</returns>
        public static Task EnqueueAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void WrappedAction()
            {
                try
                {
                    action();
                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }

            Enqueue(ActionWrapper(WrappedAction));
            return tcs.Task;
        }

        /// <summary>
        /// Locks the queue and adds the IEnumerator to the queue
        /// </summary>
        /// <param name="action">IEnumerator function that will be executed from the main thread.</param>
        public void EnqueueInternal(IEnumerator action)
        {
            ExecutionQueueLock.Wait();

            try
            {
                ExecutionQueue.Enqueue(() => { StartCoroutine(action); });
            }
            finally
            {
                ExecutionQueueLock.Release();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Initialize();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnDestroyEvent?.Invoke();
            OnDestroyEvent = null;
        }
    }
}
