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
using System.IO;
using System.Threading;
using CorePlugin.Logger;
using CorePlugin.Serializable;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CorePlugin.FileSystem
{
    /// <summary>
    /// Class for saving Json file to disk.
    /// <seealso cref="CorePlugin.Serializable.Unique"/>
    /// </summary>
    public class FileSystem : IDisposable
    {
        private static SemaphoreSlim _semaphoreSlim;
        private readonly string _defaultExtension = ".json";
        private readonly string _defaultPath = Application.persistentDataPath;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FileSystem()
        {
            _semaphoreSlim ??= new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// Overloaded constructor which changes default path.
        /// </summary>
        /// <param name="path"></param>
        public FileSystem(string path) : this()
        {
            _defaultPath = path;
        }

        /// <summary>
        /// Overloaded constructor which changes default path and file extension.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        public FileSystem(string path, string extension) : this(path)
        {
            _defaultExtension = extension;
        }

        public void Dispose()
        {
            _semaphoreSlim.Dispose();
            _semaphoreSlim = null;
        }

        /// <summary>
        /// Saves class to the file with the name of class.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <param name="context">Required if errors should be shown on Object</param>
        /// <typeparam name="T"></typeparam>
        public void Save<T>(T data, Action<Exception> onError, Object context = null) where T : Unique
        {
            Save(data, typeof(T).Name, onError, context);
        }

        /// <summary>
        /// Saves your class to file.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="onError"></param>
        /// <param name="context">Required if errors should be shown on Object</param>
        /// <typeparam name="T"></typeparam>
        public void Save<T>(T data, string fileName, Action<Exception> onError, Object context = null) where T : Unique
        {
            SaveInternal(data, fileName, onError, context);
        }

        private async void SaveInternal<T>(T data, string fileName, Action<Exception> onError, Object context) where T : Unique
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var bufferPath = Path.Combine(_defaultPath, fileName, _defaultExtension);
                if (!Directory.Exists(bufferPath)) Directory.CreateDirectory(_defaultPath);
                using var stream = new FileStream(bufferPath, FileMode.OpenOrCreate);
                using var writer = new StreamWriter(stream);
                await writer.WriteAsync(data.ToString());
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                DebugLogger.LogError(e, context);
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        /// <summary>
        /// Loads file.
        /// </summary>
        /// <param name="onLoaded"></param>
        /// <param name="onError"></param>
        /// <param name="context">Required if errors should be shown on Object</param>
        /// <typeparam name="T"></typeparam>
        public void Load<T>(Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Unique
        {
            Load(typeof(T).Name, onLoaded, onError, context);
        }

        /// <summary>
        /// Loads file with different file name from passed class.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="onLoaded"></param>
        /// <param name="onError"></param>
        /// <param name="context">Required if errors should be shown on Object</param>
        /// <typeparam name="T"></typeparam>
        public void Load<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context = null) where T : Unique
        {
            LoadInternal(fileName, onLoaded, onError, context);
        }

        private async void LoadInternal<T>(string fileName, Action<T> onLoaded, Action<Exception> onError, Object context)
            where T : Unique
        {
            try
            {
                await _semaphoreSlim.WaitAsync();
                var bufferPath = Path.Combine(_defaultPath, fileName, _defaultExtension);

                if (!(Directory.Exists(_defaultPath) && File.Exists(bufferPath)))
                {
                    onLoaded?.Invoke(null);
                    return;
                }
                using var stream = new FileStream(bufferPath, FileMode.Open);
                using var writer = new StreamReader(stream);
                var loadedData = await writer.ReadToEndAsync();
                onLoaded?.Invoke(JsonUtility.FromJson<T>(loadedData));
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                DebugLogger.LogError(e, context);
                throw;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
