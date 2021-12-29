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
using CorePlugin.Attributes.Validation;
using CorePlugin.Cross.SceneData.Interface;
using CorePlugin.Singletons;

namespace CorePlugin.Cross.SceneData
{
    /// <summary>
    /// Singleton for passing data between scenes
    /// <seealso cref="CorePlugin.Cross.SceneData.Interface.ISceneData"/>
    /// </summary>
    [OneAndOnly]
    public class SceneDataHandler : StaticObjectSingleton<SceneDataHandler>
    {
        private readonly Dictionary<Type, ISceneData> _data = new Dictionary<Type, ISceneData>();

        private void Awake()
        {
            if (_instance != null &&
                _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// Adding data to dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddData<T>(T data) where T : ISceneData, new()
        {
            GetInstance().AddDataInternal(data);
        }

        /// <summary>
        /// Adding data to dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        private void AddDataInternal<T>(T data) where T : ISceneData, new()
        {
            var isContains = _data.ContainsKey(typeof(T));

            if (isContains)
                _data[typeof(T)] = data;
            else
                _data.Add(typeof(T), data);
        }

        /// <summary>
        /// Getting data from dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        public static bool GetData<T>(out T data) where T : ISceneData, new()
        {
            return GetInstance().GetDataInternal(out data);
        }

        /// <summary>
        /// Getting data from dictionary by passed Type
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        private bool GetDataInternal<T>(out T data) where T : ISceneData, new()
        {
            var isGet = _data.TryGetValue(typeof(T), out var buffer);
            data = (T)buffer;
            return isGet;
        }

        /// <summary>
        /// Removing data from dictionary by passed Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void RemoveData<T>() where T : ISceneData, new()
        {
            GetInstance().RemoveDataInternal<T>();
        }

        /// <summary>
        /// Removing data from dictionary by passed Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RemoveData<T>(T data) where T : ISceneData, new()
        {
            GetInstance().RemoveDataInternal(data);
        }

        /// <summary>
        /// Removing data from dictionary by passed Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RemoveDataInternal<T>() where T : ISceneData, new()
        {
            if (_data.ContainsKey(typeof(T))) _data.Remove(typeof(T));
        }

        /// <summary>
        /// Removing data from dictionary by passed Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void RemoveDataInternal<T>(T data) where T : ISceneData, new()
        {
            if (_data.ContainsValue(data)) _data.Remove(typeof(T));
        }
    }
}
