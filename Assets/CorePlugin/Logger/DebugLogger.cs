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
using System.Diagnostics;
using CorePlugin.Extensions;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace CorePlugin.Logger
{
    /// <summary>
    /// Custom logger solution for logs.
    /// <remarks>
    /// Logs in this class are dependant on <see langword="DEBUG"/> and <see langword="ENABLE_RELEASE_LOGS"/>. 
    /// If <see langword="ENABLE_RELEASE_LOGS"/> defined logs will displayed in Release Build.
    /// Otherwise only Editor and Developer Build will display logs.
    /// For defining preprocessor open CoreManager or write down in PlayerSettings in field "Scripting Define Symbols".
    /// It's fully stripped from Release Builds.
    /// </remarks>
    /// <seealso cref="CorePlugin.Core.CoreManager"/>
    /// </summary>
    public static class DebugLogger
    {
        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void Log(string message, Object context)
        {
            Debug.Log(message, context);
        }
        
        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void Log(object obj)
        {
            Debug.Log(obj);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void Log(object obj, Object context)
        {
            Debug.Log(obj, context);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogWarning(string message, Object context)
        {
            Debug.LogWarning(message, context);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogError(string message, Object context)
        {
            Debug.LogError(message, context);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogError(Exception exception, Object context)
        {
            Debug.LogError(exception, context);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }

        [Conditional(EditorDefinition.Debug)] [Conditional(EditorDefinition.EnableReleaseLogs)]
        public static void LogException(Exception exception, Object context)
        {
            Debug.LogException(exception, context);
        }
    }
}
