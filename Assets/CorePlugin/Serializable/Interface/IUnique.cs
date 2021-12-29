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

namespace CorePlugin.Serializable.Interface
{
    /// <summary>
    /// Interface for unique objects.
    /// For example, you can use it for objects that need to be saved in Json or objects with the same data, but with different identifiers
    /// </summary>
    public interface IUnique
    {
        public string Guid { get; }

        public bool Equals(IUnique item);
    }

}
