using UnityEngine;

namespace ExeudVR
{
    public class CursorManager : MonoBehaviour
    {
        // Singleton
        private static CursorManager _instance;
        public static CursorManager Instance { get { return _instance; } }

        public bool isGameMode { get; private set; }

        // Event handling   
        public event BodyController.CursorFocus OnObjectFocus;
        public event BodyController.ObjectTrigger OnObjectTrigger;

        // Inspector Objects
        [Tooltip("Visualise pointers in the Editor?")]
        [SerializeField] private bool DebugCursorSelection;

        [Tooltip("Default cursor for non-interactive elements")]
        [SerializeField] private Texture2D cursorForScene;

        [Tooltip("Cursor for things that can be thrown around")]
        [SerializeField] private Texture2D cursorForObjects;

        [Tooltip("Cursor for fixed interactables")]
        [SerializeField] private Texture2D cursorForControls;

        [Tooltip("Crosshair reference. See sister GameObject")]
        [SerializeField] private CentreCrosshair crosshair;


        // Private Variables
        private Vector2 hotspot = new Vector2(10, 5);
        private readonly CursorMode cMode = CursorMode.ForceSoftware;

        private GameObject focusedObject;
        private XRState xrState;

        private int GetLaterInt(string layerName)
        {
            return LayerMask.NameToLayer(layerName);
        }

        private void Awake()
        {
            _instance = this;
            xrState = XRState.NORMAL;

        }

        private void Start()
        {
            SetCrosshairVisibility(isGameMode);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToggleGameMode();
            }
        }

        void OnGUI()
        {
            if (xrState != XRState.NORMAL) { return; }

            if (!Application.isEditor || DebugCursorSelection)
            {
                SetCursorImage();
            }
        }

        private void ToggleGameMode()
        {
            isGameMode = !isGameMode;
            DesktopController.Instance.IsGameMode = isGameMode;

            SetCrosshairVisibility(isGameMode);
            SetCursorParameters(xrState);
        }

        public void SetCrosshairVisibility(bool visibility)
        {
            isGameMode = visibility;
            int isActive = (isGameMode ? 1 : 0) * 255;
            crosshair.SetColor(CrosshairColorChannel.ALPHA, isActive, true);
            crosshair.SetActive(visibility);
        }

        public void HandleCursorFocus(GameObject inFocus)
        {
            if (inFocus == null)
            {
                focusedObject = inFocus;
                return;
            }
            
            if (inFocus != focusedObject)
            {
                // lose old focus
                if (focusedObject && focusedObject.TryGetComponent(out ObjectInterface oiOld))
                {
                    oiOld.ToggleActivation(gameObject, false);
                }

                // get new focus
                if (inFocus.TryGetComponent(out ObjectInterface oiNew))
                {
                    oiNew.ToggleActivation(gameObject, true);
                }

                focusedObject = inFocus;
            }
        }

        public void DoubleClick()
        {
            if (focusedObject != null)
            {
                if (focusedObject.TryGetComponent(out ObjectInterface objInt))
                {
                    OnObjectTrigger?.Invoke(objInt, 0.75f);
                }
            }
        }

        public void SetCursorImage()
        {
            if (focusedObject != null)
            {
                SetCursorImageFromLayer(focusedObject.layer);
            }
            else
            {
                SetDefaultCursor();
            }
        }

        public void SetCursorParameters(XRState state)
        {
            xrState = state;

            if (state != XRState.NORMAL)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = isGameMode ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !isGameMode;
            }
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(cursorForScene, hotspot, cMode);
            if (isGameMode)
            {   
                crosshair.SetSize(14, true);
                crosshair.SetThickness(1, true);
                crosshair.SetGap(6, true);
            }
        }

        private void SetCursorImageFromLayer(int objectLayer)
        {
            string thisLayer = LayerMask.LayerToName(objectLayer);

            if (string.Equals(thisLayer, "Affordance") || string.Equals(thisLayer, "Buttons"))
            {
                Cursor.SetCursor(cursorForControls, hotspot, cMode);
                if (isGameMode)
                {   // a thin square
                    crosshair.SetSize(1, true);
                    crosshair.SetThickness(16, true);
                    crosshair.SetGap(8, true);
                }
            }
            else if (string.Equals(thisLayer, "Objects") || string.Equals(thisLayer, "Tools"))
            {
                Cursor.SetCursor(cursorForObjects, hotspot, cMode);
                if (isGameMode)
                {   // wide crosshair
                    crosshair.SetSize(14, true);
                    crosshair.SetThickness(1, true);
                    crosshair.SetGap(18, true);
                }
            }
            else if (string.Equals(thisLayer, "Furniture") || string.Equals(thisLayer, "Scene"))
            {
                Cursor.SetCursor(cursorForScene, hotspot, cMode);
                if (isGameMode)
                {   // narrow, thicker crosshair
                    crosshair.SetSize(6, true);
                    crosshair.SetThickness(2, true);
                    crosshair.SetGap(6, true);
                }
            }
            else
            {
                // defaults to `cursorForScene`
                SetDefaultCursor();
            }
        }
    }
}