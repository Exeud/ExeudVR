using UnityEngine;
using WebXR;

namespace ExeudVR
{
    /// <summary>
    /// The RigController acts as an adjustable pivot between the head and hands.
    /// It is useful for body-oriented events and functions, and connects the HUD.
    /// It also handles placement of the foot plate, which keeps the character grounded.
    /// </summary>
    public class RigController : MonoBehaviour
    {
        [SerializeField] private Transform cameraReference;
        [SerializeField] private GameObject HUDObjectRoot;
        [Range(1f, 10f)]
        [SerializeField] private float HUDSnappiness = 3;
        [SerializeField] private GameObject footPlate;
        [SerializeField] private LayerMask footLayerMask;

        private Vector3 BodyOffset;
        private Vector3 UiOffset;
        private Quaternion UiStartRot;
        private Vector3 footTarget;

        private WebXRState xrState = WebXRState.NORMAL;

        private void OnEnable()
        {
            WebXRManager.OnXRChange += OnXRChange;
        }

        private void OnDisable()
        {
            WebXRManager.OnXRChange -= OnXRChange;
        }

        public void Start()
        {
            BodyOffset = (cameraReference.position - transform.position) / 2f;

            UiOffset = HUDObjectRoot.transform.position - transform.position;
            UiStartRot = HUDObjectRoot.transform.localRotation;
        }

        private void Update()
        {
            float attitude = cameraReference.localRotation.eulerAngles.y;
            transform.localPosition = cameraReference.localPosition - BodyOffset;
            transform.localRotation = Quaternion.Euler(0f, attitude, 0f);
            footPlate.transform.localRotation = Quaternion.Euler(0f, attitude , 0f);

            bool hasHit = Physics.Raycast(transform.position + (transform.up * -1.3f), Vector3.down, out RaycastHit hit, 10f, footLayerMask);

            if (hasHit) 
            {
                // casting for terrain descent
                footTarget = new Vector3(transform.position.x, hit.point.y + (transform.up * 0.3f).y, transform.position.z - 0.2f);
                footPlate.transform.position = footTarget;
            }
            else
            {
                // lifting lerp for terrain ascent
                footTarget = new Vector3(transform.position.x, footPlate.transform.position.y, transform.position.z - 0.2f);
                footPlate.transform.position = Vector3.Lerp(footPlate.transform.position, footTarget, 0.05f);
            }

            
        }

        private void FixedUpdate()
        {
            if (xrState == WebXRState.NORMAL)
            {
                if (HUDObjectRoot.activeInHierarchy)
                {
                    Vector3 hudTarget = transform.position + (transform.forward * UiOffset.z) + (transform.up * UiOffset.y) + transform.right * UiOffset.x;
                    HUDObjectRoot.transform.position = Vector3.Lerp(HUDObjectRoot.transform.position, hudTarget, Time.deltaTime * HUDSnappiness);
                    HUDObjectRoot.transform.rotation = Quaternion.LookRotation(transform.forward) * UiStartRot;
                }
            }
        }

        private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            xrState = state;
            transform.localRotation = Quaternion.identity;
            HUDObjectRoot.SetActive(xrState == WebXRState.NORMAL);
        }
    }
}
