﻿/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using UnityEngine;

/// <summary>
/// Watch face - fps
/// </summary>
public class FPSCounter : MonoBehaviour
{
    public TextMesh text;
    private float fps = 0;
    private float framesCount = 0;
    private float lastCheck = 0;
    private float rate = 0.5f;

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        framesCount++;
        if (Time.time >= lastCheck + rate)
        {
            fps = framesCount / (Time.time - lastCheck);
            lastCheck = Time.time;
            framesCount = 0;
            text.text = fps.ToString("F0");
        }
    }
}
