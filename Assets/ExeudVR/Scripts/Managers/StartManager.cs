using UnityEngine;
using UnityEngine.Events;

namespace ExeudVR
{
    public class StartManager : MonoBehaviour
    {
        private static StartManager _instance;
        public static StartManager Instance { get { return _instance; } }

        [SerializeField] private UnityEvent OnInitialisation = new UnityEvent();

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
            if (!hasStarted && other.attachedRigidbody.gameObject.GetComponentInChildren<Camera>())
            {
                hasStarted = true;
                OnInitialised?.Invoke();
                OnInitialisation?.Invoke();
            }
        }

    }
}