/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Collections;
using UnityEngine;

namespace ExeudVR
{
    /// <summary>
    /// Switches watch swatches
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/ObjectBehaviour/WatchFunctions/WatchController.md"/>
    /// </summary>
    public class WatchController : MonoBehaviour
    {
        [SerializeField] private GameObject[] modeCanvases;
        [SerializeField] private GameObject characterRoot;

        private int currentMode = 0;

        public void OnWake()
        {
            SetMode(currentMode);
        }

        public void OnSleep()
        {
            SetMode(-1);
        }

        public void ChangeMode(float trigVal)
        {
            currentMode++;
            if (currentMode > modeCanvases.Length - 1) currentMode = 0;
            SetMode(currentMode);
        }

        private void SetMode(int newMode)
        {
            for (int g = 0; g < modeCanvases.Length; g++)
            {
                modeCanvases[g].SetActive(g == newMode);
            }
        }
    }
}