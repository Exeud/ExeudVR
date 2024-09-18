/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using UnityEngine.Video;

namespace ExeudVR
{
    /// <summary>
    /// A simple script that demonstrates how to connect the DoubleClick function to play and pause either audio or video streams.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Media/MediaPlayerBehaviour.md"/>
    /// </summary>
    public class MediaPlayerBehaviour : MonoBehaviour
    {
        public void OnDoubleClick()
        {
            if (GetComponent<AudioSource>()) ToggleAudio();
            if (GetComponent<VideoPlayer>()) ToggleVideo();
        }

        public void ToggleAudio()
        {
            if (!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            else
            {
                gameObject.GetComponent<AudioSource>().Pause();
            }
        }

        public void ToggleVideo()
        {
            VideoPlayer vp = gameObject.GetComponent<VideoPlayer>();

            if (vp != null && vp.isPrepared)
            {
                if (vp.isPlaying)
                {
                    vp.Pause();
                }
                else
                {
                    vp.Play();
                }
            }
            else
            {
                // do nothing, the video is not ready yet
            }
        }
    }
}