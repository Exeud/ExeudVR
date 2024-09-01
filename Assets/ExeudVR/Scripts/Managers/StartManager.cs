using UnityEngine;

namespace ExeudVR
{
    public class StartManager : MonoBehaviour
    {
        private static StartManager _instance;
        public static StartManager Instance { get { return _instance; } }

        private bool hasStarted = false;

        public delegate void InitialisationEvent();
        public event InitialisationEvent OnInitialised;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!hasStarted && other.gameObject.layer == LayerMask.NameToLayer("Body"))
            {
                hasStarted = true;
                OnInitialised?.Invoke();
            }
        }

    }
}