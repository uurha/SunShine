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

using System.Linq;
using UnityEngine;

namespace CorePlugin.Extensions
{
    public readonly struct MathfExtensions
    {
        public static Vector3 Average(params Vector3[] vectors)
        {
            var t = vectors.Aggregate(Vector3.zero, (current, vector3) => current + vector3);
            return t / vectors.Length;
        }

        public static Vector3 MiddlePoint(Vector3 start, Vector3 end)
        {
            var t = start + end;
            return t / 2;
        }

        public static Vector2 MiddlePoint(Vector2 start, Vector2 end)
        {
            var t = start + end;
            return t / 2;
        }
    }
}
