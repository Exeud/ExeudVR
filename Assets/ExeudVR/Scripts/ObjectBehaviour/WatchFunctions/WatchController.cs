using System.Collections;
using UnityEngine;

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

    public void ChangeMode()
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

    private IEnumerator LerpToTarget(GameObject objToLerp, Vector3 endPosition, Quaternion endRotation, float duration)
    {
        yield return new WaitForEndOfFrame();

        Transform t = objToLerp.transform;
        float time = 0;
        while (time < duration)
        {
            objToLerp.transform.position = Vector3.Lerp(t.position, endPosition, time / duration);
            objToLerp.transform.rotation = Quaternion.Slerp(t.rotation, endRotation, time / duration);

            time += Time.deltaTime;
            yield return null;
        }

        objToLerp.transform.position = endPosition;
        objToLerp.transform.rotation = endRotation;
    }
}
