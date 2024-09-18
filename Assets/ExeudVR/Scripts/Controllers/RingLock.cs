using UnityEngine;

/// <summary>
/// Experimental
/// </summary>
public class RingLock : MonoBehaviour
{
    public delegate void RingLockDelegate(bool locked);
    public event RingLockDelegate locked;

    private void OnTriggerEnter(Collider other)
    {
        locked.Invoke(true);    
    }

    private void OnTriggerExit(Collider other)
    {
        locked.Invoke(false);
    }
}
