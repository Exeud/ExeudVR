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
    /// Contains functionality for VR toggle and settings buttons. Handles settings animation.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/ObjectBehaviour/FollowerBehaviour.md"/>
    /// </summary>
    public class FollowerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPane;

        private float settYPos = 0.15f;
        private float settYScale = 0.0f;

        public bool settingsOpen { get; private set; } = false;

        public void ToggleSettings()
        {
            if (!settingsOpen)
            {
                settYPos = 0.35f;
                settYScale = 0.3f;
            }
            else
            {
                settYPos = 0.15f;
                settYScale = 0.0f;
            }

            Vector3 targetPos = new Vector3(-0.1f, settYPos, 0f);
            Vector3 targetScale = new Vector3(0.3f, settYScale, 0.01f);
            settingsOpen = !settingsOpen;

            StartCoroutine(MoveAndScalePane(targetPos, targetScale, 0.3f));
        }

        public void ToggleVR()
        {
            PlatformManager.Instance.StartVR();
        }

        private IEnumerator MoveAndScalePane(Vector3 targetPos, Vector3 targetScale, float speed)
        {
            if (settingsOpen)
            {
                settingsPane.SetActive(true);
            }

            Transform settingsRect = settingsPane.GetComponent<Transform>();

            float rate = 1.0f / Vector3.Distance(settingsRect.localPosition, targetPos) * speed;
            float t = 0.0f;
            while (t < 1.0)
            {
                t += Time.deltaTime * rate;
                settingsRect.localPosition = Vector3.Lerp(settingsRect.localPosition, targetPos, Mathf.SmoothStep(0.0f, 1.0f, t));
                settingsRect.localScale = Vector3.Lerp(settingsRect.localScale, targetScale, Mathf.SmoothStep(0.0f, 1.0f, t));
                yield return null;
            }

            if (!settingsOpen)
            {
                settingsPane.SetActive(false);
            }
        }
    }
}