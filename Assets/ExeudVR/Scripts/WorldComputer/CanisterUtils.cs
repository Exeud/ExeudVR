/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Runtime.InteropServices;

namespace ExeudVR
{
    public static class CanisterUtilsInternal
    {
        [DllImport("__Internal")]
        public static extern void ICLogin(int cbIndex);

        [DllImport("__Internal")]
        public static extern void ICLogout(int cbIndex);
    }

    /// <summary>
    /// Template for linking functionality to the exeudvr-canister template, specifically UnityFunctions.ts.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/WorldComputer/CanisterUtils.md"/>
    /// </summary>
    public static class CanisterUtils
    {
        public static void StartIIAuth(int cbIndex)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        CanisterUtilsInternal.ICLogin(cbIndex);
#endif
        }

        public static void EndIISession(int cbIndex)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        CanisterUtilsInternal.ICLogout(cbIndex);
#endif
        }

    }
}