/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;

/// <summary>
/// Watch face - time
/// </summary>
public class DisplayTime : MonoBehaviour
{
    public TextMesh text;
    private System.DateTime currentTime;
    private float lastCheck = 0;
    private float rate = 0.5f;

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Time.time >= lastCheck + rate)
        {
            currentTime = System.DateTime.UtcNow;
            lastCheck = Time.time;
            text.text = currentTime.ToString("HH:mm");
        }
    }
}
