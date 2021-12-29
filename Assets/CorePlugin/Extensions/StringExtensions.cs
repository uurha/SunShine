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
    public static class StringExtensions
    {
        /// <summary>
        /// Makes first char upper
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string FirstCharToUpper(this string input)
        {
            return input switch
                   {
                       null => throw new ArgumentNullException(nameof(input)),
                       "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                       _ => input.First().ToString().ToUpper() + input.Substring(1)
                   };
        }
    }
}
