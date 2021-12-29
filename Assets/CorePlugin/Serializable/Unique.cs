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
using CorePlugin.Serializable.Interface;
using UnityEngine;

namespace CorePlugin.Serializable
{
    /// <summary>
    /// Base class for classes that need to be saved in Json file.
    /// </summary>
    [Serializable]
    public class Unique : IUnique
    {
        private string _guid;

        public string Guid => _guid;

        public Unique()
        {
            _guid = System.Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }

        public bool Equals(IUnique item)
        {
            return Guid.Equals(item.Guid);
        }
    }
}
