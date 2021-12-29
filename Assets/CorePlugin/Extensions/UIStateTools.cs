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
using UnityEngine;

namespace CorePlugin.Extensions
{
    /// <summary>
    /// UI state tool for canvas groups
    /// </summary>
    public static class UIStateTools
    {
        /// <summary>
        /// Changing canvas visibility and interactivity
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isVisible"></param>
        public static void ChangeGroupState(this CanvasGroup group, bool isVisible)
        {
            group.alpha = isVisible ? 1 : 0;
            group.interactable = isVisible;
            group.blocksRaycasts = isVisible;
        }

        /// <summary>
        /// Changing state of mouse cursor
        /// </summary>
        /// <param name="state"></param>
        public static void ChangeCursorState(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = state;
        }

        /// <summary>
        /// Changing canvas visibility and interactivity after delay
        /// </summary>
        /// <param name="group"></param>
        /// <param name="isVisible"></param>
        /// <param name="delay"></param>
        public static IEnumerator ChangeGroupState(CanvasGroup group, bool isVisible, float delay)
        {
            group.alpha = isVisible ? 1 : 0;
            group.blocksRaycasts = isVisible;
            yield return new WaitForSeconds(delay);
            group.interactable = isVisible;
        }
    }
}
