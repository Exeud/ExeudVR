/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using Newtonsoft.Json;
using UnityEngine;

namespace ExeudVR {

    /// <summary>
    // Canister response interrogation example.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/WorldComputer/ChainUtils.md"/>
    /// </summary>
    public class ChainUtils : MonoBehaviour
    {
        public static void InterrogateCanisterResponse(string jsonData)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<CanisterResponseError>(jsonData);
                if (response == null)
                {
                    Debug.LogError("Unable to parse CanisterResponse. Result was null");
                    return;
                }

                if (!string.IsNullOrEmpty(response.ErrorCode))
                {
                    Debug.Log("Error code: " + response.ErrorCode);
                    return;
                }

                if (!string.IsNullOrEmpty(response.RejectMessage))
                {
                    Debug.Log("Reject Info :\n" + response.RejectCode.ToString() + 
                        ": " + response.RejectMessage);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Interrogation result:\n" + ex.Message);
            }
        }

    }
}