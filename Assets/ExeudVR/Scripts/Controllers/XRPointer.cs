using UnityEngine;

namespace ExeudVR
{
    public class XRPointer : MonoBehaviour
    {
        [SerializeField] private float MaxInteractionDistance = 15.0f;
        [SerializeField] private LayerMask PointerLayerMask;
        [SerializeField] private Transform PointerLine;

        // 'RingLock' is an experimental idea for having a wider selection area at a greater distance.
        // Might be introduced in a later version.
        //[SerializeField] private GameObject PointerRing;

        private GameObject currentObject;
        private Transform parentHand;
        private bool HasRingLock;

        private void OnEnable()
        {
            //PointerRing.GetComponent<RingLock>().locked += SetRingLock;
        }

        private void OnDisable()
        {
            //PointerRing.GetComponent<RingLock>().locked -= SetRingLock;
        }

        private void Start()
        {
            parentHand = transform.parent;   
        }

        private void SetRingLock(bool locked)
        {
            HasRingLock = locked;
        }

        public GameObject PlacePointer()
        {
            Vector3 pointerPos = CastControllerRay();
            transform.position = pointerPos;
            transform.rotation = parentHand.rotation;
            PointerLine.rotation = parentHand.rotation;
            return currentObject;
        }

        private Vector3 CastControllerRay()
        {
            Vector3 rayStart = parentHand.position + (parentHand.forward * 0.3f);
            Ray handRay = new Ray(rayStart, parentHand.forward);
            Vector3 currentHitPoint = parentHand.position + (parentHand.forward * -0.125f);

            if (Physics.Raycast(handRay, out RaycastHit newHit, MaxInteractionDistance, PointerLayerMask))
            {
                currentObject = newHit.collider.gameObject;
                //PointerRing.transform.localScale = new Vector3(newHit.distance, newHit.distance, 1.0f);
                currentHitPoint = newHit.point + (handRay.direction.normalized * -0.02f);
            }
            else if (HasRingLock)
            {
                currentHitPoint = parentHand.position + (parentHand.forward * transform.localPosition.z);
            }
            else
            {
                currentObject = null;
            }

            return currentHitPoint;
        }
    }
}