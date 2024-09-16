using UnityEngine;
using UnityEngine.Events;

namespace ExeudVR
{
    public class PressableButton : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent TouchBegin = new UnityEvent();
        public UnityEvent TouchEnd = new UnityEvent();
        public UnityEvent ButtonPressed = new UnityEvent();
        public UnityEvent ButtonReleased = new UnityEvent();
    }
}